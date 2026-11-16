Namespace DTOs

    ''' <summary>
    ''' DTO de respuesta para la consulta de una categoría con sus productos.
    ''' </summary>
    Public Class CategoriaDetalleDTO

        Public Property Id As Integer

        Public Property Nombre As String

        Public Property Descripcion As String

        ''' <summary>
        ''' Productos asociados a la categoría.
        ''' </summary>
        Public Property Productos As List(Of ProductoResponseDTO)

    End Class

End Namespace
