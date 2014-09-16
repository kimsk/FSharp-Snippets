
let usCoins = [50;25;10;5;1]

let listWaysToMakeChange' (denom:int list) (cents:int) =
  let rec loop acc avCoins:list<int> =
    match avCoins with 
    | [] -> snd(acc)
    | hd::tl as l ->
        printfn "%d::%A %A" hd tl acc
        let remaining, coins = acc
        if hd>remaining then
          loop acc tl
        else
          loop (remaining-hd,hd::coins) l

  denom
  |> List.map (fun c -> loop (cents,[]) [c])

listWaysToMakeChange' usCoins 10


let rec listWayToMakeChange (denom:int list) (cents:int):int list =
  match denom with
  | [] -> []
  | hd::tl as l ->
      //printfn "%d::%A %A" hd tl cents
      if hd>cents then
        (listWayToMakeChange tl cents)
      else
        hd::(listWayToMakeChange l (cents - hd))

let listWaysToMakeChange (denom:int list) (cents:int):int list list =
  let listMultipleWaysToMakeChange (l:int list) c =
    [
      let hd::tl = l
      if tl.Length > 0 then
        let times = c/hd
        for i in 1..times ->
          let pre = List.init i (fun _ -> hd)
          pre@(listWayToMakeChange tl (c-(hd*i)))
    ]
  
  let rec loop l =
    match l with
    | [] -> []
    | hd::tl as l ->
      [       
        yield (listWayToMakeChange l cents)

        yield! (listMultipleWaysToMakeChange l cents)

        yield! loop tl
      ]
  loop denom  
  |> Seq.distinct |> Seq.sort |> List.ofSeq

listWaysToMakeChange [10;5;1] 10

listWaysToMakeChange [10;5;1] 27

listWaysToMakeChange [5;1] 27


listWaysToMakeChange [25;5;1] 27


listWaysToMakeChange [50;25;10;5;1] 100


listWayToMakeChange [5;1] 25
[1;2]::[2;3]::[]

  
