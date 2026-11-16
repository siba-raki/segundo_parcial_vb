Desarrollo de una API REST con Visual Basic .NET y Entity Framework Core
Tecnologías: Visual Basic .NET, ASP.NET Core Web API, Entity Framework Core,
SQL Server y Swagger.

# 1. Objetivos
Desarrollar una API REST para administrar un sistema de inventario, aplicando los
conocimientos adquiridos durante las clases.
Implementar operaciones CRUD, relacionar entidades, aplicar validaciones y reglas de negocio, organizar correctamente el proyecto y documentar y probar los endpoints mediante Swagger.
# 2. Enunciado
Una empresa necesita desarrollar una API REST para administrar sus productos y categorías.
Actualmente, la empresa registra sus productos sin una clasificación definida, lo que dificulta su organización y consulta.
Desarrollar una API REST que permita administrar las categorías y los productos, garantizando la integridad de la información mediante relaciones entre entidades y reglas de negocio.

El proyecto deberá desarrollarse utilizando Visual Basic .NET y SQL Server con Entity Framework Core.

## Requerimientos funcionales
### 3.1. Administración de categorías
Implementar las operaciones necesarias para administrar las categorías.
La entidad Categoria deberá contemplar:
• Identificador.
• Nombre.
• Descripción.
Operaciones requeridas:
4. Consultar todas las categorías.
5. Consultar una categoría por identificador.
6. Registrar una categoría.
7. Modificar una categoría.
8. Eliminar una categoría, siempre que no tenga productos asociados.
### 3.2. Administración de productos
Implementar las operaciones CRUD para administrar productos.
La entidad Producto deberá contemplar:
• Identificador.
• Nombre.
• Descripción.
• Precio.
• Stock.
• Fecha de registro.
• Categoría.
Operaciones requeridas:
9. Consultar todos los productos.
10. Consultar un producto por identificador.
11. Registrar un producto.
12. Modificar un producto.
13. Eliminar un producto.
### 3.3. Relaciones entre entidades
Implementar una relación uno a muchos (1):
• Una categoría puede tener varios productos.
• Cada producto debe pertenecer a una categoría.
El alumno deberá utilizar Entity Framework Core para configurar la relación y
generar la migración correspondiente.
### 3.4. Consultas relacionadas
Implementar las siguientes consultas:
14. Consultar productos incluyendo los datos de su categoría.
15. Consultar productos pertenecientes a una categoría específica.
16. Consultar una categoría incluyendo sus productos.
Se deberá utilizar Include() cuando corresponda para recuperar la información
relacionada.
17. DTOs y validaciones
La API deberá utilizar DTOs para recibir y devolver información.
Se solicita implementar DTOs para:
• Registrar productos.
• Modificar productos.
• Responder consultas de productos.
• Registrar categorías.
• Modificar categorías.
• Responder consultas de categorías.
Aplicar las validaciones necesarias:
Campo Validación
Nombre del producto Obligatorio, máximo 50 caracteres
Descripción Máximo 200 caracteres
Precio Mayor que cero
Stock No puede ser negativo
Categoría Debe ser obligatoria y existir
Nombre de categoría Obligatorio, máximo 50 caracteres
18. Reglas de negocio
Implementar las siguientes reglas:
Regla 1. Productos duplicados
No se permite registrar dos productos con el mismo nombre. La comparación
deberá ignorar mayúsculas y minúsculas.
Regla 2. Categorías duplicadas
No se permite registrar dos categorías con el mismo nombre.
Regla 3. Categoría inexistente
No se permite registrar ni modificar un producto utilizando una categoría que no
exista.
Regla 4. Eliminación de categorías
No se permite eliminar una categoría que tenga productos asociados.
Regla 5. Stock
No se permite registrar ni modificar un producto con stock negativo.
Las reglas de negocio deberán implementarse en la capa correspondiente,
respetando la separación de responsabilidades.
19. Arquitectura del proyecto
El proyecto deberá mantener una estructura organizada, utilizando las siguientes
capas:
• Controllers.
• Services.
• Repositories.
• Models.
• DTOs.
• Data.
• Middleware.
Consideraciones:
• Los Controllers no deberán acceder directamente al DbContext.
• Los Services deberán contener las reglas de negocio.
• Los Repository deberán encargarse del acceso a datos.
• Los DTOs deberán separar los datos de entrada y salida de las entidades.
• Se deberá utilizar inyección de dependencias.
20. Manejo de errores y respuestas HTTP
La API deberá utilizar códigos HTTP apropiados según el resultado de cada operación.
Implementar el manejo centralizado de excepciones mediante el mecanismo de manejo global trabajado en clase.
No se deberá devolver información interna de las excepciones al consumidor de la API.
Documentar y probar los endpoints utilizando Swagger, se deberán demostrar como mínimo las siguientes pruebas:
22. Registrar una categoría.
23. Registrar un producto asociado a una categoría.
24. Consultar todos los productos incluyendo su categoría.
25. Consultar productos por categoría.
26. Intentar registrar un producto duplicado.
27. Intentar registrar un producto con una categoría inexistente.
28. Intentar eliminar una categoría que tenga productos asociados.
29. Modificar un producto.
30. Eliminar un producto.

Presentar:
32. Proyecto completo desarrollado en Visual Basic .NET.
33. Script SQL de la base de datos o migraciones necesarias para reconstruirla.
34. Código fuente organizado según la arquitectura solicitada.
35. Evidencias de las pruebas realizadas desde Swagger.
36. Archivo README con instrucciones para configurar y ejecutar el proyecto.
37. Enlace al repositorio GitHub del proyecto.
