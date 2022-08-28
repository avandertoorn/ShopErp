namespace WebApi.Controllers

open System
open Management.Contracts
open Management.Contracts.CreateWorkcell
open Management.Core.Workcell
open Management.Core.Workcell.InternalTypes
open Management.Core.Workcell.PublicTypes
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Server.HttpSys
open Microsoft.FSharp.Core

type CreateWorkcellApi = Request -> AsyncResult<Response, CreateWorkcellError>

module Something =
    let checkForUniqueShopId : CheckForUniqueShopId =
        fun shopId ->
            printf "Checked if Unique Shop Id"
            true
            
    let checkWorkcellCategoryExists : CheckWorkcellCategoryExists =
        fun workcellCategory ->
            printf "Checked if Workcell Category Exists"
            true

[<ApiController>]
[<Route("[controller]")>]
type WorkcellController () =
    inherit ControllerBase()
    
    [<HttpPost>]
    member _.Post([<FromBody>] request : Request) : ActionResult =
        let unvalidatedWorkcell = request |> CreateWorkcell.toNewUnvalidatedWorkcell
        let workflow =
             Implementation.createWorkcell
               Something.checkForUniqueShopId
               Something.checkWorkcellCategoryExists
         
        let result = workflow unvalidatedWorkcell |> Async.StartAsTask
        match result.Result with
            | Ok workcell ->
                let workcellDto = workcell |> CreateWorkcell.fromDomain
                base.Ok workcellDto
            | Error error ->
                let dto = error |> CreateWorkcellErrorDto.fromDomain
                base.BadRequest dto
    
    [<HttpGet>]
    member _.Get() =
        { Id = Guid.NewGuid(); ShopId = "dsk"; WorkcellCategory = "Cat"; Description = "Dis" }