Imports System
Imports ApiInventario.Data
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Infrastructure
Imports Microsoft.EntityFrameworkCore.Metadata
Imports Microsoft.EntityFrameworkCore.Metadata.Builders
Imports Microsoft.EntityFrameworkCore.Migrations

Namespace Migrations

    ''' <summary>
    ''' Instantanea del modelo utilizada por Entity Framework Core
    ''' para detectar los cambios pendientes en el esquema.
    ''' </summary>
    <DbContext(GetType(AppDbContext))>
    Public Class AppDbContextModelSnapshot
        Inherits ModelSnapshot

        Protected Overrides Sub BuildModel(modelBuilder As ModelBuilder)

            modelBuilder.HasAnnotation("ProductVersion", "10.0.0")
            modelBuilder.HasAnnotation("Relational:MaxIdentifierLength", 128)

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder)

            modelBuilder.Entity("ApiInventario.Models.Categoria",
                Sub(b As EntityTypeBuilder)

                    b.Property(Of Integer)("Id").
                        ValueGeneratedOnAdd().
                        HasColumnType("int")

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property(Of Integer)("Id"))

                    b.Property(Of String)("Descripcion").
                        HasMaxLength(200).
                        HasColumnType("nvarchar(200)")

                    b.Property(Of String)("Nombre").
                        IsRequired().
                        HasMaxLength(50).
                        HasColumnType("nvarchar(50)")

                    b.HasKey("Id")

                    b.HasIndex("Nombre").
                        IsUnique()

                    b.ToTable("Categorias")

                End Sub)

            modelBuilder.Entity("ApiInventario.Models.Producto",
                Sub(b As EntityTypeBuilder)

                    b.Property(Of Integer)("Id").
                        ValueGeneratedOnAdd().
                        HasColumnType("int")

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property(Of Integer)("Id"))

                    b.Property(Of Integer)("CategoriaId").
                        HasColumnType("int")

                    b.Property(Of String)("Descripcion").
                        HasMaxLength(200).
                        HasColumnType("nvarchar(200)")

                    b.Property(Of DateTime)("FechaRegistro").
                        ValueGeneratedOnAdd().
                        HasColumnType("datetime2").
                        HasDefaultValueSql("GETDATE()")

                    b.Property(Of String)("Nombre").
                        IsRequired().
                        HasMaxLength(50).
                        HasColumnType("nvarchar(50)")

                    b.Property(Of Decimal)("Precio").
                        HasPrecision(18, 2).
                        HasColumnType("decimal(18,2)")

                    b.Property(Of Integer)("Stock").
                        HasColumnType("int")

                    b.HasKey("Id")

                    b.HasIndex("CategoriaId")

                    b.HasIndex("Nombre").
                        IsUnique()

                    b.ToTable("Productos")

                End Sub)

            modelBuilder.Entity("ApiInventario.Models.Producto",
                Sub(b As EntityTypeBuilder)

                    b.HasOne("ApiInventario.Models.Categoria", "Categoria").
                        WithMany("Productos").
                        HasForeignKey("CategoriaId").
                        OnDelete(DeleteBehavior.Restrict).
                        IsRequired()

                    b.Navigation("Categoria")

                End Sub)

            modelBuilder.Entity("ApiInventario.Models.Categoria",
                Sub(b As EntityTypeBuilder)

                    b.Navigation("Productos")

                End Sub)

        End Sub

    End Class

End Namespace
