#I "../packages/FSharp.Charting.0.90.6"
#I "../packages/Deedle.0.9.12"
#load "FSharp.Charting.fsx"
#load "Deedle.fsx"

open System
open Deedle
open FSharp.Charting

let trainDf = 
    Frame.ReadCsv(@"D:\kaggle\titanic\train.csv") 
    |> Frame.indexRowsInt "PassengerId" 
    |> Frame.orderRows

trainDf.Rows.[2].GetAs<string>("Cabin")

let testDf = 
    Frame.ReadCsv(@"D:\kaggle\titanic\test.csv") 
    |> Frame.indexRowsInt "PassengerId" 
    |> Frame.orderRows

let grouped = trainDf |> Frame.groupRowsByString "Sex" |> Frame.nest

// For each group, calculate the total number of survived & died
let bySex =
  grouped
  |> Series.map (fun sex df -> 
      // Group by the 'Survived' column & count by Boolean values
      df.GetSeries<bool>("Survived") |> Series.groupBy (fun k v -> v) 
      |> Frame.ofColumns |> Frame.countValues )
  |> Frame.ofRows
  |> Frame.indexColsWith ["Died"; "Survived"]

bySex?Total <- Frame.countRows $ grouped

["Died (%)" => bySex?Died/bySex?Total * 100.0]
