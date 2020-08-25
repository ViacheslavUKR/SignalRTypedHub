
Imports Microsoft.AspNetCore.SignalR
Imports Microsoft.Extensions.Logging

Public Class OutputMessages
    Inherits Hub(Of IOutputMessages)

    Public Shared ReadOnly ConnectedUserList As Dictionary(Of String, String) = New Dictionary(Of String, String)(1024)
    Private ReadOnly _hubContext As IHubContext(Of OutputMessages, IOutputMessages)
    Private ReadOnly _logger As ILogger(Of OutputMessages)

    Public Sub New(logger As ILogger(Of OutputMessages), hubContext As IHubContext(Of OutputMessages, IOutputMessages))
        _hubContext = hubContext
        _logger = logger
    End Sub

    Public Async Function AdminMessages(Message As String) As Task
        Await _hubContext.Clients.All.AdminMessages(Message)
    End Function

    Public Overrides Async Function OnConnectedAsync() As Task
        Dim JWT As String = Context.GetHttpContext().Request.Headers("Authorization")
        Dim ValidUserID As String = ""

        Try
            ValidUserID = UserHelpers.ValidateJWT(JWT)
        Catch e As Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException
            _logger.LogInformation("token experied")
        End Try

        If ValidUserID IsNot Nothing Then
            _logger.LogInformation($"User {ValidUserID} connected to {Me.[GetType]().Name} hub")
            If ConnectedUserList.Any(Function(x) x.Key = ValidUserID) Then ConnectedUserList.Remove(ValidUserID)
            ConnectedUserList.Add(ValidUserID, Context.ConnectionId)
        Else
            _logger.LogInformation("WrongToken")
        End If

        Await MyBase.OnConnectedAsync()
    End Function

    Public Overrides Async Function OnDisconnectedAsync(ByVal exception As System.Exception) As Task
        Dim JWT = Context.GetHttpContext().Request.Headers("Authorization")
        Dim ValidUserID As String = ""

        Try
            ValidUserID = UserHelpers.ValidateJWT(JWT)
        Catch e As Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException
            _logger.LogInformation("token experied")
        End Try

        If ValidUserID IsNot Nothing Then
            _logger.LogInformation($"User {ValidUserID} disconnected from {Me.[GetType]().Name} hub")
            If ConnectedUserList.Any(Function(x) x.Key = ValidUserID) Then ConnectedUserList.Remove(ValidUserID)
            ConnectedUserList.Add(ValidUserID, Context.ConnectionId)
        Else
            _logger.LogInformation("WrongToken")
        End If

        Await MyBase.OnDisconnectedAsync(exception)
    End Function


End Class
