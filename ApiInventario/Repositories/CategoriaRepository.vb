Imports ApiInventario.Data
Imports ApiInventario.Models
Imports Microsoft.EntityFrameworkCore

Namespace Repositories

    ''' <summary>
    ''' Implementación del acceso a datos de las categorías.
    ''' </summary>
    Public Class CategoriaRepository
        Implements ICategoriaRepository

        Private ReadOnly _contexto As AppDbContext

        Public Sub New(contexto As AppDbContext)
            _contexto = contexto
        End Sub

        Public Function ObtenerTodos() As IReadOnlyList(Of Categoria) Implements ICategoriaRepository.ObtenerTodos
            Return _contexto.Categorias.
                OrderBy(Function(categoria) categoria.Nombre).
                ToList()
        End Function

        Public Function ObtenerPorId(id As Integer) As Categoria Implements ICategoriaRepository.ObtenerPorId
            Return _contexto.Categorias.Find(id)
        End Function

        Public Function ObtenerPorIdConProductos(id As Integer) As Categoria Implements ICategoriaRepository.ObtenerPorIdConProductos
            Return _contexto.Categorias.
                Include(Function(categoria) categoria.Productos).
                FirstOrDefault(Function(categoria) categoria.Id = id)
        End Function

        Public Function ExistePorNombre(nombre As String, excluirId As Integer) As Boolean Implements ICategoriaRepository.ExistePorNombre

            Dim nombreNormalizado = If(nombre, String.Empty).Trim().ToLowerInvariant()

            Return _contexto.Categorias.
                Any(Function(categoria) categoria.Id <> excluirId AndAlso categoria.Nombre.ToLower() = nombreNormalizado)

        End Function

        Public Function Existe(id As Integer) As Boolean Implements ICategoriaRepository.Existe
            Return _contexto.Categorias.Any(Function(categoria) categoria.Id = id)
        End Function

        Public Function TieneProductos(id As Integer) As Boolean Implements ICategoriaRepository.TieneProductos
            Return _contexto.Productos.Any(Function(producto) producto.CategoriaId = id)
        End Function

        Public Sub Agregar(categoria As Categoria) Implements ICategoriaRepository.Agregar
            _contexto.Categorias.Add(categoria)
            _contexto.SaveChanges()
        End Sub

        Public Sub Actualizar(categoria As Categoria) Implements ICategoriaRepository.Actualizar
            _contexto.Entry(categoria).State = EntityState.Modified
            _contexto.SaveChanges()
        End Sub

        Public Sub Eliminar(categoria As Categoria) Implements ICategoriaRepository.Eliminar
            _contexto.Categorias.Remove(categoria)
            _contexto.SaveChanges()
        End Sub

    End Class

End Namespace
