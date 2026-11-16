Imports Microsoft.AspNetCore.Http

Namespace Exceptions

    ''' <summary>
    ''' Excepción utilizada cuando la entidad solicitada no existe.
    ''' </summary>
    Public Class EntidadNoEncontradaException
        Inherits ReglaNegocioException

        Public Sub New(mensaje As String)
            MyBase.New(mensaje, StatusCodes.Status404NotFound)
        End Sub

    End Class

End Namespace
