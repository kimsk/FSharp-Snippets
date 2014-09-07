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


[|for s in "♣♦♥♠" do 
    for r in "A23456789TJQK" -> 
        System.String[|r;s|]|] 
    |> Array.sortBy (fun _ -> System.Random().NextDouble())

let s1 = seq {1..1000}
let s2 = {1..1000}
let s3 = seq {for i in 1..1000 -> i}
let s4 = Seq.initInfinite (fun i -> i * i)
s4 |> Seq.take 1000 |> Seq.nth 0


2L |> Seq.unfold (fun i -> Some(i, i * i)) |> Seq.take 6 |> Array.ofSeq 

let fizzBuzz = 1|>Seq.unfold(fun i -> match i with i when i%3=0&&i%5=0->Some("FizzBuzz",i+1)|i when i%3=0->Some("Fizz",i+1)|i when i%5=0->Some("Buzz",i+1)|_->Some(i.ToString(),i+1))
fizzBuzz |> Seq.take 20 |> Array.ofSeq

open System
let DaysFollowing (start:DateTime) =
    Seq.initInfinite (fun i -> start.AddDays(float(i)))

DaysFollowing (DateTime(2015,2,1)) |> Seq.take 5

[for s in "abcd" -> s]

"abcd".[0]

let rec seqWithRecursion (i:int64) =
    seq { yield i;yield! (seqWithRecursion (i*i))}

seqWithRecursion 2L |> Seq.truncate 6 |> Seq.length 