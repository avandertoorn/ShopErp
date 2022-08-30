namespace Management.Core.Workcell.CompoundTypes

open System
open Management.Core.Workcell.SimpleTypes

type WorkcellCategory =
    { Id: int
      CategoryName: CategoryName }

type Workcell =
    { Id: Guid
      Name: Name
      WorkcellCategory: WorkcellCategory
      Description: WorkcellDescription option }