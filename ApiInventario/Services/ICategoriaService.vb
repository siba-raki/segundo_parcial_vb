Imports ApiInventario.DTOs

Namespace Services

    ''' <summary>
    ''' Contrato de la capa de servicios para la administración de categorías.
    ''' </summary>
    Public Interface ICategoriaService

        Function ObtenerTodas() As IReadOnlyList(Of CategoriaResponseDTO)

        Function ObtenerPorId(id As Integer) As CategoriaResponseDTO

        Function ObtenerConProductos(id As Integer) As CategoriaDetalleDTO

        Function Crear(dto As CategoriaCreateDTO) As CategoriaResponseDTO

        Function Actualizar(id As Integer, dto As CategoriaUpdateDTO) As CategoriaResponseDTO

        Sub Eliminar(id As Integer)

    End Interface

End Namespace
