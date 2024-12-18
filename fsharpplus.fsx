#r @"nuget: FSharpPlus"

open FSharpPlus
open FSharpPlus.Data

// Apply +4 to a list
let lst5n6  = map ((+) 4) [ 1; 2 ]

// Apply +4 to an array
let arr5n6  = map ((+) 4) [|1; 2|]

// I could have written this
let arr5n6' = (+) <!> [|4|] <*> [|1; 2|]

// Add two options
let opt120  = (+) <!> Some 20 <*> tryParse "100"


// Applicatives need Return (result)

// Test return
let resSome22: Option<_> = result 22
let resSing22 : list<_>   = result 22
let resLazy22 : Lazy<_>   = result 22
let (quot5 : Microsoft.FSharp.Quotations.Expr<int>) = result 5

// Example
type Person = { Name: string; Age: int } with static member create n a = { Name = n; Age = a }

let person1 = Person.create <!> tryHead ["gus"] <*> tryParse "42"
let person2 = Person.create <!> tryHead ["gus"] <*> tryParse "fourty two"
let person3 = Person.create <!> tryHead ["gus"] <*> (tryHead ["42"] >>= tryParse)


// Other ways to write applicative expressions


// Function lift2 helps in many cases

let person1' = (tryHead ["gus"], tryParse "42")               ||> lift2 Person.create 
let person2' = (tryHead ["gus"], tryParse "fourty two")       ||> lift2 Person.create 
let person3' = (tryHead ["gus"], tryHead ["42"] >>= tryParse) ||> lift2 Person.create 


// Using Idiom brackets from http://www.haskell.org/haskellwiki/Idiom_brackets

let res3n4   = iI ((+) 2) [1;2] Ii
let res3n4'  = iI (+) (result 2) [1;2] Ii
let res18n24 = iI (+) (ZipList(seq [8;4])) (ZipList(seq [10;20])) Ii

let tryDiv x y = if y = 0 then None else Some (x </div/> y)
let resSome3   = join (iI tryDiv (Some 6) (Some 2) Ii)
let resSome3'  =       iI tryDiv (Some 6) (Some 2) Ji

let tryDivBy y = if y = 0 then None else Some (fun x -> x </div/> y)
let resSome2  = join (result tryDivBy  <*> Some 4) <*> Some 8
let resSome2' = join (   iI tryDivBy (Some 4) Ii) <*> Some 8

let resSome2'' = iI tryDivBy (Some 4) J (Some 8) Ii
let resNone    = iI tryDivBy (Some 0) J (Some 8) Ii
let res16n17   = iI (+) (iI (+) (result 4) [2; 3] Ii) [10] Ii

let opt121  = iI (+) (Some 21) (tryParse "100") Ii
let opt122  = iI tryDiv (tryParse "488") (trySqrt 16) Ji


// Using applicative math operators

open FSharpPlus.Math.Applicative

let opt121'  = Some 21 .+. tryParse "100"
let optTrue  = 30 >. tryParse "29"
let optFalse = tryParse "30" .< 29
let m1m2m3 = -.[1; 2; 3]


// Using applicative computation expression

let getName s = tryHead s
let getAge  s = tryParse s

let person4 = applicative {
    let! name = getName ["gus"]
    and! age  = getAge "42"
    return { Name = name; Age = age } }