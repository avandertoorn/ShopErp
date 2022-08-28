namespace WebApi
#nowarn "20"
open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

        builder.Services.AddControllersWithViews()
        builder.Services.AddRazorPages()

        let app = builder.Build()
        
        if app.Environment.IsDevelopment() then
            app.UseWebAssemblyDebugging()
        else
            app.UseExceptionHandler("/Error")
            app.UseHsts() |> ignore
        
        app.UseHttpsRedirection()

        app.UseBlazorFrameworkFiles()
        app.UseStaticFiles()
        
        app.UseRouting()
        
        app.UseAuthorization()
        app.MapControllers()

        app.MapRazorPages()
        app.MapControllers()
        app.MapFallbackToFile("index.html")
        
        app.Run()

        exitCode
