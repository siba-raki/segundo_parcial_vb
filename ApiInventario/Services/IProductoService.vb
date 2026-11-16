Imports ApiInventario.DTOs

Namespace Services

    ''' <summary>
    ''' Contrato de la capa de servicios para la administración de productos.
    ''' </summary>
    Public Interface IProductoService

        Function ObtenerTodos() As IReadOnlyList(Of ProductoResponseDTO)

        Function ObtenerPorId(id As Integer) As ProductoResponseDTO

        Function ObtenerPorCategoria(categoriaId As Integer) As IReadOnlyList(Of ProductoResponseDTO)

        Function Crear(dto As ProductoCreateDTO) As ProductoResponseDTO

        Function Actualizar(id As Integer, dto As ProductoUpdateDTO) As ProductoResponseDTO

        Sub Eliminar(id As Integer)

    End Interface

End Namespace
