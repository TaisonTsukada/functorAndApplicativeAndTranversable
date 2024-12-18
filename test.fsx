#r "nuget: FSharpPlus, 1.6.1"
open FSharpPlus
open System
type ResultError = ResultError of string
let sequence = traverse id
let result x = Result.map (fun f -> f x)

let rec invert x =
  match x with
  | [] -> result []
  | head::tail -> result (fun h t -> h::t) <*> head <*> (invert tail)

let listResult: Result<int,ResultError> list = [ Ok 1; Ok 2; Error ("errorだよ" |> ResultError) ]
let resultList: Result<int list, ResultError> = sequence listResult 
printfn "%A" resultList

  
let listAsync = [ async.Return 1; async.Return 2; async.Return 3 ]
printfn "%A" (invert listAsync)
let hoge = invert listAsync