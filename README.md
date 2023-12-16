# n5-challenge
El proyecto "n5-challenge" es una aplicación que aborda varios aspectos de desarrollo web y servicios, implementando operaciones CRUD a través de una Web API y utilizando patrones como Repository, Unit of Work, DataAccess, CQRS, así como servicios de Elasticsearch y Kafka.

## Estructura del Proyecto

__a) n5.WebApi__
Este proyecto contiene la implementación de un servicio REST con un controlador llamado PermissionsController. Aquí se definen las operaciones para obtener, chequear y modificar permisos.

__b) n5.Infrastructure__
Este assembly proporciona la implementación base de los patrones Repository, Unit of Work, DataAccess y CQRS. Además, contiene la definición de las tablas de las bases de datos en la carpeta models. Como mejora potencial, se podría considerar trasladar los modelos a otro assembly. En este proyecto también se implementan los servicios de Elasticsearch y Kafka. Es importante destacar que todos los servicios se implementan mediante interfaces para facilitar la aplicación del patrón de inyección de dependencias y la realización de pruebas unitarias.

__c) n5.Application__
En este assembly se encuentra la lógica funcional de la aplicación. Destaca el servicio PermissionsService, al cual se le inyectan las interfaces de los repositorios, Elasticsearch y Kafka.

__d) n5.Core__
Este assembly, aunque actualmente no se utiliza, podría ser reservado para funcionalidades de base en caso de que el proyecto crezca y sea necesario contar con funciones adicionales.

__e) n5.Tests__
Este assembly contiene un juego de tests unitarios. Como mejora deberíamos poder tener muchos mas y lograr una elevada cobertura de código.

## Dockerización del Proyecto
Este proyecto incorpora la implementación de Docker para facilitar su despliegue y ejecución en entornos de desarrollo. A continuación, se describen los elementos clave relacionados con Docker.

__a) Dockerfile__

El archivo Dockerfile empaqueta la solución .NET en una imagen Docker, proporcionando un entorno consistente para la ejecución de la aplicación.

__b) dockercompose.yaml__

En la carpeta "project", encontrarás un archivo docker-compose.yaml diseñado para definir todos los servicios necesarios para un despliegue efectivo del proyecto en entornos de desarrollo. Este archivo de composición define los siguientes servicios:
- sqlserver: La definición del contenedor Docker de SQL Server, con variables de entorno para la primera instancia, la definición del volume donde se crea la base de datos.
- challenge-n5: El servicio principal definido en el desafío. Configura variables de entorno para construir la cadena de conexión al servidor SQL Server.
- elastic-search: Implementación del contenedor Docker de Elasticsearch. Se configuran los volúmes para apuntar a un directorio en la máquina local.
-kafka: Configuración del contenedor Docker de Kafka, incluida la definición del volumen.

### Makefile
Se ha implementado un Makefile para dinamizar aún más el proceso de gestión de los contenedores. Algunos de los comandos disponibles incluyen:
- __up:__ Levanta todos los contenedores definidos en docker-compose.yaml.
- __down:__ Detiene todos los contenedores definidos en el archivo de composición.
- __up_build:__ Detiene los contenedores, realiza la construcción de todos los contenedores y luego los reinicia.
- __up_sql:__ Levanta únicamente el servicio de SQL Server.
- __up_elastic:__ Levanta únicamente el servicio de Elasticsearch.
- __up_kafka:__ Levanta únicamente el servicio de Kafka.

## Hablemos de Kubernetes
Para desplegar los contenedores en kubernetes deberiamos seguir los siguientes pasos

- __Crear un namespace__

```bash
kubectl create namespace n5-namesapace
```
- __Crear los secretes__

```bash
kubectl create secret generic sqlserver-secret --from-literal=PASS=Challenge!N5
```
- __Crear el ConfigMap__
```bash
kubectl create configmap sqlserver-configmap --from-literal==DBNAME=challengeN5 --from-literal=SERVER=sql-service --from-literal=USER=sa
```
- __Creamos el servicio de SQL Server__

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: sqlserver
  namespace: n5-namespace
spec:
  replicas: 1
  selector:
    matchLabels:
      app: sqlserver
  template:
    metadata:
      labels:
        app: sqlserver
    spec:
      containers:
      - name: sqlserver
        image: mcr.microsoft.com/mssql/server:2019-latest
        ports:
        - containerPort: 1433
        env:
        - name: ACCEPT_EULA
          value: Y
        - name: SA_PASSWORD
          valueFrom:
            secretKeyRef:
              name: sqlserver-secret
              key: PASS
        - name: MSSQL_PID
          value: Developer
        volumeMounts:
        - name: sqlserver-data
          mountPath: /var/opt/mssql/data

      volumes:
      - name: sqlserver-data
        hostPath:
          path: /path/to/db-data/sqlserver
