Imports ApiInventario.Models
Imports Microsoft.EntityFrameworkCore

Namespace Data

    ''' <summary>
    ''' Contexto de datos de la API de inventario.
    ''' </summary>
    Public Class AppDbContext
        Inherits DbContext

        Public Sub New(options As DbContextOptions(Of AppDbContext))
            MyBase.New(options)
        End Sub

        Public Property Categorias As DbSet(Of Categoria)

        Public Property Productos As DbSet(Of Producto)

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)

            modelBuilder.Entity(Of Categoria)().
                HasIndex(Function(categoria) categoria.Nombre).
                IsUnique()

            modelBuilder.Entity(Of Producto)().
                HasIndex(Function(producto) producto.Nombre).
                IsUnique()

            modelBuilder.Entity(Of Producto)().
                HasOne(Function(producto) producto.Categoria).
                WithMany(Function(categoria) categoria.Productos).
                HasForeignKey(Function(producto) producto.CategoriaId).
                OnDelete(DeleteBehavior.Restrict)

            modelBuilder.Entity(Of Producto)().
                Property(Function(producto) producto.Precio).
                HasPrecision(18, 2)

            modelBuilder.Entity(Of Producto)().
                Property(Function(producto) producto.FechaRegistro).
                HasDefaultValueSql("GETDATE()")

        End Sub

    End Class

End Namespace
