(*** hide ***)
#I ".."
#load "packages/FsLab.0.0.13-beta/FsLab.fsx"

(**
Scatter Plots
=============

F# code for [Scatter Plots] unit.
*)
[<Measure>] type sqrft
[<Measure>] type dollar
let data1 = 
    [
        (1700<sqrft>,51000<dollar>)
        (2100<sqrft>,63000<dollar>)
        (1900<sqrft>,57000<dollar>)
        (1300<sqrft>,39000<dollar>)
        (1600<sqrft>,48000<dollar>)
        (2200<sqrft>,66000<dollar>)
    ]

open Deedle
let df1 = Frame(["size"; "price"], [ data1 |> Seq.map fst |> Series.ofValues ; data1 |> Seq.map snd |> Series.ofValues])
(*** include-value:df1 ***)
(*** define-output:chart1 ***)
open FSharp.Charting
Chart.Point(data1)
(*** include-it:chart1 ***)
(**
Is this linear? **Yes**
*)
(**
Do we believe there is a fixed price per square foot? **Yes**
*)
let pricePerSqrft1 = (data1 |> Seq.map snd |> Seq.sum)/(data1 |> Seq.map fst |> Seq.sum)
(*** include-value:pricePerSqrft1 ***)
(** 
** Second data set
*)
let data2 = 
    [
        (1700<sqrft>,53000<dollar>)
        (2100<sqrft>,65000<dollar>)
        (1900<sqrft>,59000<dollar>)
        (1300<sqrft>,41000<dollar>)
        (1600<sqrft>,50000<dollar>)
        (2200<sqrft>,68000<dollar>)
    ]

let df2 = Frame(["size"; "price"], [ data2 |> Seq.map fst |> Series.ofValues ; data2 |> Seq.map snd |> Series.ofValues])
(*** include-value:df2 ***)
(*** define-output:chart2 ***)
Chart.Point(data2)
(*** include-it:chart2 ***)
(**
Is this linear? **Yes**
*)
(**
Do we believe there is a fixed price per square foot? **No**
*)
let pricePerSqrft2 = float(data2 |> Seq.map snd |> Seq.sum)/float(data2 |> Seq.map fst |> Seq.sum)
(*** include-value:pricePerSqrft2 ***)
let firstHome = pricePerSqrft2*1700.<sqrft>
(*** include-value:firstHome***)
(**
How to calculate the price?
*)
30<dollar/sqrft>*1700<sqrft> + 2000<dollar>
(** 
** 3rd data set
*)
let data3 = 
    [
        (1700<sqrft>,53000<dollar>)
        (2100<sqrft>,44000<dollar>)
        (1900<sqrft>,59000<dollar>)
        (1300<sqrft>,82000<dollar>)
        (1600<sqrft>,50000<dollar>)
        (2200<sqrft>,68000<dollar>)
    ]

let df3 = Frame(["size"; "price"], [ data3 |> Seq.map fst |> Series.ofValues ; data3 |> Seq.map snd |> Series.ofValues])
(*** include-value:df3 ***)
(*** define-output:chart3 ***)
Chart.Point(data3)
(*** include-it:chart3 ***)
(**
Is this linear? **No**, because there are [Outlier]s
*)
(**
[Scatter Plots]: https://www.udacity.com/course/viewer#!/c-st101/l-48646867
[Linear]: https://en.wikipedia.org/wiki/Linear
[Outlier]: https://en.wikipedia.org/wiki/Outlier
*)


