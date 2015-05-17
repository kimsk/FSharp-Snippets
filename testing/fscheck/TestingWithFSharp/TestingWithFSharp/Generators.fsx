#r @"..\packages\FsCheck.1.0.4\lib\net45\FsCheck.dll"

open FsCheck

let fizzbuzz = function
    | d when d%15 = 0 -> "fizzbuzz"
    | d when d%3 = 0 -> "fizz"
    | d when d%5 = 0 -> "buzz"
    | d -> sprintf "%d" d

let ``fizz when evenly divisible by 3`` d =
    (d > 0 && d%3 = 0 && d%5 <> 0) ==> ("fizz" = fizzbuzz d)

Check.Quick ``fizz when evenly divisible by 3``

// a generators generating 1 to s
let posNumberGenerator = Gen.sized <| fun s -> Gen.choose(1,s)

// a shrinker
let positiveIntegers =
    {new Arbitrary<int>() with
        override x.Generator = posNumberGenerator
        override x.Shrinker t = Seq.empty}

// shorthand function 
let positiveIntegers' =
    Arb.Default.Int32()
    |> Arb.mapFilter abs (fun n -> n > 0)

// putting them together
type FizzBuzz = 
    // positive integers generator
    static member positiveIntegers' =
        Arb.Default.Int32()
        |> Arb.mapFilter abs (fun n -> n > 0)

    // property
    static member ``fizz when evenly divisible by 3`` d =
        (d%3 = 0 && d%5 <> 0) ==> ("fizz" = fizzbuzz d)

    static member ``fizz when evenly divisible by 5`` d =
        (d%3 <> 0 && d%5 = 0) ==> ("buzz" = fizzbuzz d)

    static member ``fizz when evenly divisible by 15`` d =
        (d%3 = 0 && d%5 = 0) ==> ("fizzbuzz" = fizzbuzz d)


Arb.registerByType(typeof<FizzBuzz>)
Check.QuickAll(typeof<FizzBuzz>)
Check.VerboseAll(typeof<FizzBuzz>)
