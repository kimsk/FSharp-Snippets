#r @"G:\GitHub\FSharp-Snippets\deedle\packages\FSharp.Data.1.1.10\lib\net40\FSharp.Data.dll"
#I @"G:\GitHub\FSharp-Snippets\r-provider\TryRProvider\packages\RProvider.1.0.5\lib"
#r "RDotNet.dll"
#r "RDotNet.FSharp.dll"
#r "RProvider.dll"

open System
open RDotNet
open RProvider
open RProvider.``base``
open RProvider.graphics
open FSharp.Data

// create vector
let v = R.c({1..16}).AsVector()
v.[1]

let data = [for i in 0. .. 0.1 .. 10. -> i * sin i ]
R.plot data