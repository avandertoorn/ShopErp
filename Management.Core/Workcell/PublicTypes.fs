module Management.Core.Workcell.PublicTypes

open System
open Management.Core.WorkcellType

type UnvalidatedWorkcell = {
    ShopId : string
    WorkcellCategory : string
    Description : string
}

type WorkcellCreated = {
    Id: Guid
    ShopId: ShopId
}

type CreateWorkcellEvent =
    | WorkcellCreated of WorkcellCreated

type ValidationError = ValidationError of string

type CreateWorkcellError =
    | Validation of ValidationError
    
type CreateWorkcell =
    UnvalidatedWorkcell -> AsyncResult<Workcell, CreateWorkcellError>