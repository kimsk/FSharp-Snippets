#I @"..\packages\FsLab.0.0.13-beta\"
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

// get values from face
let im = R.matrix(nrow=96,ncol=96,data=Array.rev((face 0).Image))
let noseTipXy = imXy (face 0).NoseTip
let leftEyeCenterXy = imXy (face 0).LeftEyeCenter
let rightEyeCenterXy = imXy (face 0).RightEyeCenter

// Visualize image using R
// image(1:96, 1:96, im, col=gray((0:255)/255))
let imageParams = 
    [
        "x",R.c([|1..96|])
        "y",R.c([|1..96|])
        "z",im
        "col",R.gray(["level",R.c(seq { for i in 0. .. 255. -> i/255.})]|>namedParams)
    ] |> namedParams
R.image(imageParams)

// add points for nose tip, left eye, right eye
// points(96-d.train$nose_tip_x[1], 96-d.train$nose_tip_y[1], col="red")
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


// helper function to slice matrix
let sliceMatrix (m:SymbolicExpression) (x1,x2,y1,y2) =
    m.Engine.SetSymbol("m", m)
    sprintf "m[%M:%M,%M:%M]" x1 x2 y1 y2 |> m.Engine.Evaluate


let patchSize = 10m
let patches = 
    let leftEyeCenters = 
        faces 
        |> Seq.filter (fun f -> f.LeftEyeCenter.IsSome) 
        |> Seq.map (fun f -> f.LeftEyeCenter.Value)

    leftEyeCenters
    |> Seq.mapi (fun i (x,y) -> 
            let im = R.matrix(nrow=96,ncol=96,data=Array.rev((face i).Image))
            im.Engine.SetSymbol("im", im)
            let x1,x2,y1,y2 = (x-patchSize),(x+patchSize),(y-patchSize),(y+patchSize)
            if x1>=1m && x2<=96m && y1>=1m && y2 <=96m then
                let newMatrix = sliceMatrix im (x1,x2,y1,y2)
                Some(R.as_vector(newMatrix).AsList())
            else
                None                
        )


// show patch of eye
let im2 = R.matrix(nrow=21,ncol=21,data=(patches |> Seq.nth 10).Value)
let imageParams2 = 
    [
        "x",R.c([|1..21|])
        "y",R.c([|1..21|])
        "z",im2
        "col",R.gray(["level",R.c(seq { for i in 0. .. 255. -> i/255.})]|>namedParams)
    ] |> namedParams
R.image(imageParams2)
    
    






