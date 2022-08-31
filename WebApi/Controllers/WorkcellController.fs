namespace WebApi.Controllers

open System
open Management.Contracts
open Management.Contracts.WorkcellContracts
open Management.Core.Workcell
open Management.Core.Workcell.CompoundTypes
open Management.Core.Workcell.InternalTypes
open Management.Core.Workcell.PublicTypes
open Management.Core.Workcell.SimpleTypes
open Microsoft.AspNetCore.Mvc
open Microsoft.FSharp.Core

type CreateWorkcellApi = CreateRequest -> AsyncResult<WorkcellResponse, CreateWorkcellError>

module Something =
    let dummyCheckForUniqueName : CheckForUniqueName =
        fun shopId ->
            printf "Checked if Unique Shop Id"
            true
            
    //In reality it would return from database
    let dummyCheckWorkcellCategoryExists : GetWorkcellCategory =
        fun workcellCategory ->
            result {
                let! name =
                    "Dummy Name"
                    |> CategoryName.create "categoryName"
                    |> Result.mapError ValidationError
                let category: WorkcellCategory = {
                    Id = workcellCategory
                    CategoryName = name
                }
            return category
        }

[<ApiController>]
[<Route("api/[controller]")>]
type WorkcellController () =
    inherit ControllerBase()
    
    [<HttpPost>]
    member _.Post([<FromBody>] request : CreateRequest) : ActionResult =
        let unvalidatedWorkcell = request |> toWorkcell
        let workflow =
             Implementation.createWorkcell
               Something.dummyCheckForUniqueName
               Something.dummyCheckWorkcellCategoryExists
         
        let result = workflow unvalidatedWorkcell |> Async.StartAsTask
        match result.Result with
            | Ok workcell ->
                let workcellDto = workcell |> fromDomain
                base.Ok workcellDto
            | Error error ->
                let dto = error |> CreateWorkcellErrorDto.fromDomain
                base.BadRequest dto
    
    [<HttpGet>]
    member _.Get() =
        let a: WorkcellResponse = { Id = Guid.NewGuid(); Name = "dsk"; WorkcellCategoryId = 5; Description = "Dis" }
        a