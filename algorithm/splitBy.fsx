let input = [2;12;4;5;8;1;0;14;10;15]

let splitBy v list =
  let yieldRevNonEmpty list = 
    if list = [] then []
    else [List.rev list]
  
  let rec loop groupSoFar list = seq { 
    match list with
    | [] -> yield! yieldRevNonEmpty groupSoFar
    | head::tail when head = v ->
        yield! yieldRevNonEmpty groupSoFar
        yield! loop [] tail
    | head::tail ->
        yield! loop (head::groupSoFar) tail }
  loop [] list |> List.ofSeq

input |> splitBy 1



