// Part 1 - create a workflow
let strToInt str = 
  let ok, value = System.Int32.TryParse(str)
  if ok 
  then Some(value)
  else None

type MaybeBuilder() =
  member this.Bind(m, f) = Option.bind f m
  member this.Return(x) = Some x

let maybe = new MaybeBuilder()

let stringAddWorkflow x y z =
//  maybe
//    {
//      let! a = strToInt x
//      let! b = strToInt y
//      let! c = strToInt z
//      return a + b + c
//    }
  maybe.Bind(strToInt x, fun a ->
  maybe.Bind(strToInt y, fun b ->
  maybe.Bind(strToInt z, fun c ->
    maybe.Return(a + b + c)
  )))

let good = stringAddWorkflow "12" "3" "2"
let bad = stringAddWorkflow "12" "xyz" "2"

// Part 2 - Create a bind function
let strAdd str i = 
  let ok, value = System.Int32.TryParse(str)
  if ok 
  then Some(i + value)
  else None

let (>>=) m f = Option.bind f m

let good' = strToInt "1" >>= strAdd "2" >>= strAdd "3"
let bad' = strToInt "1" >>= strAdd "xyz" >>= strAdd "3"