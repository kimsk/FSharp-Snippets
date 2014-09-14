let b:int[] = Array.zeroCreate 20

5
|> function
   | 1 -> Some(1)
   | _ -> None


5 
|> fun i ->
        match i with
        | 1 -> Some(1)
        | 5 -> Some(5)
        | _ -> None

(fun i ->
        match i with
        | 1 -> Some(1)
        | 5 -> Some(5)
        | _ -> None) 5


[1;2;3;4]
|> List.reduce (fun acc v -> acc + v)

// https://twitter.com/fsibot/status/508465436346691584
[|for s in "♣♦♥♠" do 
    for r in "A23456789TJQK" -> 
        System.String[|r;s|]|] 
    |> Array.sortBy (fun _ -> System.Random().NextDouble())

