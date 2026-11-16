Imports Microsoft.AspNetCore.Http

Namespace Exceptions

    ''' <summary>
    ''' Excepción que representa el incumplimiento de una regla de negocio.
    ''' </summary>
    Public Class ReglaNegocioException
        Inherits Exception

        ''' <summary>
        ''' Código de estado HTTP con el que se responde al consumidor de la API.
        ''' </summary>
        Public Property CodigoHttp As Integer

        Public Sub New(mensaje As String, codigoHttp As Integer)
            MyBase.New(mensaje)
            Me.CodigoHttp = codigoHttp
        End Sub

    End Class

End Namespace
