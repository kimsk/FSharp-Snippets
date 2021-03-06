﻿#I @"..\packages\FsLab.0.0.13-beta\"
#load @"FsLab.fsx"

open Deedle
open Deedle.RPlugin
open RProviderConverters
open System
open System.IO


type Face =
    {
        LeftEyeCenter:Option<decimal*decimal>
        RightEyeCenter:Option<decimal*decimal>
        LeftEyeInnerCorner:Option<decimal*decimal>
        LeftEyeOuterCorner:Option<decimal*decimal>
        RightEyeInnerCorner:Option<decimal*decimal>
        RightEyeOuterCorner:Option<decimal*decimal>
        LeftEyeBrowInnerEnd:Option<decimal*decimal>
        LeftEyeBrowOuterEnd:Option<decimal*decimal>
        RightEyeBrowInnerEnd:Option<decimal*decimal>
        RighttEyeBrowOuterEnd:Option<decimal*decimal>
        NoseTip:Option<decimal*decimal>
        MouthLeftCorner:Option<decimal*decimal>
        MouthRightCorner:Option<decimal*decimal>
        MouthCenterTopLip:Option<decimal*decimal>
        MouthCenterBottomLip:Option<decimal*decimal>
        Image:int[]
    }

let lines = File.ReadAllLines("D:/kaggle/facial-keypoints-detection/training.csv")

let faces =      
    let d = Decimal.TryParse
    let toXy v1 v2 =         
        match d v1 with
        | false, _ -> None
        | true, x ->
            match d v2 with
            | false, _ -> None
            | true,y -> Some(x,y)        

    lines
    |> Seq.skip 1
    |> Seq.map (fun (row:string) -> 
            let r = row.Split(',')
            {
               LeftEyeCenter = toXy r.[0] r.[1]
               RightEyeCenter = toXy r.[2] r.[3]
               LeftEyeInnerCorner = toXy r.[4] r.[5]
               LeftEyeOuterCorner = toXy r.[6] r.[7]
               RightEyeInnerCorner = toXy r.[8] r.[9]
               RightEyeOuterCorner = toXy r.[10] r.[11]
               LeftEyeBrowInnerEnd = toXy r.[12] r.[13]
               LeftEyeBrowOuterEnd = toXy r.[14] r.[15]
               RightEyeBrowInnerEnd = toXy r.[16] r.[17]
               RighttEyeBrowOuterEnd = toXy r.[18] r.[19]
               NoseTip = toXy r.[20] r.[21]
               MouthLeftCorner = toXy r.[22] r.[23]
               MouthRightCorner = toXy r.[24] r.[25]
               MouthCenterTopLip = toXy r.[26] r.[27]
               MouthCenterBottomLip = toXy r.[28] r.[29]
               Image = (r.[30].Split(' ') |> Array.map (Int32.Parse))
            }
        )

faces |> Seq.skip 210 |> Seq.iteri (fun i r -> printfn "%d %A" i r)
((lines
|> Seq.nth 211).Split(',')
|> Seq.nth 30).Split(' ') |> Array.iteri (fun i r -> printfn "%d %d" i (Int32.Parse(r)))


// R Stuff
open RProvider
open RProvider.``base``
open RProvider.graphics
open RProvider.grDevices
open RDotNet

// helper functions
let o x = x :> obj
let imXy p = match p with Some(x,y) -> (96m-x),(96m-y) | _ -> (0m,0m)     
let face i = faces |> Seq.nth i
let imPointParams (x,y) color = ["x",o x;"y",o y;"col",o color] |> namedParams

// Visualize image using R
let gray = R.gray(["level",R.c(seq { for i in 0. .. 255. -> i/255.})]|>namedParams)
let grayScaleImage nrow ncol data =
    [ 
        "x", R.c([|1..nrow|])
        "y", R.c([|1..ncol|])
        "z", R.matrix(nrow=nrow,ncol=ncol,data=data)
        "col", gray
    ] |> namedParams |> R.image

// helper function to slice matrix
let sliceMatrix (m:SymbolicExpression) (x1,x2,y1,y2) =
    m.Engine.SetSymbol("m", m)
    sprintf "m[%M:%M,%M:%M]" x1 x2 y1 y2 |> m.Engine.Evaluate

// image(1:96, 1:96, im, col=gray((0:255)/255))
Array.rev((face 0).Image) |> grayScaleImage 96 96

// add points for nose tip, left eye, right eye
// points(96-d.train$nose_tip_x[1], 96-d.train$nose_tip_y[1], col="red")
let noseTipXy = imXy (face 0).NoseTip
let leftEyeCenterXy = imXy (face 0).LeftEyeCenter
let rightEyeCenterXy = imXy (face 0).RightEyeCenter
R.points(imPointParams noseTipXy "red")
R.points(imPointParams leftEyeCenterXy "blue")
R.points(imPointParams rightEyeCenterXy "green")

