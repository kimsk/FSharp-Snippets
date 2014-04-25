#r @"..\packages\numl.0.7.6\lib\net40\numl.dll"
#load "Util.fs"

open System
open System.IO
open Util
open Util.FileReader
open Util.Files
open numl
open numl.Model
open numl.Supervised
open numl.Supervised.NaiveBayes
open Util.MachineLearningUtil

let trainingRecords = trainingFile |> readTrainingFile rowToTrainingRecord |> Seq.cast<obj>

#time
let nbModel = 
    let d = Descriptor.Create<TrainingRecord>()
    let g = NaiveBayesGenerator(2)
    g.Generate(d, trainingRecords)
#time

let testImages = readTestFile Files.testFile
    
#time
[|
    testImages.[0] 
    testImages.[1] 
    testImages.[2] 
    testImages.[3] 
    testImages.[4] 
    testImages.[5] 
    testImages.[6] 
|] |> Array.Parallel.map (predictTrainingRecord nbModel) 
#time

#time
let lines = testImages |> Array.mapi (fun i r -> sprintf "%d,%d" (i+1) (r |> predict nbModel |> int)) 
#time

File.WriteAllLines(@"D:\kaggle\digit-recognizer\naivebayes-result.csv", lines)

// 0.83514