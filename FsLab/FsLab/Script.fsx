#I @"..\packages\FsLab.0.0.13-beta\"
#load @"FsLab.fsx"

// R Stuff
open RProvider
open RProvider.``base``
open RProvider.graphics

let v = R.c({1..16})
let m = R.matrix(data = v, nrow = 4, ncol = 4)

// create identity matrix
let i = R.diag 4

let data = [for i in 0. .. 0.1 .. 10. -> i * sin i ]
R.plot data

// REngine
let c = i.Engine.Evaluate("c(1,2,3)")


// Math.NET
open MathNet.Numerics.LinearAlgebra
open MathNet.Numerics.LinearAlgebra.Double
open MathNet.Numerics.Distributions
let a1 = DenseMatrix(2, 3, [| 1.0; 2.0; 10.0; 20.0; 100.0; 300.0 |])
let a2 = DenseMatrix.raw 2 3 [| 1.0; 2.0; 10.0; 20.0; 100.0; 300.0 |]

let b1:Matrix<float> = DenseMatrix.zero 3 4
let b2 = DenseMatrix.create 3 4 20.5
let b3 = SparseMatrix.create 3 4 0.
b3.[1,1] <- 10.
// Or create it from a multi-dimensional array
let k = DenseMatrix.ofArray2 (array2D [[1.0;  2.0;  3.0]; [10.0; 11.0; 12.0]])

// We can now add two matrices together ...
let z = a1 + a2
z.Transpose()

// create random matrix with Normal Distribution
let normal = Normal()
let sq:Matrix<float> = DenseMatrix.random 5 5 normal

a1 * b2

// identity matrix
let ident:Matrix<float> = DenseMatrix.identity 5
let ident2 = DenseMatrix.diag 5 1.

let m2 = matrix [[1.; 2.]; [3.; 4.]]
let v2 = vector [4.;5.]