# Evidencias de pruebas - ApiInventario

Ejecucion completa contra SQL Server real (base `InventarioDB` recien creada con las migraciones).
Cada bloque muestra la peticion HTTP y la respuesta obtenida con su codigo de estado.

```text
### 1) Registrar una categoria
$ curl -X POST /api/categorias -H "Content-Type: application/json" -d "{"nombre":"Bebidas","descripcion":"Bebidas y licores"}"
{"id":1,"nombre":"Bebidas","descripcion":"Bebidas y licores"}
HTTP 201

### 2) Registrar una segunda categoria
$ curl -X POST /api/categorias -H "Content-Type: application/json" -d "{"nombre":"Almacen","descripcion":"Productos de almacen"}"
{"id":2,"nombre":"Almacen","descripcion":"Productos de almacen"}
HTTP 201

### 3) Registrar un producto asociado a una categoria
$ curl -X POST /api/productos -H "Content-Type: application/json" -d "{"nombre":"Coca Cola","descripcion":"Gaseosa 500ml","precio":1500.50,"stock":100,"categoriaId":1}"
{"id":1,"nombre":"Coca Cola","descripcion":"Gaseosa 500ml","precio":1500.50,"stock":100,"fechaRegistro":"2026-11-15T16:55:33.972049-03:00","categoriaId":1,"categoria":{"id":1,"nombre":"Bebidas","descripcion":"Bebidas y licores"}}
HTTP 201

### 4) Registrar otro producto
$ curl -X POST /api/productos -H "Content-Type: application/json" -d "{"nombre":"Pepsi","descripcion":"Gaseosa cola 500ml","precio":1400,"stock":80,"categoriaId":1}"
{"id":2,"nombre":"Pepsi","descripcion":"Gaseosa cola 500ml","precio":1400,"stock":80,"fechaRegistro":"2026-11-15T16:55:34.023379-03:00","categoriaId":1,"categoria":{"id":1,"nombre":"Bebidas","descripcion":"Bebidas y licores"}}
HTTP 201

### 5) Consultar todos los productos incluyendo su categoria
$ curl -X GET /api/productos
[{"id":1,"nombre":"Coca Cola","descripcion":"Gaseosa 500ml","precio":1500.50,"stock":100,"fechaRegistro":"2026-11-15T16:55:33.972049","categoriaId":1,"categoria":{"id":1,"nombre":"Bebidas","descripcion":"Bebidas y licores"}},{"id":2,"nombre":"Pepsi","descripcion":"Gaseosa cola 500ml","precio":1400.00,"stock":80,"fechaRegistro":"2026-11-15T16:55:34.023379","categoriaId":1,"categoria":{"id":1,"nombre":"Bebidas","descripcion":"Bebidas y licores"}}]
HTTP 200

### 6) Consultar los productos de una categoria
$ curl -X GET /api/productos/categoria/1
[{"id":1,"nombre":"Coca Cola","descripcion":"Gaseosa 500ml","precio":1500.50,"stock":100,"fechaRegistro":"2026-11-15T16:55:33.972049","categoriaId":1,"categoria":{"id":1,"nombre":"Bebidas","descripcion":"Bebidas y licores"}},{"id":2,"nombre":"Pepsi","descripcion":"Gaseosa cola 500ml","precio":1400.00,"stock":80,"fechaRegistro":"2026-11-15T16:55:34.023379","categoriaId":1,"categoria":{"id":1,"nombre":"Bebidas","descripcion":"Bebidas y licores"}}]
HTTP 200

### 7) Consultar una categoria con sus productos
$ curl -X GET /api/categorias/1/productos
{"id":1,"nombre":"Bebidas","descripcion":"Bebidas y licores","productos":[{"id":1,"nombre":"Coca Cola","descripcion":"Gaseosa 500ml","precio":1500.50,"stock":100,"fechaRegistro":"2026-11-15T16:55:33.972049","categoriaId":1},{"id":2,"nombre":"Pepsi","descripcion":"Gaseosa cola 500ml","precio":1400.00,"stock":80,"fechaRegistro":"2026-11-15T16:55:34.023379","categoriaId":1}]}
HTTP 200

### 8) Intentar registrar un producto duplicado (nombre identico)
$ curl -X POST /api/productos -H "Content-Type: application/json" -d "{"nombre":"Coca Cola","descripcion":"Repetido","precio":100,"stock":10,"categoriaId":1}"
{"mensaje":"Ya existe un producto con el nombre 'Coca Cola'."}
HTTP 409

### 9) Intentar registrar un producto duplicado (distinta caja)
$ curl -X POST /api/productos -H "Content-Type: application/json" -d "{"nombre":"coca cola","descripcion":"Repetido","precio":100,"stock":10,"categoriaId":1}"
{"mensaje":"Ya existe un producto con el nombre 'coca cola'."}
HTTP 409

### 10) Intentar registrar un producto con una categoria inexistente
$ curl -X POST /api/productos -H "Content-Type: application/json" -d "{"nombre":"Producto Sin Categoria","precio":100,"stock":10,"categoriaId":999}"
{"mensaje":"La categoría con id 999 no existe."}
HTTP 400

### 11) Intentar registrar un producto con precio cero
$ curl -X POST /api/productos -H "Content-Type: application/json" -d "{"nombre":"Producto Precio Cero","precio":0,"stock":10,"categoriaId":1}"
{"mensaje":"Los datos enviados no son validos.","detalle":"El precio debe ser mayor que cero"}
HTTP 400

### 12) Intentar registrar un producto con stock negativo
$ curl -X POST /api/productos -H "Content-Type: application/json" -d "{"nombre":"Producto Stock Negativo","precio":100,"stock":-5,"categoriaId":1}"
{"mensaje":"Los datos enviados no son validos.","detalle":"El stock no puede ser negativo"}
HTTP 400

### 13) Intentar registrar una categoria duplicada
$ curl -X POST /api/categorias -H "Content-Type: application/json" -d "{"nombre":"bebidas","descripcion":"Duplicada"}"
{"mensaje":"Ya existe una categoría con el nombre 'bebidas'."}
HTTP 409

### 14) Intentar eliminar una categoria con productos asociados
$ curl -X DELETE /api/categorias/1
{"mensaje":"No se puede eliminar la categoría 'Bebidas' porque tiene productos asociados."}
HTTP 409

### 15) Modificar un producto
$ curl -X PUT /api/productos/1 -H "Content-Type: application/json" -d "{"nombre":"Coca Cola Zero","descripcion":"Gaseosa sin azucar 500ml","precio":1800,"stock":75,"categoriaId":2}"
{"id":1,"nombre":"Coca Cola Zero","descripcion":"Gaseosa sin azucar 500ml","precio":1800,"stock":75,"fechaRegistro":"2026-11-15T16:55:33.972049","categoriaId":2,"categoria":{"id":2,"nombre":"Almacen","descripcion":"Productos de almacen"}}
HTTP 200

### 16) Consultar el producto modificado
$ curl -X GET /api/productos/1
{"id":1,"nombre":"Coca Cola Zero","descripcion":"Gaseosa sin azucar 500ml","precio":1800.00,"stock":75,"fechaRegistro":"2026-11-15T16:55:33.972049","categoriaId":2,"categoria":{"id":2,"nombre":"Almacen","descripcion":"Productos de almacen"}}
HTTP 200

### 17) Eliminar un producto
$ curl -X DELETE /api/productos/2
{"mensaje":"Producto eliminado exitosamente."}
HTTP 200

### 18) Eliminar la categoria que ya no tiene productos
$ curl -X DELETE /api/categorias/1
{"mensaje":"Categoria eliminada exitosamente."}
HTTP 200

### 19) Consultar un producto inexistente
$ curl -X GET /api/productos/999
{"mensaje":"No se encontró el producto con id 999."}
HTTP 404

### 20) Consultar todas las categorias
$ curl -X GET /api/categorias
[{"id":2,"nombre":"Almacen","descripcion":"Productos de almacen"}]
HTTP 200

```