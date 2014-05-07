(*** hide ***)
#I ".."
#load "packages/FsLab.0.0.13-beta/FsLab.fsx"

(**
Scatter Plots
=============

F# code for [Scatter Plots] unit.

Is this linear?
*)
let data1 = [(1700,51000);(2100,63000);(1900,57000);(1300,39000);(1600,48000);(2200,66000)]

open Deedle
let df1 = Frame(["size"; "price"], [ data1 |> Seq.map fst |> Series.ofValues ; data1 |> Seq.map snd |> Series.ofValues])
(*** include-value:df1 ***)
(*** define-output:chart ***)
open FSharp.Charting
Chart.Point(data1)
(*** include-it:chart ***)
(**
[Scatter Plots]: https://www.udacity.com/course/viewer#!/c-st101/l-48646867
[Linear]: https://en.wikipedia.org/wiki/Linear
*)


