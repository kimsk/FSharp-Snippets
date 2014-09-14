#r @"C:\nuget\NUnit.2.6.3\lib\nunit.framework.dll"
#r @"C:\nuget\FsUnit.1.3.0.1\Lib\Net40\FsUnit.NUnit.dll"

open FsUnit
open NUnit.Framework

module Calculator =
  let add x y = x + y


Calculator.add 1 2 |> should equal 1

module Tests =
  [<Test>]
  let AddShouldWork() =
    Calculator.add 1 2 |> should equal 3
    
Tests.AddShouldWork()