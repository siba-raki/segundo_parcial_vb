Imports ApiInventario.Models

Namespace Repositories

    ''' <summary>
    ''' Contrato de acceso a datos de los productos.
    ''' </summary>
    Public Interface IProductoRepository

        ''' <summary>
        ''' Obtiene todos los productos incluyendo los datos de su categoría.
        ''' </summary>
        Function ObtenerTodos() As IReadOnlyList(Of Producto)

        ''' <summary>
        ''' Obtiene un producto por su identificador.
        ''' </summary>
        Function ObtenerPorId(id As Integer) As Producto

        ''' <summary>
        ''' Obtiene los productos pertenecientes a una categoría.
        ''' </summary>
        Function ObtenerPorCategoria(categoriaId As Integer) As IReadOnlyList(Of Producto)

        ''' <summary>
        ''' Indica si existe un producto con el nombre indicado, ignorando mayúsculas y minúsculas.
        ''' </summary>
        Function ExistePorNombre(nombre As String, excluirId As Integer) As Boolean

        Sub Agregar(producto As Producto)

        Sub Actualizar(producto As Producto)

        Sub Eliminar(producto As Producto)

    End Interface

End Namespace
