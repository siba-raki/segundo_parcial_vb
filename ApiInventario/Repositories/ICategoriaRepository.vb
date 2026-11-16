Imports ApiInventario.Models

Namespace Repositories

    ''' <summary>
    ''' Contrato de acceso a datos de las categorías.
    ''' </summary>
    Public Interface ICategoriaRepository

        ''' <summary>
        ''' Obtiene todas las categorías.
        ''' </summary>
        Function ObtenerTodos() As IReadOnlyList(Of Categoria)

        ''' <summary>
        ''' Obtiene una categoría por su identificador.
        ''' </summary>
        Function ObtenerPorId(id As Integer) As Categoria

        ''' <summary>
        ''' Obtiene una categoría junto con sus productos.
        ''' </summary>
        Function ObtenerPorIdConProductos(id As Integer) As Categoria

        ''' <summary>
        ''' Indica si existe una categoría con el nombre indicado, ignorando mayúsculas y minúsculas.
        ''' </summary>
        Function ExistePorNombre(nombre As String, excluirId As Integer) As Boolean

        ''' <summary>
        ''' Indica si la categoría existe.
        ''' </summary>
        Function Existe(id As Integer) As Boolean

        ''' <summary>
        ''' Indica si la categoría tiene productos asociados.
        ''' </summary>
        Function TieneProductos(id As Integer) As Boolean

        Sub Agregar(categoria As Categoria)

        Sub Actualizar(categoria As Categoria)

        Sub Eliminar(categoria As Categoria)

    End Interface

End Namespace
