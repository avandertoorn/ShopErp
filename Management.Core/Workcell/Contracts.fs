namespace Management.Contracts

open System
open Management.Core.Workcell.PublicTypes
open Management.Core.Workcell.CompoundTypes
open Management.Core.Workcell.SimpleTypes
open SimpleTypes
    
module WorkcellContracts =
    
    type CreateRequest =
        { Name: string
          WorkcellCategoryId: int
          Description: string }
        
    type CreateResponse =
        { Id: Guid
          Name: string
          WorkcellCategoryId: int
          Description: string }
        
    type UpdateRequest =
        { Id: Guid
          Name: string
          WorkcellCategory: string
          Description: string }
    type UpdateResponse =
        { Id: Guid
          Name: string
          WorkcellCategory: string
          Description: string }
        
    let toWorkcell (dto:CreateRequest) :UnvalidatedWorkcell =
        { Id = Guid.NewGuid()
          Name = dto.Name
          WorkcellCategoryId = dto.WorkcellCategoryId
          Description = dto.Description }
            
    let fromDomain (domainObj: Workcell) :CreateResponse =
        { Id = domainObj.Id
          Name = domainObj.Name |> Name.value
          WorkcellCategoryId = domainObj.WorkcellCategory.Id
          Description = domainObj.Description |> Option.map WorkcellDescription.value |> Utilities.Utils.defaultIfNone null }
        
type CreateWorkcellErrorDto =
    { Code: string
      Message: string }

module CreateWorkcellErrorDto =
    let fromDomain (domainObj:CreateWorkcellError) :CreateWorkcellErrorDto =
        match domainObj with
        | Validation validationError ->
            let (ValidationError msg) = validationError
            {
                Code = "ValidationError"
                Message = msg
            }