#I @"..\packages\FsLab.0.0.13-beta\"
#load @"FsLab.fsx"

open MathNet.Numerics
open MathNet.Numerics.LinearAlgebra


let a = matrix [[ 3.0; 2.0; -1.0 ]
                [ 2.0; -2.0; 4.0 ]
                [ -1.0; 0.5; -1.0 ]]
let b = vector [ 1.0; -2.0; 0.0 ]

let a' = a.Transpose()

let aPlus5 = a + 5.

let m : Matrix<float> = DenseMatrix.randomStandard 50 50
(m * m.Transpose()).Determinant()

m.UpperTriangle()

m.EnumerateRows() |> Array.ofSeq |> Seq.collect id

let f i j =
    ((float i)*10.) + (float j)

let arr = Array2D.init 10 10 f
let arr2 = arr.[..5,..8]

let m2:Matrix<float> = DenseMatrix.ofArray2 arr2

let colMeans (m:Matrix<float>) =
    let m' = m.Transpose()
    let len = m'.Row(0).Count |> float
    m'.EnumerateRows()
    |> Seq.map (fun r -> (Seq.sum r)/len)    

colMeans m2