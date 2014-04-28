#I @"..\packages\FsLab.0.0.13-beta\"
#load @"FsLab.fsx"

open RProvider
open RProvider.``base``
open Deedle
open Deedle.RPlugin
open RProviderConverters
open RProvider.datasets


// R vector and R matrix
let v = R.c({1..16})
let m = R.matrix(data = v, nrow = 4, ncol = 4)
let v1 = R.c(namedParams ["A",1;"B",2;"C",3])
let v2 = R.c(namedParams ["A",11;"B",22;"C",33])
let v3 = R.c(namedParams ["A",10;"B",20;"D",30])

// R data frame
let rdf = ["One",[|1;2;3|];"Two",[|2;3;4|];"Three",[|3;4;5|]] |> (namedParams >> R.data_frame)
let rdf2 = ["One",v1;"Two",v2;"Three",v3] |> (namedParams >> R.data_frame)
let mtcars = R.mtcars

// From R data frame to deedle data frame
let ddf:Frame<int,string> = rdf.GetValue()
let ddf2:Frame<string,string> = rdf2.GetValue()
let dmtcars:Frame<string,string> = mtcars.GetValue()

// using R functions with Deedle data frame and then convert to F# float[]
let means = R.colMeans(ddf2)
let dv2:float[] = means.GetValue()