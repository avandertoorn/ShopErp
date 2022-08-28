module SimpleTypes.Utilities

[<AutoOpen>]
module Utils =
    let defaultIfNone defaultValue opt =
        match opt with
        | Some v -> v
        | None -> defaultValue