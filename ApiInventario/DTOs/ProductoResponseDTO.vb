Namespace DTOs

    ''' <summary>
    ''' DTO de respuesta para las consultas de productos.
    ''' </summary>
    Public Class ProductoResponseDTO

        Public Property Id As Integer

        Public Property Nombre As String

        Public Property Descripcion As String

        Public Property Precio As Decimal

        Public Property Stock As Integer

        Public Property FechaRegistro As DateTime

        Public Property CategoriaId As Integer

        ''' <summary>
        ''' Datos de la categoría a la que pertenece el producto.
        ''' </summary>
        Public Property Categoria As CategoriaResponseDTO

    End Class

End Namespace
