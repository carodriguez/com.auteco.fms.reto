Descripción General
Este proyecto implementa un Sistema de Gestión de Servicios Técnicos (Field Service Management) para administrar órdenes de servicio, técnicos, clientes y servicios. La aplicación permite seguir todo el ciclo de vida de las órdenes de servicio, desde su creación hasta su finalización o cancelación.
Tecnologías Utilizadas
Backend
•	.NET 8: Framework más reciente de Microsoft para el desarrollo de aplicaciones.
•	ASP.NET Core: Utilizado para implementar la arquitectura web.
•	Razor Pages: Para la creación de vistas dinámicas y reutilizables.
•	Entity Framework Core: ORM para el manejo de la persistencia de datos.
•	AutoMapper: Para el mapeo entre objetos de dominio y DTOs.
Frontend
•	Bootstrap 5: Framework CSS para diseño responsivo.
•	jQuery: Biblioteca JavaScript para manipulación del DOM.
•	DataTables: Plugin de jQuery para tablas dinámicas con funciones avanzadas.
•	SweetAlert2: Biblioteca para mostrar alertas y diálogos mejorados.
•	Moment.js: Para manipulación y formateo de fechas.
Arquitectura
El sistema está diseñado siguiendo los principios de Clean Architecture y Domain-Driven Design (DDD), permitiendo una clara separación de responsabilidades y facilitando el mantenimiento y la escalabilidad:
Capas
1.	Fsm.Domain:
•	Contiene las entidades de negocio (Cliente, Técnico, OrdenDeServicio, etc.)
•	Define enumeraciones y constantes del dominio
•	Implementa reglas de negocio específicas
2.	Fsm.Application:
•	Implementa casos de uso y lógica de aplicación
•	Define interfaces de servicios y repositorios
•	Contiene DTOs para la transferencia de datos
•	Gestiona mapeos con AutoMapper
3.	Fsm.Adapters.Persistence:
•	Implementa repositorios definidos en Application
•	Configura el contexto de Entity Framework
•	Maneja la persistencia en base de datos
•	Implementa el patrón Unit of Work
4.	Fsm.WebApp:
•	Implementa la interfaz de usuario con ASP.NET Core
•	Contiene controladores, vistas y recursos web
•	Gestiona la interacción con el usuario
Patrones Implementados
•	Repository Pattern: Para abstraer el acceso a datos
•	Unit of Work: Para gestionar transacciones y cambios en la base de datos
•	Mediator: Para desacoplar componentes y gestionar la comunicación entre capas
•	DTO (Data Transfer Objects): Para transferir datos entre capas sin exponer las entidades del dominio
Funcionalidades Principales
Gestión de Órdenes de Servicio
•	Creación, edición y eliminación de órdenes
•	Seguimiento del estado (Pendiente, En Proceso, Finalizada, Cancelada)
•	Asignación de técnicos y servicios
•	Gestión de prioridades y categorías
•	Vista detallada con información completa
Gestión de Clientes
•	Registro y mantenimiento de información de clientes
•	Clasificación por tipo de cliente
Gestión de Servicios
•	Catálogo de servicios disponibles
•	Asignación a órdenes de servicio
Mejores Prácticas Implementadas
1.	Validación de Datos: Uso de Data Annotations para validar entrada de usuarios
2.	Manejo de Errores: Implementación de respuestas consistentes mediante el tipo Result<T>

4.	Experiencia de Usuario:
•	Interfaces intuitivas y responsivas
•	Feedback inmediato mediante notificaciones
•	Filtros avanzados para búsqueda de información
5.	Mantenibilidad:
•	Código documentado con comentarios XML
•	Separación clara de responsabilidades
•	Uso de interfaces para facilitar testing y extensibilidad
Flujo de Trabajo Típico
1.	Registro de cliente en el sistema
2.	Creación de orden de servicio asociada al cliente
3.	Asignación de técnico y servicios requeridos
4.	Seguimiento del estado de la orden (cambios de estado)
5.	Finalización o cancelación de la orden
Requisitos del Sistema
•	.NET SDK 8.0 o superior
•	SQL Server (u otro RDBMS compatible con EF Core)
•	Navegador web moderno
