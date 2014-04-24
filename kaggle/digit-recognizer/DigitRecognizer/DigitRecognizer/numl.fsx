#r @"..\packages\numl.0.7.6\lib\net40\numl.dll"
#load "Util.fs"

open System
open System.IO
open Util
open Util.FileReader
open Util.Files
open numl
open numl.Model
open numl.Supervised.DecisionTree

let trainingRecords = readTrainingFile trainingFile |> Seq.take 20
let testImages = readTestFile testFile

let description = Descriptor.New(typeof<Record>)
                        .With("Pixels").AsEnumerable(784)
                        .Learn("Label").As(typeof<int>)
description.Features                         
                        
let generator = new DecisionTreeGenerator(description)
let model = generator.Generate(trainingRecords)
printfn "%A" model



