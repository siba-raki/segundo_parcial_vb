Namespace DTOs

    ''' <summary>
    ''' DTO utilizado para responder los errores de la API.
    ''' </summary>
    Public Class ErrorResponseDTO

        ''' <summary>
        ''' Mensaje legible del error.
        ''' </summary>
        Public Property Mensaje As String

        ''' <summary>
        ''' Detalle de las validaciones fallidas, cuando corresponda.
        ''' </summary>
        Public Property Detalle As String

    End Class

End Namespace
