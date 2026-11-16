Imports ApiInventario.DTOs
Imports ApiInventario.Services
Imports Microsoft.AspNetCore.Http
Imports Microsoft.AspNetCore.Mvc

Namespace Controllers

    ''' <summary>Endpoints para la administracion de productos.</summary>
    <ApiController>
    <Route("api/[controller]")>
    <Produces("application/json")>
    Public Class ProductosController
        Inherits ControllerBase

        Private ReadOnly _productoService As IProductoService

        Public Sub New(productoService As IProductoService)
            _productoService = productoService
        End Sub

        ''' <summary>Consulta todos los productos incluyendo los datos de su categoria.</summary>
        <HttpGet>
        <ProducesResponseType(StatusCodes.Status200OK)>
        Public Function ObtenerTodos() As ActionResult(Of IReadOnlyList(Of ProductoResponseDTO))
            Return Ok(_productoService.ObtenerTodos())
        End Function

        ''' <summary>Consulta un producto por su identificador.</summary>
        <HttpGet("{id:int}")>
        <ProducesResponseType(StatusCodes.Status200OK)>
        <ProducesResponseType(GetType(ErrorResponseDTO), StatusCodes.Status404NotFound)>
        Public Function ObtenerPorId(id As Integer) As ActionResult(Of ProductoResponseDTO)
            Return Ok(_productoService.ObtenerPorId(id))
        End Function

        ''' <summary>Consulta los productos pertenecientes a una categoria especifica.</summary>
        <HttpGet("categoria/{categoriaId:int}")>
        <ProducesResponseType(StatusCodes.Status200OK)>
        <ProducesResponseType(GetType(ErrorResponseDTO), StatusCodes.Status404NotFound)>
        Public Function ObtenerPorCategoria(<FromRoute> categoriaId As Integer) As ActionResult(Of IReadOnlyList(Of ProductoResponseDTO))
            Return Ok(_productoService.ObtenerPorCategoria(categoriaId))
        End Function

        ''' <summary>Registra un producto.</summary>
        <HttpPost>
        <ProducesResponseType(StatusCodes.Status201Created)>
        <ProducesResponseType(GetType(ErrorResponseDTO), StatusCodes.Status400BadRequest)>
        <ProducesResponseType(GetType(ErrorResponseDTO), StatusCodes.Status409Conflict)>
        Public Function Crear(<FromBody> dto As ProductoCreateDTO) As ActionResult(Of ProductoResponseDTO)

            Dim producto = _productoService.Crear(dto)

            Return CreatedAtAction(NameOf(ObtenerPorId),
                                   New With {.id = producto.Id},
                                   producto)

        End Function

        ''' <summary>Modifica un producto.</summary>
        <HttpPut("{id:int}")>
        <ProducesResponseType(StatusCodes.Status200OK)>
        <ProducesResponseType(GetType(ErrorResponseDTO), StatusCodes.Status400BadRequest)>
        <ProducesResponseType(GetType(ErrorResponseDTO), StatusCodes.Status404NotFound)>
        <ProducesResponseType(GetType(ErrorResponseDTO), StatusCodes.Status409Conflict)>
        Public Function Actualizar(id As Integer, <FromBody> dto As ProductoUpdateDTO) As ActionResult(Of ProductoResponseDTO)
            Return Ok(_productoService.Actualizar(id, dto))
        End Function

        ''' <summary>Elimina un producto.</summary>
        <HttpDelete("{id:int}")>
        <ProducesResponseType(StatusCodes.Status200OK)>
        <ProducesResponseType(GetType(ErrorResponseDTO), StatusCodes.Status404NotFound)>
        Public Function Eliminar(id As Integer) As IActionResult

            _productoService.Eliminar(id)

            Return Ok(New With {.mensaje = "Producto eliminado exitosamente."})

        End Function

    End Class

End Namespace
