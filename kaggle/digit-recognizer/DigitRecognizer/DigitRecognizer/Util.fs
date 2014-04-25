namespace Util

open System
open System.IO
open numl
open numl.Model
open numl.Supervised

module Files =
    let trainingFile = @"D:\kaggle\digit-recognizer\train.csv"
    let testFile = @"D:\kaggle\digit-recognizer\test.csv"
    let knnFile = @"D:\kaggle\digit-recognizer\knn_benchmark.csv"
    let rfFile = @"D:\kaggle\digit-recognizer\rf_benchmark.csv"


type Record = {Label:int; Pixels: float[]}

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
    [<EnumerableFeature(784)>] 
    member val Pixels:float[] = pixels with get
    [<Label>]
    member val Label:Digit = label with get,set

module FileReader =
    let rowToTrainingRecord (row:string) =
        let arr = row.Split(',') |> Array.Parallel.map Int32.Parse
        let i = arr.[0]
    
        let digit = 
            match i with        
            | _ when i >= 0 && i <= 9 -> enum i
            | _ -> Digit.NA

        TrainingRecord(digit, (arr.[1..] |> Array.map float))

    let rowToRecord (row:string) =
        let arr = row.Split(',') |> Array.Parallel.map Int32.Parse
        { Label = arr.[0]; Pixels = arr.[1..] |> Array.map float }

    let readTrainingFile f file =     
        File.ReadAllLines(file)
            .[1..] |> Array.Parallel.map f

    let readTestFile file =
        File.ReadAllLines(file)
            .[1..] |> Array.Parallel.map (fun row -> row.Split(',') |> Array.Parallel.map Double.Parse)

    let readBenchmarkFile file =
        File.ReadAllLines(file)
            .[1..] |> Array.Parallel.map (fun row -> row.Split(',').[1] |> Int32.Parse)


module MachineLearningUtil = 
    // Math reminder: the euclidean distance is
    // distance [ x1; y1; z1 ] [ x2; y2; z2 ] = 
    // (x1-x2)*(x1-x2) + (y1-y2)*(y1-y2) + (z1-z2)*(z1-z2) 
    let getEuclideanDistance firstPixels secondPixels = 
        Array.map2 (fun p1 p2 -> (p1 - p2) * (p1 - p2)) firstPixels secondPixels
        |> Array.sum

    // classifier picks a training sample with algorithm
    let classifier f samples pixels =        
        let record = samples |> Array.minBy (fun x -> f x.Pixels pixels)
        record.Label
  
    let runClassifier trainingRecords testImages =
        testImages 
        |> Array.Parallel.map (fun i -> (trainingRecords, i) ||> classifier getEuclideanDistance)

    let predictTrainingRecord (model:IModel) pixels =
        let record = TrainingRecord(Digit.NA, pixels)
        model.Predict<TrainingRecord>(record).Label