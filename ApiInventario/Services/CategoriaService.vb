Imports ApiInventario.DTOs
Imports ApiInventario.Exceptions
Imports ApiInventario.Models
Imports ApiInventario.Repositories

Namespace Services

    ''' <summary>
    ''' Implementa las reglas de negocio de la administración de categorías.
    ''' </summary>
    Public Class CategoriaService
        Implements ICategoriaService

        Private ReadOnly _categoriaRepository As ICategoriaRepository

        Public Sub New(categoriaRepository As ICategoriaRepository)
            _categoriaRepository = categoriaRepository
        End Sub

        Public Function ObtenerTodas() As IReadOnlyList(Of CategoriaResponseDTO) Implements ICategoriaService.ObtenerTodas
            Return _categoriaRepository.ObtenerTodos().
                Select(Function(categoria) Mapear(categoria)).
                ToList()
        End Function

        Public Function ObtenerPorId(id As Integer) As CategoriaResponseDTO Implements ICategoriaService.ObtenerPorId

            Dim categoria = _categoriaRepository.ObtenerPorId(id)

            If categoria Is Nothing Then
                Throw New EntidadNoEncontradaException($"No se encontró la categoría con id {id}.")
            End If

            Return Mapear(categoria)

        End Function

        Public Function ObtenerConProductos(id As Integer) As CategoriaDetalleDTO Implements ICategoriaService.ObtenerConProductos

            Dim categoria = _categoriaRepository.ObtenerPorIdConProductos(id)

            If categoria Is Nothing Then
                Throw New EntidadNoEncontradaException($"No se encontró la categoría con id {id}.")
            End If

            Return New CategoriaDetalleDTO With {
                .Id = categoria.Id,
                .Nombre = categoria.Nombre,
                .Descripcion = categoria.Descripcion,
                .Productos = categoria.Productos.
                    Select(Function(producto) MapearProducto(producto)).
                    ToList()
            }

        End Function

        Public Function Crear(dto As CategoriaCreateDTO) As CategoriaResponseDTO Implements ICategoriaService.Crear

            Dim nombre = If(dto.Nombre, String.Empty).Trim()

            If _categoriaRepository.ExistePorNombre(nombre, 0) Then
                Throw New EntidadDuplicadaException($"Ya existe una categoría con el nombre '{nombre}'.")
            End If

            Dim categoria As New Categoria With {
                .Nombre = nombre,
                .Descripcion = If(dto.Descripcion, String.Empty).Trim()
            }

            _categoriaRepository.Agregar(categoria)

            Return Mapear(categoria)

        End Function

        Public Function Actualizar(id As Integer, dto As CategoriaUpdateDTO) As CategoriaResponseDTO Implements ICategoriaService.Actualizar

            Dim categoria = _categoriaRepository.ObtenerPorId(id)

            If categoria Is Nothing Then
                Throw New EntidadNoEncontradaException($"No se encontró la categoría con id {id}.")
            End If

            Dim nombre = If(dto.Nombre, String.Empty).Trim()

            If _categoriaRepository.ExistePorNombre(nombre, id) Then
                Throw New EntidadDuplicadaException($"Ya existe otra categoría con el nombre '{nombre}'.")
            End If

            categoria.Nombre = nombre
            categoria.Descripcion = If(dto.Descripcion, String.Empty).Trim()

            _categoriaRepository.Actualizar(categoria)

            Return Mapear(categoria)

        End Function

        Public Sub Eliminar(id As Integer) Implements ICategoriaService.Eliminar

            Dim categoria = _categoriaRepository.ObtenerPorId(id)

            If categoria Is Nothing Then
                Throw New EntidadNoEncontradaException($"No se encontró la categoría con id {id}.")
            End If

            If _categoriaRepository.TieneProductos(id) Then
                Throw New OperacionNoPermitidaException(
                    $"No se puede eliminar la categoría '{categoria.Nombre}' porque tiene productos asociados.")
            End If

            _categoriaRepository.Eliminar(categoria)

        End Sub

        Private Shared Function Mapear(categoria As Categoria) As CategoriaResponseDTO
            Return New CategoriaResponseDTO With {
                .Id = categoria.Id,
                .Nombre = categoria.Nombre,
                .Descripcion = categoria.Descripcion
            }
        End Function

        Private Shared Function MapearProducto(producto As Producto) As ProductoResponseDTO
            Return New ProductoResponseDTO With {
                .Id = producto.Id,
                .Nombre = producto.Nombre,
                .Descripcion = producto.Descripcion,
                .Precio = producto.Precio,
                .Stock = producto.Stock,
                .FechaRegistro = producto.FechaRegistro,
                .CategoriaId = producto.CategoriaId
            }
        End Function

    End Class

End Namespace
