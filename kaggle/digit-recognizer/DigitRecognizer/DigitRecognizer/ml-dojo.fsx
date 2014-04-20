#load "Util.fs"

open System
open System.IO
open Util
open Util.FileReader

let trainingFile = @"D:\kaggle\digit-recognizer\train.csv"
let testFile = @"D:\kaggle\digit-recognizer\test.csv"
let knnFile = @"D:\kaggle\digit-recognizer\knn_benchmark.csv"
let rfFile = @"D:\kaggle\digit-recognizer\rf_benchmark.csv"

#time
let trainingRecords = readTrainingFile trainingFile
let testImages = readTestFile testFile
let knnLabels = readBenchmarkFile knnFile
let rfLabels = readBenchmarkFile rfFile
#time

(trainingRecords, testImages.[1]) ||> classifier getEuclideanDistance


#time
let result = runClassifier trainingRecords testImages.[..10]
                |> Array.mapi (fun i l -> 
                    (l, knnLabels.[i], rfLabels.[i], l=knnLabels.[i], l=rfLabels.[i], knnLabels.[i]=rfLabels.[i])
                )
#time

let lines = result 
                |> Array.map (fun (l, knn, rf, l_knn, l_rf, knn_rf) -> 
                    sprintf "%d,%d,%d,%A,%A,%A" l knn rf l_knn l_rf knn_rf)

File.WriteAllLines(@"D:\kaggle\digit-recognizer\result.csv", lines)

trainingRecords |> Array.length

// result : 0.97114