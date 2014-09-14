type Result = Success of float | DivByZero

let divide x y =
  match y with
  | 0. -> DivByZero
  | y -> Success(x/y)

type DefinedBuilder() =
  member __.Bind(x:Result, rest:float->Result) =
      match x with
      | DivByZero -> DivByZero
      | Success(x) -> rest x

  member __.Return(x) = x

let totalResistance r1 r2 r3 =
  let defined = DefinedBuilder()
  defined {
    let! x = divide 1. r1
    let! y = divide 1. r2
    let! z = divide 1. r3
    return divide 1. (x + y + z)
  }


let b = match Success(1.) with Success(x) -> x | _ -> 0.

let (Success z) = Success(1.)

type LoggingBuilder() =
  member __.Bind(x,f) =
    printfn "%s" x
    f x

  member __.Zero() =     
    ()

let logging = LoggingBuilder()

logging {
  let! _ = "A"
  let! _ = "B"
  let! _ = "C"
  ()
}