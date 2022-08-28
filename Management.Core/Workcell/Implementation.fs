module Management.Core.Workcell.Implementation

open System
open Management.Core.Workcell.InternalTypes
open Management.Core.Workcell.PublicTypes
open Management.Core.WorkcellType

let toShopId (checkShopIdIsUnique:CheckForUniqueShopId ) shopId =
    let checkShopId shopId =
        if checkShopIdIsUnique shopId then
            Ok shopId
        else
            let msg = $"Id %A{shopId} already used"
            Error (ValidationError msg)
            
    shopId
    |> ShopId.create "Shop Id"
    |> Result.mapError ValidationError
    |> Result.bind checkShopId
    
let toWorkcellCategory (checkWorkcellCategoryExists:CheckWorkcellCategoryExists) workCellCategory =
    let checkWorkcellCategory workCellCategory =
        if checkWorkcellCategoryExists workCellCategory then
            Ok workCellCategory
        else
            let msg = $"Workcell category &A{workCellCategory} is invalid"
            Error (ValidationError msg)
    
    workCellCategory
    |> WorkcellCategory.create "Workcell Category"
    |> Result.mapError ValidationError
    |> Result.bind checkWorkcellCategory
    
let toWorkcellDescription description =
    description
    |> WorkcellDescription.create "Workcell Description"
    |> Result.mapError ValidationError
    
let validateWorkcell : ValidateWorkcell =
    fun checkUniqueShopId checkWorkcellCategoryExists unvalidatedWorkcell ->
        asyncResult {
            let! shopId =
                unvalidatedWorkcell.ShopId
                |> toShopId checkUniqueShopId
                |> AsyncResult.ofResult
            let! workcellCategory =
                unvalidatedWorkcell.WorkcellCategory
                |> toWorkcellCategory checkWorkcellCategoryExists
                |> AsyncResult.ofResult
            let! workcellDescription =
                unvalidatedWorkcell.Description
                |> toWorkcellDescription
                |> AsyncResult.ofResult
            let validatedWorkcell : Workcell = {
                Id = Guid.NewGuid()
                ShopId = shopId
                WorkcellCategory = workcellCategory
                Description = workcellDescription
                }
            return validatedWorkcell
        }
        
let createWorkcellCreatedEvent (createdWorkcell:Workcell) : WorkcellCreated =
    {
        Id = createdWorkcell.Id
        ShopId = createdWorkcell.ShopId
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
        