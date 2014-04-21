#I @"G:\GitHub\FSharp-Snippets\kaggle\digit-recognizer\DigitRecognizer\packages\RProvider.1.0.5"
#load "RProvider.fsx"
#r "RDotNet.NativeLibrary.dll"

open System
open RDotNet
open RProvider
open RProvider.``base``
open RProvider.graphics

let data = [for x in 0. .. 0.1 .. 10. -> x * sin x ]
R.plot data

// create vector
let v = R.c({1..16}).AsVector()