```

```yaml
apiVersion: v1
kind: Service
metadata:
  name: sqlserver-service
  namespace: n5-namespace
spec:
  selector:
    app: sqlserver
  ports:
  - protocol: TCP
    port: 1433
    targetPort: 1433
```

- __Creamos el servicio de n5-challenge__

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: challenge-n5
  namespace: n5-namespace
spec:
  replicas: 1
  selector:
    matchLabels:
      app: challenge-n5
  template:
    metadata:
      labels:
        app: challenge-n5
    spec:
      containers:
      - name: challenge-n5
        image: n5/pc-challenge:latest
        ports:
        - containerPort: 80
        env:
        - name: DBServer
          value: sqlserver
        - name: DBName
          valueFrom:
            configMapKeyRef:
              name: sqlserver-configmap
              key: DBNAME
        - name: DBUser
          valueFrom:
            configMapKeyRef:
              name: sqlserver-configmap
              key: USER
        - name: DBPassword
          valueFrom:
            secretKeyRef:
              name: sqlserver-secret
              key: PASS
```

```yaml
apiVersion: v1
kind: Service
metadata:
  name: challenge-n5-service
  namespace: n5-namespace
spec:
  selector:
    app: challenge-n5
  ports:
  - protocol: TCP
    port: 80
    targetPort: 80
```

- __Creamos el servicio de elastic search __
```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: elastic-search
  namespace: n5-namespace
spec:
  replicas: 1
  selector:
    matchLabels:
      app: elastic-search
  template:
    metadata:
      labels:
        app: elastic-search
    spec:
      containers:
      - name: elastic-search
        image: docker.elastic.co/elasticsearch/elasticsearch:8.11.3
        ports:
        - containerPort: 9200
        env:
        - name: node.name
          value: es01
        - name: cluster.name
          value: es-docker-cluster
        - name: discovery.seed_hosts
          value: es01
        - name: cluster.initial_master_nodes
          value: es01
        - name: bootstrap.memory_lock
          value: "true"
        - name: ES_JAVA_OPTS
          value: "-Xms512m -Xmx512m"
        volumeMounts:
        - name: elastic-search-data
          mountPath: /usr/share/elasticsearch/data

      volumes:
      - name: elastic-search-data
        hostPath:
          path: /path/to/db-data/elastic
```

```yaml
apiVersion: v1
kind: Service
metadata:
  name: elastic-search-service
  namespace: n5-namespace
spec:
  selector:
    app: elastic-search
  ports:
  - protocol: TCP
    port: 9200
    targetPort: 9200
```

- __Creamos el servicio de Kafka__
```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: kafka
  namespace: n5-namespace
spec:
  replicas: 1
  selector:
    matchLabels:
      app: kafka
  template:
    metadata:
      labels:
        app: kafka
    spec:
      containers:
      - name: kafka
        image: docker.io/bitnami/kafka:3.6
        ports:
        - containerPort: 9092
        env:
        - name: KAFKA_CFG_NODE_ID
          value: "0"
        - name: KAFKA_CFG_PROCESS_ROLES
          value: "controller,broker"
        - name: KAFKA_CFG_CONTROLLER_QUORUM_VOTERS
          value: "0@kafka:9093"
        - name: KAFKA_CFG_LISTENERS
          value: "PLAINTEXT://:9092,CONTROLLER://:9093"
        - name: KAFKA_CFG_ADVERTISED_LISTENERS
          value: "PLAINTEXT://:9092"
        - name: KAFKA_CFG_LISTENER_SECURITY_PROTOCOL_MAP
          value: "CONTROLLER:PLAINTEXT,PLAINTEXT:PLAINTEXT"
        - name: KAFKA_CFG_CONTROLLER_LISTENER_NAMES
          value: "CONTROLLER"
        - name: KAFKA_CFG_INTER_BROKER_LISTENER_NAME
          value: "PLAINTEXT"
        volumeMounts:
        - name: kafka-data
          mountPath: /bitnami

      volumes:
      - name: kafka-data
        hostPath:
          path: /path/to/db-data/kafka
```

```yaml
apiVersion: v1
kind: Service
metadata:
  name: kafka-service
  namespace: n5-namespace
spec:
  selector:
    app: kafka
  ports:
  - protocol: TCP
    port: 9092
    targetPort: 9092
```

### Mejoras a Kubernetes
Se recomienda realizar las siguientes mejoras.
- Crear un networkpoclicy al pod sqlserver que solamente acepte un ingress del pod challenge-n5.
- Crear los persistent volumes y los persisten volumes claims. Definiendo espacio correctamente
- En el pod challenge-n5, en la solucion definir un recurso por ejemplo health para que en la definición del pod configurar correctamente las propiedades readinessProbe y livenessProbe.


