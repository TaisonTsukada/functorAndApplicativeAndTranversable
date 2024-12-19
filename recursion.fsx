open System
let rec map f lst =
  match lst with
  | [] -> []
  | head::tail -> f head :: (map f tail)
  
map (fun x -> x * 2) [1..400000] 
  
42 |> (fun x ->
  43 |> (fun y ->
    x + y |> (fun z ->
      z)))

let map f lst =
  let rec mapInner f lst acc =
    match lst with
    | [] -> acc
    | head::tail -> mapInner f tail ((f head) :: acc)
  mapInner f lst [] |> List.rev
  
let map3 f list =
    let cons x xs = x :: xs
    let rec mapInner acc = function
        | [] -> acc []
        | head::tail -> mapInner (fun result -> acc (cons (f head) result)) tail
    mapInner id list
    
type 'a Tree =
  | Leaf of 'a
  | Node of 'a Tree * 'a Tree


let rec mapv f tree =
  match tree with
  | Leaf x -> f x |> Leaf
  | Node (left, right ) ->
      let leftResult = mapv f left
      let rightResult = mapv f right
      Node(leftResult, rightResult)
      

let testTree = Node(
  Node(Leaf(1), Leaf(2)),
  Leaf(3))

let mapTree f tree =
  let rec mapInner tree continuation =
    printfn "Mapping tree %A" tree
    match tree with
    | Leaf x -> 
        Leaf(f x) |> continuation
    | Node (left, right) ->
        mapInner left (fun leftResult ->
          mapInner right (fun rightResult ->
            Node(leftResult, rightResult) |> continuation))
  mapInner tree id

