#r @"..\packages\MathNet.Numerics.2.6.1\lib\net40\MathNet.Numerics.dll"
#r @"..\packages\MathNet.Numerics.FSharp.2.6.0\lib\net40\MathNet.Numerics.FSharp.dll"
#r @"..\packages\Sharpkit.Learn.0.2.0\lib\net40\Sharpkit.Learn.dll"

open MathNet.Numerics.LinearAlgebra
open MathNet.Numerics.LinearAlgebra.Double
open MathNet.Numerics.Distributions
let a1 = DenseMatrix(2, 3, [| 1.0; 2.0; 10.0; 20.0; 100.0; 300.0 |])
let a2 = DenseMatrix.raw 2 3 [| 1.0; 2.0; 10.0; 20.0; 100.0; 300.0 |]

let b1 = DenseMatrix.zeroCreate 3 4
let b2 = DenseMatrix.create 3 4 20.5
let b3 = SparseMatrix.zeroCreate 3 4

// Or create it from a multi-dimensional array
let k = DenseMatrix.ofArray2 (array2D [[1.0;  2.0;  3.0]; [10.0; 11.0; 12.0]])

// We can now add two matrices together ...
let z = a1 + a2
z.Transpose()

// create random matrix with Normal Distribution
let normal = Normal()
let sq = DenseMatrix.randomCreate 5 5 normal

a1 * b2

// identity matrix
DenseMatrix.Identity 5

let m = matrix [[1.; 2.]; [3.; 4.]]
let v = vector [4.;5.]


open Sharpkit.Learn.LinearModel
let clf = new LinearRegression()
clf.Fit(array2D [[0.0; 0.0]; [1.0; 1.0]; [2.0; 2.0]], [|0.0; 1.0; 2.0|]) |> ignore
clf.Coef

let prediction = clf.Predict([|3.0; 3.0|])
prediction


