open NUnit.Framework
open FsUnit

module Calculator = 
  let add x y = x + y
  let mul x y = x * y

module Tests =
  [<Test>]
  let ``add should work``() =
    Calculator.add 10 10 |> should equal 20

  [<Test>]
  let ``mul should work``() =
    Calculator.mul 10 10 |> should equal 100