Imports ApiInventario.DTOs
Imports ApiInventario.Exceptions
Imports Microsoft.AspNetCore.Http
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Logging

Namespace Middleware

    ''' <summary>
    ''' Middleware de manejo global de excepciones.
    ''' Traduce las excepciones de la aplicacion a respuestas HTTP
    ''' sin exponer informacion interna de las excepciones al consumidor de la API.
    ''' </summary>
    Public Class ExceptionHandlingMiddleware

        Private ReadOnly _next As RequestDelegate

        Private ReadOnly _logger As ILogger(Of ExceptionHandlingMiddleware)

        Public Sub New(siguiente As RequestDelegate,
                       logger As ILogger(Of ExceptionHandlingMiddleware))

            _next = siguiente
            _logger = logger

        End Sub

        Public Async Function InvokeAsync(context As HttpContext) As Task

            Dim errorCapturado As Exception = Nothing
            Dim codigoHttp As Integer = StatusCodes.Status500InternalServerError
            Dim mensaje As String = "Ocurrio un error interno en la API. Intenta nuevamente mas tarde."

            Try
                Await _next(context)
            Catch ex As ReglaNegocioException
                errorCapturado = ex
                codigoHttp = ex.CodigoHttp
                mensaje = ex.Message
            Catch ex As DbUpdateException
                _logger.LogError(ex, "Error de persistencia al ejecutar la operacion.")
                errorCapturado = ex
                codigoHttp = StatusCodes.Status409Conflict
                mensaje = "La operacion no puede completarse porque ya existe un registro con los mismos datos."
            Catch ex As Exception
                _logger.LogError(ex, "Error inesperado al procesar la solicitud.")
                errorCapturado = ex
            End Try

            If errorCapturado IsNot Nothing Then
                Await EscribirError(context, codigoHttp, mensaje)
            End If

        End Function

        Private Async Function EscribirError(context As HttpContext,
                                             codigo As Integer,
                                             descripcion As String) As Task

            If context.Response.HasStarted Then
                Return
            End If

            context.Response.Clear()
            context.Response.StatusCode = codigo
            context.Response.ContentType = "application/json; charset=utf-8"

            Dim respuesta As New ErrorResponseDTO With {
                .Mensaje = descripcion
            }

            Await context.Response.WriteAsJsonAsync(respuesta)

        End Function

    End Class

End Namespace
