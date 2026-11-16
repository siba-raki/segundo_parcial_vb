Imports System.IO
Imports System.Reflection
Imports System.Text.Json.Serialization
Imports ApiInventario.Data
Imports ApiInventario.DTOs
Imports ApiInventario.Middleware
Imports ApiInventario.Repositories
Imports ApiInventario.Services
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.OpenApi

Module Program

    Sub Main(args As String())

        Dim builder = WebApplication.CreateBuilder(args)

        builder.Services.AddDbContext(Of AppDbContext)(Sub(options As DbContextOptionsBuilder)
            options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionBD"))
        End Sub)

        builder.Services.AddScoped(Of ICategoriaRepository, CategoriaRepository)()
        builder.Services.AddScoped(Of IProductoRepository, ProductoRepository)()
        builder.Services.AddScoped(Of ICategoriaService, CategoriaService)()
        builder.Services.AddScoped(Of IProductoService, ProductoService)()

        builder.Services.AddControllers().
            AddJsonOptions(Sub(options)
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            End Sub)
        builder.Services.ConfigureHttpJsonOptions(Sub(options)
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        End Sub)
        builder.Services.AddEndpointsApiExplorer()

        builder.Services.Configure(Of ApiBehaviorOptions)(Sub(options)
            options.InvalidModelStateResponseFactory =
                Function(context As ActionContext) As IActionResult

                    Dim mensajes = context.ModelState.
                        Where(Function(entrada) entrada.Value.Errors.Count > 0).
                        SelectMany(Function(entrada) entrada.Value.Errors.
                            Select(Function(err) err.ErrorMessage)).
                        Where(Function(mensaje) Not String.IsNullOrWhiteSpace(mensaje)).
                        ToList()

                    Dim respuesta As New ErrorResponseDTO With {
                        .Mensaje = "Los datos enviados no son validos.",
                        .Detalle = String.Join(" | ", mensajes)
                    }

                    Return New BadRequestObjectResult(respuesta)

                End Function
        End Sub)

        builder.Services.AddSwaggerGen(Sub(options)

            options.SwaggerDoc("v1", New OpenApiInfo With {
                .Title = "API de Inventario",
                .Version = "v1",
                .Description = "API REST para administrar productos y categorias de un sistema de inventario."
            })

            Dim nombreAssembly = Assembly.GetExecutingAssembly().GetName().Name
            Dim rutaDocumentacion = Path.Combine(AppContext.BaseDirectory, $"{nombreAssembly}.xml")

            If File.Exists(rutaDocumentacion) Then
                options.IncludeXmlComments(rutaDocumentacion)
            End If

        End Sub)

        Dim app = builder.Build()

        app.UseMiddleware(Of ExceptionHandlingMiddleware)()

        app.UseSwagger()
        app.UseSwaggerUI(Sub(options)
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Inventario v1")
        End Sub)

        app.MapControllers()

        app.Run()

    End Sub

End Module
