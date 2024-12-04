#r "nuget: FSharpPlus, 1.6.1"
type CreditCard =
    { Number: string
      Expiry: string
      Cvv: string }

let validateNumber number: Result<string, string> =
    if String.length number > 16 then
        Error "A credit card number must be less than 16 characters"
    else
        Ok number


let validateExpiry expiry: Result<string, string> =
  if String.length expiry > 5 then
    Error "A Credit Card expiry must be 5 characters long"
  else
    Ok expiry

let validateCvv cvv: Result<string, string> =
  if String.length cvv > 3 then
    Error "A Credit Card CVV must be 3 characters long"
  else
    Ok cvv


let bind f result =
  match result with
  | Ok value -> f value
  | Error error -> Error error

let createCreditCard number expiry cvv =
    { Number = number
      Expiry = expiry
      Cvv = cvv }

let apply a f =
    match f, a with
    | Ok g, Ok x -> g x |> Ok
    | Error e, Ok _ -> e |> Error
    | Ok _, Error e -> e |> Error
    | Error e1, Error _ -> e1 |> Error

let inline (<!>) f x = Result.map f x


let validateCreditCard (creditCard: CreditCard): Result<CreditCard, string> =
  createCreditCard
  <!> validateNumber creditCard.Number
  |> apply (validateExpiry creditCard.Expiry)
  |> apply (validateCvv creditCard.Cvv)

