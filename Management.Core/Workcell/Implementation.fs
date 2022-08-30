module Management.Core.Workcell.Implementation

open System
open Management.Core.Workcell.InternalTypes
open Management.Core.Workcell.PublicTypes
open Management.Core.Workcell.CompoundTypes
open Management.Core.Workcell.SimpleTypes
let toName (checkWorkcellNameIsUnique:CheckForUniqueName ) name =
    let checkName name =
        if checkWorkcellNameIsUnique name then
            Ok name
        else
            let msg = $"Id %A{name} already used"
            Error (ValidationError msg)
            
    name
    |> Name.create "Workcell Name"
    |> Result.mapError ValidationError
    |> Result.bind checkName
    
let toWorkcellCategory (getWorkcellCategory:GetWorkcellCategory) workcellCategory =
    workcellCategory
    |> getWorkcellCategory
    
let toWorkcellDescription description =
    description
    |> WorkcellDescription.create "Workcell Description"
    |> Result.mapError ValidationError
    
let validateWorkcell : ValidateWorkcell =
    fun checkUniqueShopId getWorkcellCategory unvalidatedWorkcell ->
        asyncResult {
            let! name =
                unvalidatedWorkcell.Name
                |> toName checkUniqueShopId
                |> AsyncResult.ofResult
            let! workcellCategory =
                unvalidatedWorkcell.WorkcellCategoryId
                |> toWorkcellCategory getWorkcellCategory
                |> AsyncResult.ofResult
            let! workcellDescription =
                unvalidatedWorkcell.Description
                |> toWorkcellDescription
                |> AsyncResult.ofResult
            let validatedWorkcell : Workcell = {
                Id = unvalidatedWorkcell.Id
                Name = name
                WorkcellCategory = workcellCategory
                Description = workcellDescription
                }
            return validatedWorkcell
        }
        
let createWorkcellCreatedEvent (createdWorkcell:Workcell) : WorkcellCreated =
    {
        Id = createdWorkcell.Id
        Name = createdWorkcell.Name
    }
        
let createEvents : CreateEvents =
    fun workcell ->
        let createdWorkcellEvents =
            workcell
            |> createWorkcellCreatedEvent
            |> CreateWorkcellEvent.WorkcellCreated
            |> List.singleton
        [
        yield! createdWorkcellEvents
        ]
        
let createWorkcell
    checkUniqueShopId
    checkWorkcellCategoryExists
    : CreateWorkcell =
    
    fun unvalidatedWorkcell ->
        asyncResult {
            let! validatedWorkcell =
                validateWorkcell checkUniqueShopId checkWorkcellCategoryExists unvalidatedWorkcell
                |> AsyncResult. mapError CreateWorkcellError.Validation
            let events =
                createEvents validatedWorkcell
                //TODO: send domain event
            return validatedWorkcell
        }
        