module Management.Core.Workcell.SimpleTypes

open SimpleTypes

type Name = private Name of string
type CategoryName = private CategoryName of string
type WorkcellDescription = private WorkcellDescription of string

module CategoryName =
    let value (CategoryName w) = w
    let create fieldName str =
        ConstrainedType.createString fieldName CategoryName 20 str

module Name =
    let value (Name w) = w
    let create fieldName str =
        ConstrainedType.createString fieldName Name 20 str
        
module WorkcellDescription =
    let value (WorkcellDescription w) = w
    let create fieldName str =
        ConstrainedType.createStringOption fieldName WorkcellDescription 200 str