#r @"..\packages\FsCheck.0.9.4.0\lib\net40-Client\FsCheck.dll"
#r @"..\packages\NUnit.2.6.3\lib\nunit.framework.dll"
#r @"..\packages\FsUnit.1.3.0.1\Lib\Net40\FsUnit.NUnit.dll"
open FsUnit
open FsCheck

Check.QuickThrowOnFailure(fun (x:string) -> 
    x <> null ==> lazy
        x.Length
        |> should greaterThan -1
)

Check.QuickThrowOnFailure(fun a b c-> 
    ((a * b) * c )|> should equal (a * (b * c))
)