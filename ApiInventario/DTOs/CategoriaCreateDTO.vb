Imports System.ComponentModel.DataAnnotations

Namespace DTOs

    ''' <summary>
    ''' DTO de entrada para registrar una categoría.
    ''' </summary>
    Public Class CategoriaCreateDTO

        <Required(ErrorMessage:="El nombre de la categoría es obligatorio")>
        <StringLength(50, MinimumLength:=1, ErrorMessage:="El nombre de la categoría no puede superar los 50 caracteres")>
        Public Property Nombre As String

        <StringLength(200, ErrorMessage:="La descripción de la categoría no puede superar los 200 caracteres")>
        Public Property Descripcion As String

    End Class

End Namespace
