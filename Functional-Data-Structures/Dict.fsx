// mutable style
open System.Collections.Generic
let dict1 = new Dictionary<string, string>()
dict1.Add("A","1")
dict1.["A"] <- "10"


// immutable style
let dict' = [ "A",1;"B",2;"C",3;"C",4] |> dict // undocumented, 

dict'.["A"] // <- 10
dict'.Add("D",4) // NotSupportedException

dict'.["C"]


open System.Collections.Concurrent

let conDict = new ConcurrentDictionary<string, int>()
