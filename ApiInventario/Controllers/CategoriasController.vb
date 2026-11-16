Imports ApiInventario.DTOs
Imports ApiInventario.Services
Imports Microsoft.AspNetCore.Http
Imports Microsoft.AspNetCore.Mvc

Namespace Controllers

    ''' <summary>Endpoints para la administracion de categorias.</summary>
    <ApiController>
    <Route("api/[controller]")>
    <Produces("application/json")>
    Public Class CategoriasController
        Inherits ControllerBase

        Private ReadOnly _categoriaService As ICategoriaService

        Public Sub New(categoriaService As ICategoriaService)
            _categoriaService = categoriaService
        End Sub

        ''' <summary>Consulta todas las categorias.</summary>
        <HttpGet>
        <ProducesResponseType(StatusCodes.Status200OK)>
        Public Function ObtenerTodas() As ActionResult(Of IReadOnlyList(Of CategoriaResponseDTO))
            Return Ok(_categoriaService.ObtenerTodas())
        End Function

        ''' <summary>Consulta una categoria por su identificador.</summary>
        <HttpGet("{id:int}")>
        <ProducesResponseType(StatusCodes.Status200OK)>
        <ProducesResponseType(GetType(ErrorResponseDTO), StatusCodes.Status404NotFound)>
        Public Function ObtenerPorId(id As Integer) As ActionResult(Of CategoriaResponseDTO)
            Return Ok(_categoriaService.ObtenerPorId(id))
        End Function

        ''' <summary>Consulta una categoria junto con los productos que contiene.</summary>
        <HttpGet("{id:int}/productos")>
        <ProducesResponseType(StatusCodes.Status200OK)>
        <ProducesResponseType(GetType(ErrorResponseDTO), StatusCodes.Status404NotFound)>
        Public Function ObtenerConProductos(id As Integer) As ActionResult(Of CategoriaDetalleDTO)
            Return Ok(_categoriaService.ObtenerConProductos(id))
        End Function

        ''' <summary>Registra una categoria.</summary>
        <HttpPost>
        <ProducesResponseType(StatusCodes.Status201Created)>
        <ProducesResponseType(GetType(ErrorResponseDTO), StatusCodes.Status400BadRequest)>
        <ProducesResponseType(GetType(ErrorResponseDTO), StatusCodes.Status409Conflict)>
        Public Function Crear(<FromBody> dto As CategoriaCreateDTO) As ActionResult(Of CategoriaResponseDTO)

            Dim categoria = _categoriaService.Crear(dto)

            Return CreatedAtAction(NameOf(ObtenerPorId),
                                   New With {.id = categoria.Id},
                                   categoria)

        End Function

        ''' <summary>Modifica una categoria.</summary>
        <HttpPut("{id:int}")>
        <ProducesResponseType(StatusCodes.Status200OK)>
        <ProducesResponseType(GetType(ErrorResponseDTO), StatusCodes.Status400BadRequest)>
        <ProducesResponseType(GetType(ErrorResponseDTO), StatusCodes.Status404NotFound)>
        <ProducesResponseType(GetType(ErrorResponseDTO), StatusCodes.Status409Conflict)>
        Public Function Actualizar(id As Integer, <FromBody> dto As CategoriaUpdateDTO) As ActionResult(Of CategoriaResponseDTO)
            Return Ok(_categoriaService.Actualizar(id, dto))
        End Function

        ''' <summary>Elimina una categoria, siempre que no tenga productos asociados.</summary>
        <HttpDelete("{id:int}")>
        <ProducesResponseType(StatusCodes.Status200OK)>
        <ProducesResponseType(GetType(ErrorResponseDTO), StatusCodes.Status404NotFound)>
        <ProducesResponseType(GetType(ErrorResponseDTO), StatusCodes.Status409Conflict)>
        Public Function Eliminar(id As Integer) As IActionResult

            _categoriaService.Eliminar(id)

            Return Ok(New With {.mensaje = "Categoria eliminada exitosamente."})

        End Function

    End Class

End Namespace
