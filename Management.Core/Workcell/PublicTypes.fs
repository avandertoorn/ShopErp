module Management.Core.Workcell.PublicTypes

open System
open Management.Core.Workcell.CompoundTypes
open Management.Core.Workcell.SimpleTypes

type UnvalidatedWorkcell = {
    Id: Guid
    Name : string
    WorkcellCategoryId : int
    Description : string
}

type WorkcellCreated = {
    Id: Guid
    Name: Name
}

type CreateWorkcellEvent =
    | WorkcellCreated of WorkcellCreated

type ValidationError = ValidationError of string

type CreateWorkcellError =
    | Validation of ValidationError
    
type CreateWorkcell =
    UnvalidatedWorkcell -> AsyncResult<Workcell, CreateWorkcellError>