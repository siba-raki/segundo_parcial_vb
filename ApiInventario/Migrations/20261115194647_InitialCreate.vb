Imports System
Imports ApiInventario.Data
Imports Microsoft.EntityFrameworkCore.Infrastructure
Imports Microsoft.EntityFrameworkCore.Migrations
Imports Microsoft.EntityFrameworkCore.Migrations.Operations
Imports Microsoft.EntityFrameworkCore.Migrations.Operations.Builders

Namespace Migrations

    ''' <summary>
    ''' Migracion inicial que crea las tablas Categorias y Productos,
    ''' sus indices y la relacion uno a muchos entre ellas.
    ''' </summary>
    <DbContext(GetType(AppDbContext))>
    <Migration("20261115194647_InitialCreate")>
    Public Class InitialCreate
        Inherits Migration

        ''' <summary>
        ''' Columnas de la tabla Categorias.
        ''' </summary>
        Private Class ColumnasCategorias

            Public Property Id As OperationBuilder(Of AddColumnOperation)

            Public Property Nombre As OperationBuilder(Of AddColumnOperation)

            Public Property Descripcion As OperationBuilder(Of AddColumnOperation)

        End Class

        ''' <summary>
        ''' Columnas de la tabla Productos.
        ''' </summary>
        Private Class ColumnasProductos

            Public Property Id As OperationBuilder(Of AddColumnOperation)

            Public Property Nombre As OperationBuilder(Of AddColumnOperation)

            Public Property Descripcion As OperationBuilder(Of AddColumnOperation)

            Public Property Precio As OperationBuilder(Of AddColumnOperation)

            Public Property Stock As OperationBuilder(Of AddColumnOperation)

            Public Property FechaRegistro As OperationBuilder(Of AddColumnOperation)

            Public Property CategoriaId As OperationBuilder(Of AddColumnOperation)

        End Class

        Protected Overrides Sub Up(migrationBuilder As MigrationBuilder)

            migrationBuilder.CreateTable(Of ColumnasCategorias)(
                name:="Categorias",
                columns:=Function(table As ColumnsBuilder) New ColumnasCategorias With {
                    .Id = table.Column(Of Integer)(type:="int", name:="Id", nullable:=False).
                        Annotation("SqlServer:Identity", "1, 1"),
                    .Nombre = table.Column(Of String)(type:="nvarchar(50)", maxLength:=50, name:="Nombre", nullable:=False),
                    .Descripcion = table.Column(Of String)(type:="nvarchar(200)", maxLength:=200, name:="Descripcion", nullable:=True)
                },
                constraints:=Sub(table As CreateTableBuilder(Of ColumnasCategorias))
                    table.PrimaryKey("PK_Categorias", Function(x) x.Id)
                End Sub)

            migrationBuilder.CreateTable(Of ColumnasProductos)(
                name:="Productos",
                columns:=Function(table As ColumnsBuilder) New ColumnasProductos With {
                    .Id = table.Column(Of Integer)(type:="int", name:="Id", nullable:=False).
                        Annotation("SqlServer:Identity", "1, 1"),
                    .Nombre = table.Column(Of String)(type:="nvarchar(50)", maxLength:=50, name:="Nombre", nullable:=False),
                    .Descripcion = table.Column(Of String)(type:="nvarchar(200)", maxLength:=200, name:="Descripcion", nullable:=True),
                    .Precio = table.Column(Of Decimal)(type:="decimal(18,2)", precision:=18, scale:=2, name:="Precio", nullable:=False),
                    .Stock = table.Column(Of Integer)(type:="int", name:="Stock", nullable:=False),
                    .FechaRegistro = table.Column(Of DateTime)(type:="datetime2", name:="FechaRegistro", nullable:=False, defaultValueSql:="GETDATE()"),
                    .CategoriaId = table.Column(Of Integer)(type:="int", name:="CategoriaId", nullable:=False)
                },
                constraints:=Sub(table As CreateTableBuilder(Of ColumnasProductos))
                    table.PrimaryKey("PK_Productos", Function(x) x.Id)
                    table.ForeignKey(
                        name:="FK_Productos_Categorias_CategoriaId",
                        column:=Function(x) x.CategoriaId,
                        principalTable:="Categorias",
                        principalColumn:="Id",
                        onDelete:=ReferentialAction.Restrict)
                End Sub)

            migrationBuilder.CreateIndex(
                name:="IX_Categorias_Nombre",
                table:="Categorias",
                column:="Nombre",
                unique:=True)

            migrationBuilder.CreateIndex(
                name:="IX_Productos_CategoriaId",
                table:="Productos",
                column:="CategoriaId")

            migrationBuilder.CreateIndex(
                name:="IX_Productos_Nombre",
                table:="Productos",
                column:="Nombre",
                unique:=True)

        End Sub

        Protected Overrides Sub Down(migrationBuilder As MigrationBuilder)

            migrationBuilder.DropTable(
                name:="Productos")

            migrationBuilder.DropTable(
                name:="Categorias")

        End Sub

    End Class

End Namespace
