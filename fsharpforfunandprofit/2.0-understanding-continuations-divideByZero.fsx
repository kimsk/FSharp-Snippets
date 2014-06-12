let divideBy bottom top =
  if bottom = 0
  then None
  else Some(top/bottom)

let pipeInto (someExpression, lambda) = 
  match someExpression with
  | None -> None
  | Some x -> x |> lambda

let return' c = Some c

let divideByWorkflow x y w z = 
  pipeInto (x |> divideBy y, fun a ->
  pipeInto (a |> divideBy w, fun b ->
  pipeInto (b |> divideBy z, fun c ->
  return' c
  )))

(**
maybe 
    {
    let! a = x |> divideBy y
    let! b = a |> divideBy w
    let! c = b |> divideBy z
    return c
    }    
*)

let good = divideByWorkflow 12 3 2 1
let bad = divideByWorkflow 12 3 0 1


(**
Chaining workflow
*)
let divideByWorkflow' x y w z = 
    let a = x |> divideBy y 
    match a with
    | None -> None  // give up
    | Some a' ->    // keep going
        let b = a' |> divideBy w
        match b with
        | None -> None  // give up
        | Some b' ->    // keep going
            let c = b' |> divideBy z
            match c with
            | None -> None  // give up
            | Some c' ->    // keep going
                //return 
                Some c' 

let good' = divideByWorkflow' 12 3 2 1
let bad' = divideByWorkflow' 12 3 0 1