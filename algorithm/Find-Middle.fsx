let findMiddle list =
    let rec loop fst snd =
        let second = 
                match snd with
                | a::b::tl ->
                    tl
                | _ ->
                    []
        let first =
                match fst with
                | a::tl ->
                    tl
                | _ ->
                    []
        printfn "%A %A" first second
        match fst,second with
        | hd::tl, [] -> Some(hd)
        | [],[] -> None
        | _ -> loop first second
    loop list list


[1;2;3] |> findMiddle
[1;2;3;4;5] |> findMiddle
[1;2;3;4;5;6] |> findMiddle
[1;2;3;4;5;6;7] |> findMiddle
let out:int option = [] |> findMiddle

