#I @"..\packages\FsLab.0.0.13-beta\"
#load @"FsLab.fsx"

// R Stuff
//open RProvider
//open RProvider.``base``
//open RProvider.graphics
open Deedle
open System
open System.IO

type Record = 
    {
        left_eye_center_x:decimal; left_eye_center_y:decimal; right_eye_center_x:decimal; right_eye_center_y:decimal; left_eye_inner_corner_x:decimal; left_eye_inner_corner_y:decimal; left_eye_outer_corner_x:decimal; left_eye_outer_corner_y:decimal; right_eye_inner_corner_x:decimal; right_eye_inner_corner_y:decimal; right_eye_outer_corner_x:decimal; right_eye_outer_corner_y:decimal; left_eyebrow_inner_end_x:decimal; left_eyebrow_inner_end_y:decimal; left_eyebrow_outer_end_x:decimal; left_eyebrow_outer_end_y:decimal; right_eyebrow_inner_end_x:decimal; right_eyebrow_inner_end_y:decimal; right_eyebrow_outer_end_x:decimal; right_eyebrow_outer_end_y:decimal; nose_tip_x:decimal; nose_tip_y:decimal; mouth_left_corner_x:decimal; mouth_left_corner_y:decimal; mouth_right_corner_x:decimal; mouth_right_corner_y:decimal; mouth_center_top_lip_x:decimal; mouth_center_top_lip_y:decimal; mouth_center_bottom_lip_x:decimal; mouth_center_bottom_lip_y:decimal; 
        Image:int[]
    }

let records = 
    let d = Decimal.Parse
    File.ReadAllLines("D:/kaggle/facial-keypoints-detection/training.csv")
    |> Seq.skip 1
    |> Seq.map (fun (row:string) -> 
            let r = row.Split(',')
            {
                left_eye_center_x=(d r.[0]); left_eye_center_y=(d r.[1]); 
                right_eye_center_x=(d r.[2]); right_eye_center_y=(d r.[3]); 
                left_eye_inner_corner_x=(d r.[4]); left_eye_inner_corner_y=(d r.[5]); 
                left_eye_outer_corner_x=(d r.[6]); left_eye_outer_corner_y=(d r.[7]); 
                right_eye_inner_corner_x=(d r.[8]); right_eye_inner_corner_y=(d r.[9]); 
                right_eye_outer_corner_x=(d r.[10]); right_eye_outer_corner_y=(d r.[11]); 
                left_eyebrow_inner_end_x=(d r.[12]); left_eyebrow_inner_end_y=(d r.[13]); 
                left_eyebrow_outer_end_x=(d r.[14]); left_eyebrow_outer_end_y=(d r.[15]); 
                right_eyebrow_inner_end_x=(d r.[16]); right_eyebrow_inner_end_y=(d r.[17]); 
                right_eyebrow_outer_end_x=(d r.[18]); right_eyebrow_outer_end_y=(d r.[19]); 
                nose_tip_x=(d r.[20]); nose_tip_y=(d r.[21]); 
                mouth_left_corner_x=(d r.[22]); mouth_left_corner_y=(d r.[23]); mouth_right_corner_x=(d r.[24]); mouth_right_corner_y=(d r.[25]); 
                mouth_center_top_lip_x=(d r.[26]); mouth_center_top_lip_y=(d r.[27]); mouth_center_bottom_lip_x=(d r.[28]); mouth_center_bottom_lip_y=(d r.[29]); 
                Image = (r.[30].Split(' ') |> Array.map (Int32.Parse))
            }
        ) 
    //|> Array.ofSeq

records |> Seq.iteri (fun i r -> printfn "%d %A" i r)
((File.ReadAllLines("D:/kaggle/facial-keypoints-detection/training.csv") 
|> Seq.nth 209).Split(',')
|> Seq.nth 30).Split(' ') |> Array.iteri (fun i r -> printfn "%d %d" i (Int32.Parse(r)))


