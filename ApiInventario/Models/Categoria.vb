Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Models

    ''' <summary>
    ''' Entidad que representa una categoría del inventario.
    ''' </summary>
    <Table("Categorias")>
    Public Class Categoria

        <Key>
        Public Property Id As Integer

        <Required>
        <StringLength(50)>
        Public Property Nombre As String

        <StringLength(200)>
        Public Property Descripcion As String

        ''' <summary>
        ''' Productos asociados a la categoría.
        ''' </summary>
        Public Property Productos As ICollection(Of Producto)

    End Class

End Namespace
