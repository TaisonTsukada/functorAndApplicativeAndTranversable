#r "nuget: FSharpPlus, 1.6.1"
open FSharpPlus

type ItemId = ItemId of string
type BasketId = BasketId of string
type CheckoutId = CheckoutId of string
type BasketItem = 
    { ItemId: ItemId
      Quantity: float }

type Basket = 
    { Id: BasketId; 
      Items: BasketItem list }

type ReservedBasketItem =
    { ItemId: ItemId
      Price: float }

type Checkout = 
    { Id: CheckoutId
      BasketId: BasketId
      Price: float }
let reserveBasketItem (item: BasketItem): ReservedBasketItem option = failwith "TODO"

let consOption (head: 'a option) (tail: option<list<'a>>): option<list<'a>> =
  match head, tail with
  | Some h, Some t -> Some (h::t)
  | _ -> None


let rec sequence (reservedItems: list<option<ReservedBasketItem>>): option<list<ReservedBasketItem>> =
  match reservedItems with
  | [] -> Some []
  | head::tail -> Some (fun h t -> h :: t) <*> head <*> sequence tail

let rec traverse (f: 'a -> 'b option) (list: 'a list): option<list<'b>> =
    let cons head tail = head :: tail
    match list with
    | [] -> Some []
    | head :: tail -> Some cons  <*> (f head) <*> (traverse f  tail)

  
let rec inverts lst =
  let cons head tail =  head :: tail
  match lst with
  | [] -> Some []
  | head::tail -> Some cons <*> head <*> inverts tail


let createCheckout (basket: Basket): Checkout option =
  let reservedItems = traverse reserveBasketItem basket.Items
  reservedItems
  |> Option.map
    (fun x -> {
        Id = CheckoutId "1"
        BasketId = basket.Id
        Price = x |> List.sumBy (fun x -> x.Price)
    })



module Result =
  let apply a f = 
    match f, a  with
    | Ok g, Ok x ->  g x |> Ok
    | Error e, Ok _ -> e |> Error
    | Ok _, Error e -> e |> Error
    | Error e, Error _ -> e |> Error
  
  let pure' a = Ok a
  let rec sequence lst =
    let cons head tail = head :: tail
    match lst with
    | [] -> pure' []
    | head::tail -> Ok cons |> apply head |> apply (sequence tail)
    