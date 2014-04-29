#I @"..\packages\FsLab.0.0.13-beta\"
#load @"FsLab.fsx"

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


// slicing matrix
let im = DenseMatrix.CreateRandom(96, 96, normal)
im.GetSlice(Some(1),Some(21),Some(1),Some(21))

let slice r1 r2 r3 r4 (m:Matrix<float>) = 
    m.GetSlice(Some(r1), Some(r2), Some(r3), Some(r4))

#time
//let matrices = [|1..7049|] |> Array.map (fun _ -> DenseMatrix.CreateRandom(96, 96, normal))
let matrices = [|1..7049|] |> Array.map (fun _ -> DenseMatrix.create 96 96 1.)
#time

#time
let sliced = matrices |> Array.map (fun m -> m |> slice 1 21 1 21)
#time



#time
let means = 
    let len = matrices.Length

    (matrices 
    |> Array.map (fun m -> m |> slice 1 21 1 21)
    |> Array.reduce (fun acc m -> acc + m))/(float len)
    
#time