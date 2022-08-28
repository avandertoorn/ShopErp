module Management.Core.Workcell.InternalTypes

open Management.Core.Workcell.PublicTypes
open Management.Core.WorkcellType

type CheckWorkcellCategoryExists =
    WorkcellCategory -> bool
    
type CheckForUniqueShopId =
    ShopId -> bool

type ValidateWorkcell =
   CheckForUniqueShopId 
     -> CheckWorkcellCategoryExists
     -> UnvalidatedWorkcell
     -> AsyncResult<Workcell, ValidationError>
     
type CreateEvents =
    Workcell -> CreateWorkcellEvent list