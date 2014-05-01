#I @"..\packages\FsLab.0.0.13-beta\"
#load @"FsLab.fsx"

// R Stuff
open RProvider
open RProvider.``base``
open RProvider.graphics

let v = R.c({1..16})
let m = R.matrix(data = v, nrow = 4, ncol = 4)

// create identity matrix
let i = R.diag 4

let data = [for i in 0. .. 0.1 .. 10. -> i * sin i ]
R.plot data

// REngine
let c = i.Engine.Evaluate("c(1,2,3)")
let c2 = R.eval(R.parse(text="c(1,2,3)"))

let arr = ["One",R.c([|1..10|]);"Two",R.c([|11..20|])]
let df = R.data_frame(namedParams arr)

