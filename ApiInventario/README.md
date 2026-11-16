# ApiInventario - API REST de inventario (Segundo Parcial)

API REST para administrar **productos** y **categorías** de un sistema de inventario,
desarrollada en **Visual Basic .NET** con **ASP.NET Core Web API**, **Entity Framework Core**,
**SQL Server** y **Swagger**.

## 1. Tecnologías

| Tecnología | Versión / detalle |
|------------|-------------------|
| Visual Basic .NET | .NET 10 (`net10.0`) |
| ASP.NET Core Web API | Controllers con inyección de dependencias |
| Entity Framework Core | 10.0.0 (Code First + Migraciones) |
| SQL Server | Provider `Microsoft.EntityFrameworkCore.SqlServer` |
| Swagger | Swashbuckle.AspNetCore 10.3.1 (OpenAPI 3) |

## 2. Estructura del proyecto

```
SegundoParcial/
├── consigna.md
└── ApiInventario/
    ├── ApiInventario.vbproj
    ├── Program.vb                     # Composition root: DI, DbContext, Swagger, middleware
    ├── appsettings.json               # Cadena de conexión
    ├── appsettings.Development.json
    ├── script_db.sql                  # Script SQL para reconstruir la base
    ├── Controllers/
    │   ├── CategoriasController.vb    # Endpoints de categorías
    │   └── ProductosController.vb     # Endpoints de productos
    ├── Services/
    │   ├── ICategoriaService.vb / CategoriaService.vb      # Reglas de negocio de categorías
    │   └── IProductoService.vb / ProductoService.vb        # Reglas de negocio de productos
    ├── Repositories/
    │   ├── ICategoriaRepository.vb / CategoriaRepository.vb # Acceso a datos (DbContext)
    │   └── IProductoRepository.vb / ProductoRepository.vb
    ├── Models/
    │   ├── Categoria.vb
    │   └── Producto.vb
    ├── DTOs/
    │   ├── CategoriaCreateDTO.vb / CategoriaUpdateDTO.vb / CategoriaResponseDTO.vb
    │   ├── CategoriaDetalleDTO.vb
    │   ├── ProductoCreateDTO.vb / ProductoUpdateDTO.vb / ProductoResponseDTO.vb
    │   └── ErrorResponseDTO.vb
    ├── Data/
    │   └── AppDbContext.vb            # DbContext + configuración de la relación 1:N
    ├── Exceptions/
    │   ├── ReglaNegocioException.vb
    │   ├── EntidadDuplicadaException.vb
    │   ├── EntidadNoEncontradaException.vb
    │   └── OperacionNoPermitidaException.vb
    ├── Middleware/
    │   └── ExceptionHandlingMiddleware.vb  # Manejo global de excepciones
    └── Migrations/
        ├── 20261115194647_InitialCreate.vb
        └── AppDbContextModelSnapshot.vb
```

Separación de responsabilidades:

- **Controllers**: solo reciben la petición, delegan en el Service y traducen el resultado a códigos HTTP.
  No acceden al `DbContext`.
- **Services**: concentran las **reglas de negocio** y lanzan las excepciones de dominio.
- **Repositories**: encapsulan el **acceso a datos** (EF Core, `Include`, `Any`, `SaveChanges`).
- **DTOs**: separan los datos de entrada/salida de las entidades e incluyen las validaciones.
- **Middleware**: maneja de forma centralizada las excepciones y evita filtrar información interna.

## 3. Modelo de datos

**Categoria** (Id, Nombre, Descripcion) — **Producto** (Id, Nombre, Descripcion, Precio, Stock,
FechaRegistro, CategoriaId).

Relación **1 a N** (`Categoria` 1 —— N `Productos`) configurada con Entity Framework Core en
`AppDbContext.OnModelCreating`:

```vb
modelBuilder.Entity(Of Producto)().
    HasOne(Function(producto) producto.Categoria).
    WithMany(Function(categoria) categoria.Productos).
    HasForeignKey(Function(producto) producto.CategoriaId).
    OnDelete(DeleteBehavior.Restrict)
```

También se crean índices **únicos** por nombre (`IX_Categorias_Nombre`, `IX_Productos_Nombre`)
para blindar la integridad de los datos a nivel de base.

## 4. Endpoints

