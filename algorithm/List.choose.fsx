let evenSquare n = 
  match n%2 with
  | 0 -> Some(n*n)
  | _ -> None

let choose f l =
  let rec choose' acc l =
    match l with
    | [] -> acc |> List.rev
    | hd::tl ->
        match f(hd) with
        | Some(n) ->
            choose' (n::acc) tl
        | None ->
            choose' acc tl
  choose' [] l