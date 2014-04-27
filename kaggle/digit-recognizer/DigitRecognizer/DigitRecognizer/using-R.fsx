#r @"..\packages\numl.0.7.6\lib\net40\numl.dll"
#load "Util.fs"

open System
open System.IO
open Util
open Util.FileReader
open Util.Files
open Util.MachineLearningUtil

let trainingRecords = readTrainingFile rowToRecord trainingFile

#I @"..\packages\FsLab.0.0.13-beta\"
#load @"FsLab.fsx"

open RProvider
open RProvider.``base``
open RProvider.graphics
open RProvider.grDevices

let im = R.matrix(nrow=28,ncol=28,data=(trainingRecords.[13135].Pixels))
let imageParams = 
    [
        "x",R.c([|1..28|])
        "y",R.c([|1..28|])
        "z",im
        "col",R.gray(["level",R.c(seq { for i in 0. .. 255. -> i/255.})]|>namedParams)
    ] |> namedParams
R.image(imageParams)