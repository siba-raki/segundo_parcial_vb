Imports ApiInventario.DTOs
Imports ApiInventario.Exceptions
Imports ApiInventario.Models
Imports ApiInventario.Repositories
Imports Microsoft.AspNetCore.Http

Namespace Services

    ''' <summary>
    ''' Implementa las reglas de negocio de la administración de productos.
    ''' </summary>
    Public Class ProductoService
        Implements IProductoService

        Private ReadOnly _productoRepository As IProductoRepository

        Private ReadOnly _categoriaRepository As ICategoriaRepository

        Public Sub New(productoRepository As IProductoRepository,
                       categoriaRepository As ICategoriaRepository)

            _productoRepository = productoRepository
            _categoriaRepository = categoriaRepository

        End Sub

        Public Function ObtenerTodos() As IReadOnlyList(Of ProductoResponseDTO) Implements IProductoService.ObtenerTodos
            Return _productoRepository.ObtenerTodos().
                Select(Function(producto) Mapear(producto)).
                ToList()
        End Function

        Public Function ObtenerPorId(id As Integer) As ProductoResponseDTO Implements IProductoService.ObtenerPorId

            Dim producto = _productoRepository.ObtenerPorId(id)

            If producto Is Nothing Then
                Throw New EntidadNoEncontradaException($"No se encontró el producto con id {id}.")
            End If

            Return Mapear(producto)

        End Function

        Public Function ObtenerPorCategoria(categoriaId As Integer) As IReadOnlyList(Of ProductoResponseDTO) Implements IProductoService.ObtenerPorCategoria

            If Not _categoriaRepository.Existe(categoriaId) Then
                Throw New EntidadNoEncontradaException($"No se encontró la categoría con id {categoriaId}.")
            End If

            Return _productoRepository.ObtenerPorCategoria(categoriaId).
                Select(Function(producto) Mapear(producto)).
                ToList()

        End Function

        Public Function Crear(dto As ProductoCreateDTO) As ProductoResponseDTO Implements IProductoService.Crear

            Dim nombre = If(dto.Nombre, String.Empty).Trim()

            If dto.Precio <= 0 Then
                Throw New ReglaNegocioException(
                    "El precio del producto debe ser mayor que cero.",
                    StatusCodes.Status400BadRequest)
            End If

            If dto.Stock < 0 Then
                Throw New ReglaNegocioException(
                    "El stock del producto no puede ser negativo.",
                    StatusCodes.Status400BadRequest)
            End If

            Dim categoria = _categoriaRepository.ObtenerPorId(dto.CategoriaId)

            If categoria Is Nothing Then
                Throw New ReglaNegocioException(
                    $"La categoría con id {dto.CategoriaId} no existe.",
                    StatusCodes.Status400BadRequest)
            End If

            If _productoRepository.ExistePorNombre(nombre, 0) Then
                Throw New EntidadDuplicadaException($"Ya existe un producto con el nombre '{nombre}'.")
            End If

            Dim producto As New Producto With {
                .Nombre = nombre,
                .Descripcion = If(dto.Descripcion, String.Empty).Trim(),
                .Precio = dto.Precio,
                .Stock = dto.Stock,
                .FechaRegistro = DateTime.Now,
                .CategoriaId = dto.CategoriaId,
                .Categoria = categoria
            }

            _productoRepository.Agregar(producto)

            Return Mapear(producto)

        End Function

        Public Function Actualizar(id As Integer, dto As ProductoUpdateDTO) As ProductoResponseDTO Implements IProductoService.Actualizar

            Dim producto = _productoRepository.ObtenerPorId(id)

            If producto Is Nothing Then
                Throw New EntidadNoEncontradaException($"No se encontró el producto con id {id}.")
            End If

            Dim nombre = If(dto.Nombre, String.Empty).Trim()

            If dto.Precio <= 0 Then
                Throw New ReglaNegocioException(
                    "El precio del producto debe ser mayor que cero.",
                    StatusCodes.Status400BadRequest)
            End If

            If dto.Stock < 0 Then
                Throw New ReglaNegocioException(
                    "El stock del producto no puede ser negativo.",
                    StatusCodes.Status400BadRequest)
            End If

            If _productoRepository.ExistePorNombre(nombre, id) Then
                Throw New EntidadDuplicadaException($"Ya existe otro producto con el nombre '{nombre}'.")
            End If

            Dim categoria = _categoriaRepository.ObtenerPorId(dto.CategoriaId)

            If categoria Is Nothing Then
                Throw New ReglaNegocioException(
                    $"La categoría con id {dto.CategoriaId} no existe.",
                    StatusCodes.Status400BadRequest)
            End If

            producto.Nombre = nombre
            producto.Descripcion = If(dto.Descripcion, String.Empty).Trim()
            producto.Precio = dto.Precio
            producto.Stock = dto.Stock
            producto.CategoriaId = dto.CategoriaId
            producto.Categoria = categoria

            _productoRepository.Actualizar(producto)

            Return Mapear(producto)

        End Function

        Public Sub Eliminar(id As Integer) Implements IProductoService.Eliminar

            Dim producto = _productoRepository.ObtenerPorId(id)

            If producto Is Nothing Then
                Throw New EntidadNoEncontradaException($"No se encontró el producto con id {id}.")
            End If

            _productoRepository.Eliminar(producto)

        End Sub

        Private Shared Function Mapear(producto As Producto) As ProductoResponseDTO

            Dim respuesta As New ProductoResponseDTO With {
                .Id = producto.Id,
                .Nombre = producto.Nombre,
                .Descripcion = producto.Descripcion,
                .Precio = producto.Precio,
                .Stock = producto.Stock,
                .FechaRegistro = producto.FechaRegistro,
                .CategoriaId = producto.CategoriaId
            }

            If producto.Categoria IsNot Nothing Then
                respuesta.Categoria = New CategoriaResponseDTO With {
                    .Id = producto.Categoria.Id,
                    .Nombre = producto.Categoria.Nombre,
                    .Descripcion = producto.Categoria.Descripcion
                }
            End If

            Return respuesta

        End Function

    End Class

End Namespace
