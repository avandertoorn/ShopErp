namespace Management.Core.WorkcellType

open System
open SimpleTypes

type ShopId = private ShopId of string
type WorkcellCategory = private WorkcellCategory of string
type WorkcellDescription = private WorkcellDescription of string
        

module ShopId =
    let value (ShopId w) = w
    let create fieldName str =
        ConstrainedType.createString fieldName ShopId 20 str

module WorkcellCategory =
    let value (WorkcellCategory w) = w
    let create fieldName str =
        ConstrainedType.createString fieldName WorkcellCategory 20 str
module WorkcellDescription =
    let value (WorkcellDescription w) = w
    let create fieldName str =
        ConstrainedType.createStringOption fieldName WorkcellDescription 200 str

type Workcell =
    { Id: Guid
      ShopId: ShopId
      WorkcellCategory: WorkcellCategory
      Description: WorkcellDescription option }