// add all nose points
seq { for f in faces -> f } |> Seq.iter (fun f -> R.points(imPointParams (imXy f.NoseTip) "red") |> ignore)

// Get R Data Frame
let facesR_Df =    
    let d x =
        match Double.TryParse x with
        | true, v -> v
        | _ -> Double.NaN

    let lines = lines |> Seq.map(fun (r:string) -> r.Split(','))
    lines 
    |> Seq.nth 0 
    |> Seq.mapi (fun i h -> h, lines |> Seq.skip 1 |> Seq.map (fun r -> d r.[i]))
    |> (namedParams >> R.data_frame)
 
R.colMeans(facesR_Df, na_rm=true)

let v = R.c([|1..400|])
let m = R.matrix(data=v, nrow=20,ncol=20)
R.colMeans(m)


#time
let facesDeedleDf =
    let d x =
        match Double.TryParse x with
        | true, v -> v
        | _ -> Double.NaN

    let lines = lines |> Seq.map(fun (r:string) -> r.Split(','))
    lines 
    |> Seq.nth 0 
    |> Seq.mapi (fun i h -> h, lines |> Seq.skip 1 |> Seq.map (fun r -> d r.[i]) |> Series.ofValues)
    |> Frame.ofColumns
#time
// deedle data frame can be used with R!
let dv:float[] = R.colMeans(facesDeedleDf, na_rm=true).GetValue()
facesDeedleDf?left_eye_center_x
facesDeedleDf.Columns
facesDeedleDf.Rows
facesDeedleDf.Rows.[10]


let patchSize = 10m
// too slow...
let patches = 
    faces 
    |> Seq.filter (fun f -> f.LeftEyeCenter.IsSome) 
    |> Seq.map (fun f -> f.LeftEyeCenter.Value)
    |> Seq.mapi (fun i (x,y) -> 
            let im = R.matrix(nrow=96,ncol=96,data=(face i).Image)
            let x1,x2,y1,y2 = (x-patchSize),(x+patchSize),(y-patchSize),(y+patchSize)
            if x1>=1m && x2<=96m && y1>=1m && y2 <=96m then
                let newMatrix = sliceMatrix im (x1,x2,y1,y2)
                Some(newMatrix)
            else
                None                
        )
    |> Seq.filter (fun p -> p.IsSome)
    |> Seq.map (fun p -> p.Value)

// show patch of eye
R.rev(x=(patches |> Seq.nth 2 |> R.as_vector)) |> grayScaleImage 21 21

// calculate means
#time
let p10 = patches |> Seq.take 1000 |> Seq.map (R.as_vector) |> Seq.collect (fun p -> p.AsNumeric() |> Array.ofSeq)
let m10 = R.matrix(data=p10,nrow=1000,ncol=441)
#time
let means = R.colMeans(m10)
    
R.rev(means) |> grayScaleImage 21 21 

open MathNet.Numerics.LinearAlgebra
open MathNet.Numerics.LinearAlgebra.Double

let slice r1 r2 r3 r4 (m:Matrix<float>) = 
    m.GetSlice(Some(r1), Some(r2), Some(r3), Some(r4))

let slicedMatrices = 
    faces 
    |> Seq.filter (fun f -> f.LeftEyeCenter.IsSome) 
    |> Seq.choose (fun f -> 
            let x, y = f.LeftEyeCenter.Value
            let x1,x2,y1,y2 = int(x-patchSize), int(x+patchSize), int(y-patchSize), int(y+patchSize)
            if x1>=1 && x2<=96 && y1>=1 && y2 <=96 then
                (DenseMatrix(96, 96, f.Image |> Array.map(float))).SubMatrix(x1,(x2-x1+1),y1,y2-y1+1)
                |> Some
            else
                None              
        ) 
    |> Seq.cache

slicedMatrices |> Seq.length

#time
let means2 = 
    let len = slicedMatrices |> Seq.length

    (slicedMatrices
    |> Array.ofSeq     
    |> Array.reduce (fun acc m -> acc + m))/(float len)
    
#time

let meansVc = R.c([0..20] |> Seq.collect (fun i -> means2.Column(i)) |> Array.ofSeq |> Array.rev)
meansVc |> grayScaleImage 21 21

let vc = R.c([0..20] |> Seq.collect (fun i -> (slicedMatrices |> Seq.nth 7032).Column(i)) |> Array.ofSeq |> Array.rev)
vc |> grayScaleImage 21 21
