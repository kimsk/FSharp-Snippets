let input = [2;12;4;5;8;1;0;14;10;15]

let rec quicksort l =
  match l with
  | [] -> []
  | hd::tail ->
      let smallers, biggers = tail |> List.partition ((>=)hd)
      quicksort smallers @ [hd] @ quicksort biggers

let rec splitBy acc f l =  
  match l with
  | [] -> acc
  | hd::tl ->
      if f(hd) then
        splitBy (hd::fst(acc), snd(acc)) f tl
      else
        splitBy (fst(acc), hd::snd(acc)) f tl

splitBy ([],[]) (fun i -> i <= input.Head) input.Tail

let rec quicksort' l =
  match l with
  | [] -> []
  | hd::tail ->
      let smallers, biggers = splitBy ([],[]) (fun i -> i <= hd) tail
      quicksort smallers @ [hd] @ quicksort biggers


input |> quicksort
input |> quicksort'

