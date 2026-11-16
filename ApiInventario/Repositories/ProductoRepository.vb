Imports ApiInventario.Data
Imports ApiInventario.Models
Imports Microsoft.EntityFrameworkCore

Namespace Repositories

    ''' <summary>
    ''' Implementación del acceso a datos de los productos.
    ''' </summary>
    Public Class ProductoRepository
        Implements IProductoRepository

        Private ReadOnly _contexto As AppDbContext

        Public Sub New(contexto As AppDbContext)
            _contexto = contexto
        End Sub

        Public Function ObtenerTodos() As IReadOnlyList(Of Producto) Implements IProductoRepository.ObtenerTodos
            Return _contexto.Productos.
                Include(Function(producto) producto.Categoria).
                OrderBy(Function(producto) producto.Nombre).
                ToList()
        End Function

        Public Function ObtenerPorId(id As Integer) As Producto Implements IProductoRepository.ObtenerPorId
            Return _contexto.Productos.
                Include(Function(producto) producto.Categoria).
                FirstOrDefault(Function(producto) producto.Id = id)
        End Function

        Public Function ObtenerPorCategoria(categoriaId As Integer) As IReadOnlyList(Of Producto) Implements IProductoRepository.ObtenerPorCategoria
            Return _contexto.Productos.
                Include(Function(producto) producto.Categoria).
                Where(Function(producto) producto.CategoriaId = categoriaId).
                OrderBy(Function(producto) producto.Nombre).
                ToList()
        End Function

        Public Function ExistePorNombre(nombre As String, excluirId As Integer) As Boolean Implements IProductoRepository.ExistePorNombre

            Dim nombreNormalizado = If(nombre, String.Empty).Trim().ToLowerInvariant()

            Return _contexto.Productos.
                Any(Function(producto) producto.Id <> excluirId AndAlso producto.Nombre.ToLower() = nombreNormalizado)

        End Function

        Public Sub Agregar(producto As Producto) Implements IProductoRepository.Agregar
            _contexto.Productos.Add(producto)
            _contexto.SaveChanges()
        End Sub

        Public Sub Actualizar(producto As Producto) Implements IProductoRepository.Actualizar
            _contexto.Entry(producto).State = EntityState.Modified
            _contexto.SaveChanges()
        End Sub

        Public Sub Eliminar(producto As Producto) Implements IProductoRepository.Eliminar
            _contexto.Productos.Remove(producto)
            _contexto.SaveChanges()
        End Sub

    End Class

End Namespace
