Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.AspNetCore.SignalR
Imports Microsoft.Extensions.Logging

<Route("/Test")>
<ApiController>
<Produces("application/json")>
Public Class TestController
    Inherits ControllerBase

    Private ReadOnly _logger As ILogger(Of TestController)
    Private ReadOnly _OutputMessages As IHubContext(Of OutputMessages, IOutputMessages)
    Public Sub New(logger As ILogger(Of TestController), OutputMessages As IHubContext(Of OutputMessages, IOutputMessages))
        _logger = logger
        _OutputMessages = OutputMessages
    End Sub

    <HttpGet>
    Public Async Sub Index()
        Await _OutputMessages.Clients.All.AdminMessages("Test message")
    End Sub
End Class
