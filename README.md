# n5-challenge
El proyecto "n5-challenge" es una aplicación que aborda varios aspectos de desarrollo web y servicios, implementando operaciones CRUD a través de una Web API y utilizando patrones como Repository, Unit of Work, DataAccess, CQRS, así como servicios de Elasticsearch y Kafka.

##Estructura del Proyecto

a) n5.WebApi
Este proyecto contiene la implementación de un servicio REST con un controlador llamado PermissionsController. Aquí se definen las operaciones para obtener, chequear y modificar permisos.

b) n5.Infrastructure
Este assembly proporciona la implementación base de los patrones Repository, Unit of Work, DataAccess y CQRS. Además, contiene la definición de las tablas de las bases de datos en la carpeta models. Como mejora potencial, se podría considerar trasladar los modelos a otro assembly. En este proyecto también se implementan los servicios de Elasticsearch y Kafka. Es importante destacar que todos los servicios se implementan mediante interfaces para facilitar la aplicación del patrón de inyección de dependencias y la realización de pruebas unitarias.

c) n5.Application
En este assembly se encuentra la lógica funcional de la aplicación. Destaca el servicio PermissionsService, al cual se le inyectan las interfaces de los repositorios, Elasticsearch y Kafka.

d) n5.Core
Este assembly, aunque actualmente no se utiliza, podría ser reservado para funcionalidades de base en caso de que el proyecto crezca y sea necesario contar con funciones adicionales.

e) n5.Tests
Este assembly contiene un juego de tests unitarios. Como mejora deberíamos poder tener muchos mas y lograr una elevada cobertura de código.

## Hablemos de Docker