| Método | Endpoint | Descripción | Respuestas |
|--------|----------|-------------|------------|
| GET | `/api/categorias` | Consulta todas las categorías | 200 |
| GET | `/api/categorias/{id}` | Consulta una categoría por identificador | 200, 404 |
| GET | `/api/categorias/{id}/productos` | Consulta una categoría **con sus productos** | 200, 404 |
| POST | `/api/categorias` | Registra una categoría | 201, 400, 409 |
| PUT | `/api/categorias/{id}` | Modifica una categoría | 200, 400, 404, 409 |
| DELETE | `/api/categorias/{id}` | Elimina una categoría (solo si no tiene productos) | 200, 404, 409 |
| GET | `/api/productos` | Consulta todos los productos **incluyendo su categoría** | 200 |
| GET | `/api/productos/{id}` | Consulta un producto por identificador | 200, 404 |
| GET | `/api/productos/categoria/{categoriaId}` | Consulta los productos de una categoría | 200, 404 |
| POST | `/api/productos` | Registra un producto | 201, 400, 409 |
| PUT | `/api/productos/{id}` | Modifica un producto | 200, 400, 404, 409 |
| DELETE | `/api/productos/{id}` | Elimina un producto | 200, 404 |

Las consultas relacionadas usan `Include()` en los repositories:
`ObtenerTodos` / `ObtenerPorId` / `ObtenerPorCategoria` de productos incluyen `Categoria`,
y `ObtenerPorIdConProductos` de categorías incluye `Productos`.

## 5. Validaciones (DTOs)

| Campo | Validación |
|-------|------------|
| Nombre del producto | Obligatorio, máximo 50 caracteres |
| Descripción del producto | Máximo 200 caracteres |
| Precio | Mayor que cero |
| Stock | No puede ser negativo |
| Categoría | Obligatoria y debe existir en la base |
| Nombre de categoría | Obligatorio, máximo 50 caracteres |

Las validaciones de formato se resuelven con `DataAnnotations` sobre los DTOs y se devuelven
con código **400** y el detalle de los errores. Las validaciones que requieren consultar la base
(categoría existente) se resuelven en la capa de servicios.

## 6. Reglas de negocio

| Regla | Comportamiento | Código HTTP |
|-------|----------------|--------------|
| 1. Productos duplicados | No se registran dos productos con el mismo nombre (ignora mayúsculas/minúsculas) | 409 Conflict |
| 2. Categorías duplicadas | No se registran dos categorías con el mismo nombre | 409 Conflict |
| 3. Categoría inexistente | No se registra ni modifica un producto con una categoría que no existe | 400 Bad Request |
| 4. Eliminación de categorías | No se elimina una categoría que tenga productos asociados | 409 Conflict |
| 5. Stock | No se registra ni modifica un producto con stock negativo | 400 Bad Request |

Las reglas se implementan en `Services/` y se comunican con el exterior mediante las excepciones
de `Exceptions/`, que el `ExceptionHandlingMiddleware` traduce a la respuesta HTTP correspondiente.
Las excepciones internas nunca se exponen al consumidor de la API.

## 7. Instrucciones para ejecutar el proyecto

### 7.1 Requisitos previos

- .NET SDK 10.0 o superior
- SQL Server (local, Docker o Azure SQL)

Si no tenes SQL Server instalado, podes levantar uno con Docker:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Tu_Password123" -e "MSSQL_PID=Developer" \
  -d --name mssql-inventario -p 1433:1433 mcr.microsoft.com/mssql/server:2022-latest
