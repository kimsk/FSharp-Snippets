(**
A continuation is simply a function that you pass into another function to tell it what to do next.
*)
let divide ifZero ifSuccess top bottom =
    if(bottom=0)
    then ifZero()
    else ifSuccess(top/bottom)

let isEven ifOdd ifEven aNumber =
    if(aNumber%2 = 0)
    then aNumber |> ifEven
    else aNumber |> ifOdd

// Scenario 1: pipe the result into a message
// ----------------------------------------
// setup the functions to print a message
let ifZero1 () = printfn "bad"
let ifSuccess1 x = printfn "good %i" x

// use partial application
let divide1  = divide ifZero1 ifSuccess1

//test
let good1 = divide1 6 3
let bad1 = divide1 6 0

// Scenario 2: convert the result to an option
// ----------------------------------------
// setup the functions to return an Option
let ifZero2() = None
let ifSuccess2 x = Some x
let divide2  = divide ifZero2 ifSuccess2

//test
let good2 = divide2 6 3
let bad2 = divide2 6 0

// Scenario 3: throw an exception in the bad case
// ----------------------------------------
// setup the functions to throw exception
let ifZero3() = failwith "div by 0"
let ifSuccess3 x = x
let divide3  = divide ifZero3 ifSuccess3

//test
let good3 = divide3 6 3
let bad3 = divide3 6 0

// Mark Needham 
// F# continuation passing style (CPS)
let identity value = value
let f v = 1 + v
f (identity 5)

let identity' value k = k value
identity' 5 f

// From let
let x = 42
let y = 43
let z = x + y
// to CPS
42 |> (fun x -> 
    43 |> fun y -> 
        x + y |> fun z -> z )

