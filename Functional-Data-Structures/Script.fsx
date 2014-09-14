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

<<<<<<< HEAD
=======

Array.choose


// generate sequence
let s1 = seq {1..1000}
let s2 = {1..1000}
let s3 = seq {for i in 1..1000 -> i}
let s4 = Seq.initInfinite (fun i -> i * i)
s4 |> Seq.take 1000 |> Seq.nth 0


let fizzBuzz = 
  1
  |>Seq.unfold(fun i -> 
    match i%3,i%5 with 
    | 0,0 -> Some("FizzBuzz", i+1)
    | 0,_ -> Some("Fizz",i+1)
    | _,0 -> Some("Buzz",i+1)
    | _ -> Some(i.ToString(),i+1))
fizzBuzz |> Seq.take 20 |> Array.ofSeq

// https://twitter.com/fsibot/status/508661333030486016
Seq.unfold(fun i->match i%3,i%5 with 0,0->Some("F#", i+1)|0,_->Some("F",i+1)|_,0->Some("#",i+1)|_-> Some(i.ToString(),i+1)) 12
let s(v,i)= Some(v,i)
Seq.unfold(fun i->match i%3,i%5 with 0,0->s("FizzBuzz",i+1)|0,_->s("Fizz",i+1)|_,0->s("Buzz",i+1)|_->s(string(i),i+1)) 1

open System
let DaysFollowing (start:DateTime) =
    Seq.initInfinite (fun i -> start.AddDays(float(i)))

DaysFollowing (DateTime(2015,2,1)) |> Seq.take 5

[for s in "abcd" -> s]

"abcd".[0]

// generate sequence using unfold
2L 
|> Seq.unfold (fun i -> Some(i, i * i)) 
|> Seq.take 6 
|> Array.ofSeq 

// generate sequence using sequence expression with recursion
let rec seqWithRecursion (i:int64) =
    seq { 
      yield i
      yield! (seqWithRecursion (i*i))
    }

seqWithRecursion 2L |> Seq.truncate 6 |> Seq.length 

seq {1..10} |> Seq.find((=)10)
seq {1..10} |> Seq.tryFind((=)22)
seq {1..10} |> Seq.pick(fun i -> match i with 10 -> Some(i) | _ -> None)
seq {1..10} |> Seq.pick(fun i -> match i with 22 -> Some(i) | _ -> None)
seq {1..10} |> Seq.tryPick(fun i -> match i with 22 -> Some(i) | _ -> None)
seq {1..10} |> Seq.tryPick(fun i -> match i with 10 -> Some(i) | _ -> None)

let rnd = new System.Random()
Seq.initInfinite (fun _ -> rnd.Next())

let isPrime n =
  printfn "isPrime %d" n
  let rec check i = 
    printfn "check %d" i
    i > n/2 || (n%i <> 0 && check (i+1))
  check 2

Seq.initInfinite (fun _ -> rnd.Next(1, 1000))
|> Seq.take 10
|> Seq.map (isPrime)


["abc";"cef";"jk";"cef";"ab"]
|> Seq.distinct

["abc";"cef";"jk";"cef";"ab"]
|> Seq.distinctBy (fun s -> s.Length)

["abc";"cef";"jk";"cef";"ab"]
|> Seq.countBy id

{1..10}
|> Seq.pairwise

let result:seq<int*int> = [] |> Seq.pairwise
{1..2} |> Seq.windowed 3

// Seq.collect

let oneToThree i = [i;i+1;i+2]

[1;2;3;4;5]
|> Seq.collect (oneToThree)
|> Array.ofSeq



let f s = [for ch in "ACGT" -> string << Seq.length <| Seq.filter ((=) ch) s] |> String.concat " "; 
f "AGCTTTTCATTCTGACTGCAACGGG"


>>>>>>> origin/master
