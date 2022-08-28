namespace Management.Contracts

open System
open Management.Core.Workcell.PublicTypes
open Management.Core.WorkcellType
open SimpleTypes
    
module CreateWorkcell =
    
    type Request =
        { ShopId: string
          WorkcellCategory: string
          Description: string }
        
    type Response =
        { Id: Guid
          ShopId: string
          WorkcellCategory: string
          Description: string }
        
    let toNewUnvalidatedWorkcell (dto:Request) :UnvalidatedWorkcell =
        {
         ShopId = dto.ShopId
         WorkcellCategory = dto.WorkcellCategory
         Description = dto.Description
         }
            
    let fromDomain (domainObj: Workcell) :Response =
        {
            Id = domainObj.Id
            ShopId = domainObj.ShopId |> ShopId.value
            WorkcellCategory = domainObj.WorkcellCategory |> WorkcellCategory.value
            Description = domainObj.Description |> Option.map WorkcellDescription.value |> Utilities.Utils.defaultIfNone null
        }
        
type CreateWorkcellErrorDto = {
    Code: string
    Message: string
}
        
module CreateWorkcellErrorDto =
    let fromDomain (domainObj:CreateWorkcellError) :CreateWorkcellErrorDto =
        match domainObj with
        | Validation validationError ->
            let (ValidationError msg) = validationError
            {
                Code = "ValidationError"
                Message = msg
            }
        