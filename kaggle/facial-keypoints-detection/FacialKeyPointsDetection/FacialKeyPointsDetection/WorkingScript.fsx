#I @"..\packages\FsLab.0.0.13-beta\"
#load @"FsLab.fsx"

let _training_ = "d:/kaggle/facial-keypoints-detection/training.csv"
let _test_ = "d:/kaggle/facial-keypoints-detection/test.csv"

open Deedle
open Deedle.RPlugin
open RProviderConverters
open System
open System.IO

// R Stuff
open RProvider
open RProvider.``base``
open RProvider.graphics
open RProvider.grDevices
open RDotNet

open MathNet.Numerics
open MathNet.Numerics.LinearAlgebra
open MathNet.Numerics.LinearAlgebra.Double

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

let toFaces lines =      
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

let lines = File.ReadAllLines(_training_)
let faces = lines |> toFaces

let face i = faces |> Seq.nth i
let o x = x :> obj
let imXy p = match p with Some(x,y) -> (96m-x),(96m-y) | _ -> (0m,0m)     
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

let imToArray2D (im:int[]) = Array2D.init 96 96 (fun i j -> im.[(i*96) + j] |> float)

let im0 = (face 0).Image |> imToArray2D
let im0m:Matrix<float> = DenseMatrix.ofArray2  im0
R.c(im0m.EnumerateRows() |> Seq.collect id |> Array.ofSeq |> Array.rev ) |> grayScaleImage 96 96


let patchSize = 10m

// show average left eye
let leftEyeCenterSlicedMatrices = 
    faces 
    |> Seq.filter (fun f -> f.LeftEyeCenter.IsSome) 
    |> Seq.choose (fun f -> 
            let x, y = f.LeftEyeCenter.Value
            let x1,x2,y1,y2 = int(x-patchSize), int(x+patchSize), int(y-patchSize), int(y+patchSize)
            if x1>=1 && x2<=96 && y1>=1 && y2 <=96 then
                (f.Image |> imToArray2D).[y1..y2,x1..x2] 
                |> DenseMatrix.ofArray2 
                |> Some
            else
                None              
        ) 
    |> Seq.cache

let firstLeftEye:Matrix<float> = (leftEyeCenterSlicedMatrices |> Seq.nth 0)
R.c(firstLeftEye.EnumerateRows() |> Seq.collect id |> Array.ofSeq |> Array.rev )
|> grayScaleImage 21 21

#time
let leftEyeMeans = 
    let len = leftEyeCenterSlicedMatrices |> Seq.length

    (leftEyeCenterSlicedMatrices
    |> Array.ofSeq     
    |> Array.reduce (fun acc m -> acc + m))/(float len)
    
#time

R.c(leftEyeMeans.EnumerateRows() |> Seq.collect id |> Array.ofSeq |> Array.rev)
|> grayScaleImage 21 21


// show average right eye
let rightEyeCenterSlicedMatrices = 
    faces 
    |> Seq.filter (fun f -> f.RightEyeCenter.IsSome) 
    |> Seq.choose (fun f -> 
            let x, y = f.RightEyeCenter.Value
            let x1,x2,y1,y2 = int(x-patchSize), int(x+patchSize), int(y-patchSize), int(y+patchSize)
            if x1>=1 && x2<=96 && y1>=1 && y2 <=96 then
                (f.Image |> imToArray2D).[y1..y2,x1..x2] 
                |> DenseMatrix.ofArray2 
                |> Some
            else
                None              
        ) 
    |> Seq.cache

let firstRightEye:Matrix<float> = (rightEyeCenterSlicedMatrices |> Seq.nth 0)
R.c(firstRightEye.EnumerateRows() |> Seq.collect id |> Array.ofSeq |> Array.rev )
|> grayScaleImage 21 21

#time
let rightEyeMeans = 
    let len = rightEyeCenterSlicedMatrices |> Seq.length

    (rightEyeCenterSlicedMatrices
    |> Array.ofSeq     
    |> Array.reduce (fun acc m -> acc + m))/(float len)
    
#time

R.c(rightEyeMeans.EnumerateRows() |> Seq.collect id |> Array.ofSeq |> Array.rev)
|> grayScaleImage 21 21

// Searching for keypoint
let searchSize = 20
#time
let meanX, meanY = 
    let leftEyeCenters = 
        faces 
        |> Seq.filter (fun f -> f.LeftEyeCenter.IsSome) 

    leftEyeCenters |> Seq.map (fun f -> fst f.LeftEyeCenter.Value) |> Seq.average,
    leftEyeCenters |> Seq.map (fun f -> snd f.LeftEyeCenter.Value) |> Seq.average 
#time
let x1,x2,y1,y2 = int(meanX)-searchSize, int(meanX)+searchSize, int(meanY)-searchSize, int(meanY)+searchSize

// (64,35) to (68,39)
// params <- expand.grid(x = x1:x2, y = y1:y2)
let xy_params =
    [|
        for i in x1..x2 do
            for j in y1..y2 ->
                i,j
    |] 
    |> Array.mapi (fun i (x,y) -> [|(i,"x",x);(i,"y",y)|])
    |> Array.collect id
    |> Frame.ofValues

// get test images
let testLines = File.ReadAllLines(_test_)
let testImages = 
    (testLines |> Seq.skip 1 |> Seq.map (fun (l:string) -> l.Split(',')))
    |> Seq.map (fun l -> l.[1].Split(' ') |> Seq.map Int32.Parse |> Array.ofSeq)    
    |> Array.ofSeq

open MathNet.Numerics.Statistics
let getCorrelation (x,y) im =    
    let p = 
        (im |> imToArray2D).[int(y-float(patchSize))..int(y+float(patchSize)),int(x-float(patchSize))..int(x+float(patchSize))] 
        |> DenseMatrix.ofArray2

    let pPatch = p.EnumerateRows() |> Seq.collect id
    let meanPatch = leftEyeMeans.EnumerateRows() |> Seq.collect id    
    Correlation.Pearson(pPatch, meanPatch)

let getMaxRow im =
    let scores = 
        xy_params.Rows.Values 
        |> Seq.map (fun s -> getCorrelation (s?x,s?y) im)
        |> Series.ofValues 
    let xyDf = xy_params.Clone()
    xyDf?score <- scores
    xyDf.Rows.Values |> Seq.maxBy (fun r -> r?score)


let im = testImages.[8]
Array.rev(im)  |> grayScaleImage 96 96

let maxRow = getMaxRow im
R.points(imPointParams (maxRow?y,maxRow?x) "blue")

