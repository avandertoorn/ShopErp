module Management.Core.Workcell.InternalTypes

open Management.Core.Workcell.PublicTypes
open Management.Core.Workcell.CompoundTypes
open Management.Core.Workcell.SimpleTypes

type GetWorkcellCategory =
    int -> Result<WorkcellCategory, ValidationError>
    
type CheckForUniqueName =
    Name -> bool

type ValidateWorkcell =
   CheckForUniqueName 
     -> GetWorkcellCategory
     -> UnvalidatedWorkcell
     -> AsyncResult<Workcell, ValidationError>
     
type CreateEvents =
    Workcell -> CreateWorkcellEvent list