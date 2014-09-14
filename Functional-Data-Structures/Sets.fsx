open System

let rnd = new Random()

let num = seq { for i in 1..100 -> rnd.Next(1,5) }
let num' = seq { for i in 1..100 -> rnd.Next(1,120) }
let num'' = seq { for i in 1..100 -> rnd.Next(20,120) }


num |> Seq.length

let set = Set(num)
set |> Set.count 

let set' = set.Add(20)
set' |> Set.count

set' - set

num' |> Set.ofSeq |> (+) set |> Seq.length

[[1..10];[5..15];[10..30]] |> List.reduce (@) |> Set.ofList |> Set.count

set |> Set.add 34