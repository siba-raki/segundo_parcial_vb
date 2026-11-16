Imports System.ComponentModel.DataAnnotations

Namespace DTOs

    ''' <summary>
    ''' DTO de entrada para registrar un producto.
    ''' </summary>
    Public Class ProductoCreateDTO

        <Required(ErrorMessage:="El nombre del producto es obligatorio")>
        <StringLength(50, MinimumLength:=1, ErrorMessage:="El nombre del producto no puede superar los 50 caracteres")>
        Public Property Nombre As String

        <StringLength(200, ErrorMessage:="La descripción del producto no puede superar los 200 caracteres")>
        Public Property Descripcion As String

        <Range(0.01, 1000000000, ErrorMessage:="El precio debe ser mayor que cero")>
        Public Property Precio As Decimal

        <Range(0, 1000000000, ErrorMessage:="El stock no puede ser negativo")>
        Public Property Stock As Integer

        <Range(1, 2147483647, ErrorMessage:="La categoría es obligatoria")>
        Public Property CategoriaId As Integer

    End Class

End Namespace
