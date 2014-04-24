// implement average image
// http://www.kaggle.com/c/digit-recognizer/prospector#70

#load "Util.fs"

open System
open System.IO
open Util
open Util.FileReader
open Util.Files

let trainingRecords = readTrainingFile trainingFile
let testImages = readTestFile testFile

let averagePixels (pixelsArr:float[][]) = 
    let len1 = pixelsArr.Length-1
    let len2 = pixelsArr.[0].Length-1
    [|
        for i in 0..len2 ->
            (seq {
                for j in 0..len1 ->
                    pixelsArr.[j].[i]
            } |> Seq.sum |> float)/float(len1 + 1)
                
    |]
        
// group label and average pixels to averageImages
let averageImages = trainingRecords 
                    |> Seq.groupBy (fun r -> r.Label)
                    |> Seq.map (fun (label, record) -> { Label = label; Pixels = ((record |> Seq.map (fun r -> r.Pixels) |> Array.ofSeq) |> averagePixels) }) 
                    |> Array.ofSeq
                

// Use averageImages instead of training set
(averageImages, testImages.[43]) ||> classifier getEuclideanDistance         

#time
let lines = runClassifier averageImages testImages
                |> Array.mapi (fun i l -> sprintf "%d,%d" (i+1) l)                
#time

File.WriteAllLines(@"D:\kaggle\digit-recognizer\result.csv", lines)   

// result : 0.80614 :-(    