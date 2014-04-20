namespace Util

open System
open System.IO

type Record = {Label:int; Pixels: float[]}


module FileReader =
    let readTrainingFile file =     
        let rowToRecord (row:string) =
            let arr = row.Split(',') |> Array.Parallel.map Int32.Parse
            { Label = arr.[0]; Pixels = arr.[1..] |> Array.map float }

        File.ReadAllLines(file)
            .[1..] |> Array.Parallel.map rowToRecord

    let readTestFile file =
        File.ReadAllLines(file)
            .[1..] |> Array.Parallel.map (fun row -> row.Split(',') |> Array.Parallel.map Double.Parse)

    let readBenchmarkFile file =
        File.ReadAllLines(file)
            .[1..] |> Array.Parallel.map (fun row -> row.Split(',').[1] |> Int32.Parse)


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