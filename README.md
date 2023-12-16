# n5-challenge
El proyecto "n5-challenge" es una aplicación que aborda varios aspectos de desarrollo web y servicios, implementando operaciones CRUD a través de una Web API y utilizando patrones como Repository, Unit of Work, DataAccess, CQRS, así como servicios de Elasticsearch y Kafka.

## Estructura del Proyecto

**a) n5.WebApi**
Este proyecto contiene la implementación de un servicio REST con un controlador llamado PermissionsController. Aquí se definen las operaciones para obtener, chequear y modificar permisos.

**b) n5.Infrastructure**
Este assembly proporciona la implementación base de los patrones Repository, Unit of Work, DataAccess y CQRS. Además, contiene la definición de las tablas de las bases de datos en la carpeta models. Como mejora potencial, se podría considerar trasladar los modelos a otro assembly. En este proyecto también se implementan los servicios de Elasticsearch y Kafka. Es importante destacar que todos los servicios se implementan mediante interfaces para facilitar la aplicación del patrón de inyección de dependencias y la realización de pruebas unitarias.

**c) n5.Application**
En este assembly se encuentra la lógica funcional de la aplicación. Destaca el servicio PermissionsService, al cual se le inyectan las interfaces de los repositorios, Elasticsearch y Kafka.

**d) n5.Core**
Este assembly, aunque actualmente no se utiliza, podría ser reservado para funcionalidades de base en caso de que el proyecto crezca y sea necesario contar con funciones adicionales.

**e) n5.Tests**
Este assembly contiene un juego de tests unitarios. Como mejora deberíamos poder tener muchos mas y lograr una elevada cobertura de código.

## Dockerización del Proyecto
Este proyecto incorpora la implementación de Docker para facilitar su despliegue y ejecución en entornos de desarrollo. A continuación, se describen los elementos clave relacionados con Docker.
** a) Dockerfile **
El archivo Dockerfile empaqueta la solución .NET en una imagen Docker, proporcionando un entorno consistente para la ejecución de la aplicación.
** b) dockercompose.yaml **
En la carpeta "project", encontrarás un archivo docker-compose.yaml diseñado para definir todos los servicios necesarios para un despliegue efectivo del proyecto en entornos de desarrollo. Este archivo de composición define los siguientes servicios:
- sqlserver: La definición del contenedor Docker de SQL Server, con variables de entorno para la primera instancia, la definición del volume donde se crea la base de datos.
- challenge-n5: El servicio principal definido en el desafío. Configura variables de entorno para construir la cadena de conexión al servidor SQL Server.
- elastic-search: Implementación del contenedor Docker de Elasticsearch. Se configuran los volúmes para apuntar a un directorio en la máquina local.
-kafka: Configuración del contenedor Docker de Kafka, incluida la definición del volumen.

### Makefile
Se ha implementado un Makefile para dinamizar aún más el proceso de gestión de los contenedores. Algunos de los comandos disponibles incluyen:
**up:** Levanta todos los contenedores definidos en docker-compose.yaml.
**down:** Detiene todos los contenedores definidos en el archivo de composición.
**up_build:** Detiene los contenedores, realiza la construcción de todos los contenedores y luego los reinicia.
**up_sql:** Levanta únicamente el servicio de SQL Server.
**up_elastic:** Levanta únicamente el servicio de Elasticsearch.
**up_kafka:** Levanta únicamente el servicio de Kafka.

## Hablemos de Kubernetes


