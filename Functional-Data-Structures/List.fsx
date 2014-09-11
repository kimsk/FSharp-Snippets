open System

[1..6] 
|> List.fold (fun acc _ -> Console.ReadLine()::acc) []

// No List.unfold, so we can't do recursive sequence expression

let rec SafeFileHierarchy startDir = 
  let TryEnumerateFiles dir =
    try 
      System.IO.Directory.EnumerateDirectories(startDir)
    with _ -> Seq.empty

  let TryEnumerateDirs dir = 
    try 
      System.IO.Directory.EnumerateDirectories(startDir)
    with _ -> Seq.empty

//  [
//    yield! TryEnumerateFiles startDir
//    for dir in TryEnumerateDirs startDir do
//      yield! (SafeFileHierarchy dir)
//  ]

  seq {
    yield! TryEnumerateFiles startDir
    for dir in TryEnumerateDirs startDir do
      yield! (SafeFileHierarchy dir)
  }


#time
SafeFileHierarchy "c:\\output" |> List.ofSeq


type MutableInt = { mutable Value: int}

let l = [ for i in 1..10 -> {Value=i}]
let m = [ for i in 1..10 -> {Value=(-i)}]

let n = l@m
n.[0].Value <- 0
l.[0] = n.[0]
