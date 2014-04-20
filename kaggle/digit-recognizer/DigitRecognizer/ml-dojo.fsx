open System
open System.IO

type Record = {Label:int; Pixels: int[]}

let trainingFile = @"D:\kaggle\digit-recognizer\train.csv"
let testFile = @"D:\kaggle\digit-recognizer\test.csv"
let knnFile = @"D:\kaggle\digit-recognizer\knn_benchmark.csv"
let rfFile = @"D:\kaggle\digit-recognizer\rf_benchmark.csv"

let readTrainingFile file =    
    let rowToRecord (row:string) =
        let arr = row.Split(',') |> Array.Parallel.map Int32.Parse
        { Label = arr.[0]; Pixels = arr.[1..]}

    File.ReadAllLines(file)
        .[1..] |> Array.Parallel.map rowToRecord

let readTestFile file =
    File.ReadAllLines(file)
        .[1..] |> Array.Parallel.map (fun row -> row.Split(',') |> Array.Parallel.map Int32.Parse)

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


#time
let trainingRecords = readTrainingFile trainingFile
let testImages = readTestFile testFile
let knnLabels = readBenchmarkFile knnFile
let rfLabels = readBenchmarkFile rfFile
#time

(trainingRecords, testImages.[1]) ||> classifier getEuclideanDistance


#time
let result = runClassifier trainingRecords testImages
                |> Array.mapi (fun i l -> 
                    (l, knnLabels.[i], rfLabels.[i], l=knnLabels.[i], l=rfLabels.[i], knnLabels.[i]=rfLabels.[i])
                )
#time

let lines = result 
                |> Array.map (fun (l, knn, rf, l_knn, l_rf, knn_rf) -> 
                    sprintf "%d,%d,%d,%A,%A,%A" l knn rf l_knn l_rf knn_rf)

File.WriteAllLines(@"D:\kaggle\digit-recognizer\result.csv", lines)

trainingRecords |> Array.length