namespace SimpleTypes

open System

module ConstrainedType =
    let createGuidFromString fieldName ctor str =
        let mutable res = Guid.Empty
        if String.IsNullOrEmpty(str) then
            let msg = $"%s{fieldName} must not be null or empty"
            Error msg
        elif Guid.TryParse(str, &res) then
            if res = Guid.Empty then
                let msg = $"%s{fieldName} is not a valid Guid"
                Error msg
            else
                Ok (ctor res)
        else
            let msg = $"%s{fieldName} is not a valid Guid"
            Error msg
        
    let createString fieldName ctor maxLen str =
        if String.IsNullOrEmpty(str) then
            let msg = $"%s{fieldName} must not be null or empty"
            Error msg
        elif str.Length > maxLen then
            let msg = $"%s{fieldName} must not be more than %i{maxLen} chars"
            Error msg
        else
            Ok (ctor str)
            
    let createStringOption fieldName ctor maxLen str =
        if String.IsNullOrEmpty(str) then
            Ok None
        elif str.Length > maxLen then
            let msg = $"%s{fieldName} must not be more than %i{maxLen} chars"
            Error msg
        else
            Ok (ctor str |> Some)
            
    let createInt fieldName ctor minVal maxVal i =
        if i < minVal then
            let msg = $"%s{fieldName}: Must not be less than %i{minVal}"
            Error msg
        elif i > maxVal then
            let msg = $"%s{fieldName}: Must not be greater than %i{maxVal}"
            Error msg
        else
            Ok (ctor i)
            
    let createDecimal fieldName ctor minVal maxVal i =
        if i < minVal then
            let msg = $"%s{fieldName}: Must not be less than %M{minVal}"
            Error msg
        elif i > maxVal then
            let msg = $"%s{fieldName}: Must not be greater than %M{maxVal}"
            Error msg
        else
            Ok (ctor i)
            
    let createLike fieldName  ctor pattern str =
        if String.IsNullOrEmpty(str) then
            let msg = $"%s{fieldName}: Must not be null or empty"
            Error msg
        elif System.Text.RegularExpressions.Regex.IsMatch(str,pattern) then
            Ok (ctor str)
        else
            let msg = $"%s{fieldName}: '%s{str}' must match the pattern '%s{pattern}'"
            Error msg
            
type GuidId = private GuidId of Guid

module GuidId =
    let value (GuidId g) = g
    let createFromString fieldName str =
        ConstrainedType.createGuidFromString fieldName GuidId str
        
type String500 = private String500 of string

module String500 =
    let value (String500 string) = string
    let create fieldName str =
        ConstrainedType.createStringOption fieldName String500 500 str