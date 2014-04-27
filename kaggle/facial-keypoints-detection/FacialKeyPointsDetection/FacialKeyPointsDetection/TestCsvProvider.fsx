#I @"..\packages\FsLab.0.0.13-beta\"
#load @"FsLab.fsx"

open FSharp.Data
open System.IO

type FaceCsv = CsvProvider<"D:/kaggle/facial-keypoints-detection/training.csv", InferRows=0, PreferOptionals = true>
let faces = new FaceCsv()

(faces.Rows |> Seq.nth 1585).left_eyebrow_outer_end_x

let lines = File.ReadAllLines("D:/kaggle/facial-keypoints-detection/training.csv")


(lines |> Seq.nth 0).Split(',') |> Seq.iteri (fun i r -> printfn "%d %s" i r)
(lines |> Seq.nth 1586).Split(',') |> Seq.iteri (fun i r -> printfn "%d %s" i r)

(lines |> Seq.nth 1586)
