Imports Microsoft.AspNetCore.Http

Namespace Exceptions

    ''' <summary>
    ''' Excepción utilizada cuando ya existe un registro con los mismos datos.
    ''' </summary>
    Public Class EntidadDuplicadaException
        Inherits ReglaNegocioException

        Public Sub New(mensaje As String)
            MyBase.New(mensaje, StatusCodes.Status409Conflict)
        End Sub

    End Class

End Namespace