```

### 7.2 Configurar la base de datos

Opción A — con las migraciones de Entity Framework Core (recomendado):

```bash
dotnet tool install --global dotnet-ef --version 10.0.0
cd SegundoParcial/ApiInventario
dotnet ef database update
```

Opción B — ejecutando el script `script_db.sql` (idempotente) en SQL Server Management Studio
o con `sqlcmd`:

```bash
sqlcmd -S localhost -U sa -P Tu_Password123 -i script_db.sql
```

### 7.3 Configurar la cadena de conexión

Editar `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "ConexionBD": "Server=localhost;Database=InventarioDB;User Id=sa;Password=Tu_Password123;TrustServerCertificate=True;MultipleActiveResultSets=True;"
  }
}
```

### 7.4 Ejecutar la API

```bash
cd SegundoParcial/ApiInventario
dotnet run
```

- API: <http://localhost:5080>
- Swagger UI: <http://localhost:5080/swagger>

### 7.5 Verificar la instalación de EF Tools

```bash
dotnet ef migrations list
dotnet ef migrations has-pending-model-changes
```

## 8. Pruebas realizadas desde Swagger

Los endpoints se puede probar indistintamente desde **Swagger UI** (`/swagger`) o con `curl`.
La salida completa de las 20 pruebas (petición, respuesta y código HTTP) se encuentra en
[`Evidencias/pruebas-api.md`](Evidencias/pruebas-api.md). Para la entrega, se recomienda agregar
capturas de pantalla de Swagger UI en esa misma carpeta.

Resumen de las pruebas:

| # | Prueba | Resultado |
|---|--------|-----------|
| 1 | Registrar una categoría (`POST /api/categorias`) | **201 Created** |
| 2 | Registrar un producto asociado a una categoría (`POST /api/productos`) | **201 Created** |
| 3 | Consultar todos los productos incluyendo su categoría (`GET /api/productos`) | **200 OK** con `categoria` anidada |
| 4 | Consultar los productos de una categoría (`GET /api/productos/categoria/1`) | **200 OK** |
| 5 | Consultar una categoría con sus productos (`GET /api/categorias/1/productos`) | **200 OK** |
| 6 | Registrar producto duplicado (`POST /api/productos` con el mismo nombre) | **409 Conflict** |
| 7 | Registrar producto duplicado ignorando mayúsculas/minúsculas | **409 Conflict** |
| 8 | Registrar producto con categoría inexistente (`categoriaId: 999`) | **400 Bad Request** |
| 9 | Registrar producto con precio cero | **400 Bad Request** |
| 10 | Registrar producto con stock negativo | **400 Bad Request** |
| 11 | Registrar categoría duplicada | **409 Conflict** |
| 12 | Eliminar categoría con productos asociados (`DELETE /api/categorias/1`) | **409 Conflict** |
| 13 | Modificar un producto (`PUT /api/productos/1`) | **200 OK** |
| 14 | Eliminar un producto (`DELETE /api/productos/2`) | **200 OK** |
| 15 | Eliminar categoría sin productos asociados | **200 OK** |
| 16 | Consultar un producto inexistente | **404 Not Found** |

Ejemplos de respuestas:

```jsonc
// POST /api/productos  ->  201 Created
{
  "id": 1,
  "nombre": "Coca Cola",
  "descripcion": "Gaseosa 500ml",
  "precio": 1500.50,
  "stock": 100,
  "fechaRegistro": "2026-11-15T16:53:23.6843-03:00",
  "categoriaId": 1,
  "categoria": { "id": 1, "nombre": "Bebidas", "descripcion": "Bebidas y licores" }
}

// Producto duplicado  ->  409 Conflict
{ "mensaje": "Ya existe un producto con el nombre 'Coca Cola'." }

// Producto con categoría inexistente  ->  400 Bad Request
{ "mensaje": "La categoría con id 999 no existe." }

// Error de validación  ->  400 Bad Request
{ "mensaje": "Los datos enviados no son validos.", "detalle": "El stock no puede ser negativo" }

// Eliminar categoría con productos asociados  ->  409 Conflict
{ "mensaje": "No se puede eliminar la categoría 'Bebidas' porque tiene productos asociados." }
```

## 9. Manejo de errores

El `ExceptionHandlingMiddleware` se registra antes que los controllers y centraliza el manejo
de excepciones:

| Excepción | Respuesta |
|-----------|-----------|
| `EntidadNoEncontradaException` | 404 Not Found |
| `EntidadDuplicadaException` | 409 Conflict |
| `OperacionNoPermitidaException` | 409 Conflict |
| `ReglaNegocioException` | 400 Bad Request |
| `DbUpdateException` | 409 Conflict |
| Cualquier otra excepción | 500 Internal Server Error (mensaje genérico, el detalle se registra en el log) |

## 10. Comandos útiles

```bash
dotnet build                                           # compilar
dotnet run                                             # ejecutar la API
dotnet ef migrations add NombreDeLaMigracion           # crear una migración
dotnet ef database update                              # aplicar las migraciones
dotnet ef migrations script --idempotent -o script_db.sql   # regenerar el script SQL
```

## 11. Repositorio GitHub

https://github.com/siba-raki/segundo_parcial_vb
