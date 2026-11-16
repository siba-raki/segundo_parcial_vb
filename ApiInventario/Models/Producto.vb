Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Models

    ''' <summary>
    ''' Entidad que representa un producto del inventario.
    ''' </summary>
    <Table("Productos")>
    Public Class Producto

        <Key>
        Public Property Id As Integer

        <Required>
        <StringLength(50)>
        Public Property Nombre As String

        <StringLength(200)>
        Public Property Descripcion As String

        Public Property Precio As Decimal

        Public Property Stock As Integer

        Public Property FechaRegistro As DateTime

        ''' <summary>
        ''' Identificador de la categoría a la que pertenece el producto.
        ''' </summary>
        Public Property CategoriaId As Integer

        ''' <summary>
        ''' Categoría a la que pertenece el producto.
        ''' </summary>
        <ForeignKey("CategoriaId")>
        Public Property Categoria As Categoria

    End Class

End Namespace
