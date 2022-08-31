namespace Management.Contracts

open System
open Management.Core.Workcell.PublicTypes
open Management.Core.Workcell.CompoundTypes
open Management.Core.Workcell.SimpleTypes
open SimpleTypes
    
module WorkcellContracts =
            
    type WorkcellResponse =
        { Id: Guid
          Name: string
          WorkcellCategoryId: int
          Description: string }
        
    type GetAllWorkcellResponse =
        { Workcells: WorkcellResponse list }
        
    type CreateRequest =
        { Name: string
          WorkcellCategoryId: int
          Description: string }
        
    type UpdateRequest =
        { Id: Guid
          Name: string
          WorkcellCategory: string
          Description: string }
    
    type DeleteRequest =
        { Id: Guid }
        
    let toUnvalidatedWorkcell (dto:CreateRequest) :UnvalidatedWorkcell =
        { Id = Guid.NewGuid()
          Name = dto.Name
          WorkcellCategoryId = dto.WorkcellCategoryId
          Description = dto.Description }
            
    let fromDomain (domainObj: Workcell) :WorkcellResponse =
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