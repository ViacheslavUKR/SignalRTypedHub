Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Hosting
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Logging
Imports Microsoft.Extensions.Options
Imports Microsoft.AspNetCore.Authentication
Imports Microsoft.Extensions.Hosting
Imports Microsoft.AspNetCore.Http.Connections

Public Class Startup

    Public Shared Property _Configuration As IConfiguration
    Public Shared Property _Environment As IWebHostEnvironment

    Public Sub New(ByVal configuration As IConfiguration, env As IWebHostEnvironment)
        _Configuration = configuration
        _Environment = env
    End Sub

    Public Sub ConfigureServices(ByVal services As IServiceCollection)
        services.AddSignalR().
        AddHubOptions(Of OutputMessages)(Sub(options)
                                             options.EnableDetailedErrors = True
                                         End Sub)
        services.
            AddMvcCore(Sub(opt) opt.EnableEndpointRouting = False).
            SetCompatibilityVersion(CompatibilityVersion.Latest).
            AddFormatterMappings()
    End Sub


    Public Sub Configure(ByVal app As IApplicationBuilder, ByVal env As IHostEnvironment)
        If env.IsDevelopment() Then
            app.UseDeveloperExceptionPage()
        End If
        app.UseMvc
        app.UseRouting
        app.UseEndpoints(Sub(endpoints)
                             endpoints.MapHub(Of OutputMessages)("/OutputMessages", Sub(opt)
                                                                                        opt.Transports =
                                                                                        HttpTransportType.WebSockets Or
                                                                                        HttpTransportType.LongPolling
                                                                                    End Sub)
                         End Sub)
    End Sub
End Class

