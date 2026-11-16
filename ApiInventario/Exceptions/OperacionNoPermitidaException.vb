Imports Microsoft.AspNetCore.Http

Namespace Exceptions

    ''' <summary>
    ''' Excepción utilizada cuando la operación no puede realizarse por el estado actual de los datos.
    ''' </summary>
    Public Class OperacionNoPermitidaException
        Inherits ReglaNegocioException

        Public Sub New(mensaje As String)
            MyBase.New(mensaje, StatusCodes.Status409Conflict)
        End Sub

    End Class

End Namespace
