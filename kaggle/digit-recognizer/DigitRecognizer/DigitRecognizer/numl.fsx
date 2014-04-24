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

type Digit = 
    | NA = -1
    | Zero = 0
    | One = 1
    | Two = 2
    | Three = 3
    | Four = 4
    | Five = 5
    | Six = 6
    | Seven = 7
    | Eight = 8
    | Nine = 9

type TrainingRecord(label:Digit, pixels:float[]) =    
    [<EnumerableFeatureAttribute(784)>] 
    member val Pixels:float[] = pixels with get
    [<Label>]
    member val Label:Digit = label with get,set

let rowToTrainingRecord (row:string) =
    let arr = row.Split(',') |> Array.Parallel.map Int32.Parse
    let i = arr.[0]
    let digit = 
        match i with        
        | _ when i <= 0 -> Digit.Zero
        | _ when i <= 1 -> Digit.One
        | _ when i <= 2 -> Digit.Two
        | _ when i <= 3 -> Digit.Three
        | _ when i <= 4 -> Digit.Four
        | _ when i <= 5 -> Digit.Five
        | _ when i <= 6 -> Digit.Six
        | _ when i <= 7 -> Digit.Seven
        | _ when i <= 8 -> Digit.Eight
        | _ when i <= 9 -> Digit.Nine
        | _ -> Digit.NA

    TrainingRecord(digit, (arr.[1..] |> Array.map float))



let trainingRecords = 
    File.ReadAllLines(@"C:\kaggle\digit-recognizer\train.csv").[1..] 
    |> Array.Parallel.map rowToTrainingRecord 
    |> Seq.cast<obj>
    //|> Seq.take 1000

let d = Descriptor.Create<TrainingRecord>()

let getNaiveBayesModel description (data:seq<obj>) =
    let g = NaiveBayesGenerator(2)
    g.Generate(description, data)


let predict (model:IModel) record =
    model.Predict<TrainingRecord>(record).Label

#time
let nbModel = getNaiveBayesModel d trainingRecords
#time

let testImages = 
    File.ReadAllLines(@"C:\kaggle\digit-recognizer\test.csv").[1..] 
    |> Array.Parallel.map (fun row -> 
            TrainingRecord(Digit.NA, (row.Split(',') |> Array.Parallel.map Double.Parse))            
        )
    
#time
[|
    testImages.[0] 
    testImages.[1] 
    testImages.[2] 
    testImages.[3] 
    testImages.[4] 
    testImages.[5] 
    testImages.[6] 
|] |> Array.Parallel.map (fun r -> predict nbModel r) 
#time