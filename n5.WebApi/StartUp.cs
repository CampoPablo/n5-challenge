using Microsoft.EntityFrameworkCore;
using n5.Infrastructure.DbContext;
using n5.Infrastructure.Repository;
using n5.Infrastructure.UnitOfWork;
using n5.Application.Services;
using n5.Infrastructure.ElasticSearch;
using Serilog;
using n5.Infrastructure.Kafka;
using Confluent.Kafka;

namespace n5.WebApi
{
    public class Startup
    {
        private bool envDevelopment = false;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            envDevelopment = env.IsDevelopment();
            // Configuración de Serilog
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // Este método se llama en tiempo de ejecución. Utiliza este método para agregar servicios al contenedor.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configuración de Entity Framework y cadena de conexión
            if (envDevelopment)
            {
                // Configuración para el entorno de desarrollo
                services.AddDbContext<N5DbContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                });
            }
            else
            {
                // Configuración para otros entornos (por ejemplo, Production)
                var connectionString = Configuration.GetConnectionString("ProductionConnection");
                services.AddDbContext<N5DbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });
            }

            // Configuración de Kafka
            var kafkaBootstrapServers = Configuration["Kafka:BootstrapServers"];
            services.AddSingleton<IKafkaProducer>(provider => new KafkaProducer(kafkaBootstrapServers));

            // Configuración de la inyección de dependencias para el DbContext
            services.AddScoped<IMyApiDbContext, N5DbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPermissionServices, PermissionServices>();

            // Registrar el servicio de ElasticSearch con la URI obtenida
            var elasticSearchUri = Configuration.GetConnectionString("ElasticSearchUri");

            services.AddScoped<IElasticSearchService>(provider =>
            {
                var uri = elasticSearchUri ?? throw new ArgumentNullException(nameof(elasticSearchUri));
                return new ElasticSearchService(uri);
            });

            services.AddSingleton<Serilog.Extensions.Hosting.DiagnosticContext>();
            // Otros servicios y configuraciones pueden agregarse aquí
            services.AddControllers();
        }

        // Este método se llama en tiempo de ejecución. Utiliza este método para configurar el pipeline de la solicitud HTTP.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, N5DbContext context)
        {
            //    ConfigureServices((IServiceCollection)app.ApplicationServices, env);
            if (env.IsDevelopment())
            {
                envDevelopment = true;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Configuraciones adicionales para entorno de producción
                // Puedes personalizar el manejo de errores aquí
            }

            // Crea la base de datos si no existe
            context.Database.EnsureCreated();
            context.Database.Migrate();
            app.UseRouting();

            // Configuración para que Serilog maneje las solicitudes HTTP
            app.UseSerilogRequestLogging();

            // Configuraciones adicionales del middleware pueden ir aquí

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
