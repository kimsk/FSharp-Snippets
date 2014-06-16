(**
Any type with a generic parameter can be used as a wrapper type

ex. Option<T>, List<T>, IEnumerable<T>

something like string -> int might work, but it's not recommended.

**Computation Expressions is not Monad**

*)
type StringIntBuilder() =
  member this.Bind(m,f) =
    let b,i = System.Int32.TryParse(m)
    match b,i with
    | false,_ -> "error"
    | true,i -> f i

  member this.Return(x) =
    sprintf "%i" x

let stringint = new StringIntBuilder()

let good = 
  stringint{
    let! i = "42"
    let! j = "43"
    return i + j
  }


let bad = 
  stringint{
    let! i = "42"
    let! j = "xxx"
    return i + j
  }
(**
Rule 1: If you start with an unwrapped value
, and then you wrap it (using return)
, then unwrap it (using bind)
, you should always get back the original unwrapped value

Rule 2: If you start with a wrapped value
, and then you unwrap it (using bind), then wrap it (using return)
, you should always get back the original wrapped value

Rule 3: If you create a child workflow
, it must produce the same result as if you had "inlined" the logic in the main workflow

*)


(**
List as a wrapper type

let bind(list,f) =
  // 1) for each element in List, apply f
  // 2) f will return a list (as required by its signature)
  // 3) the result is a List of lists
*)

type ListWorkflowBuilder() =
  member this.Bind(list,f) = 
    list |> List.collect f

  member this.Return(x) =
    [x]

  member this.For(list,f) =
    this.Bind(list,f)

let listWorkflow = new ListWorkflowBuilder()

let added =
  listWorkflow{
    let! i = [1;2;3]
    printfn "%A" i
    let! j = [10;11;12]
    printfn "%A" j
    return i + j
  }

let added' =
  listWorkflow{
    for i in [1;2;3] do
    for j in [10;11;12] do
    return i+j
  }

(**
Identity workflow
*)
type IdentityBuilder() =
  member this.Bind(m,f) = f m
  member this.Return(x) = x
  member this.ReturnFrom(x) = x

let identity = new IdentityBuilder()

let result = 
  identity{
    let! x = 1
    let! y = 2
    return x + y
  }

