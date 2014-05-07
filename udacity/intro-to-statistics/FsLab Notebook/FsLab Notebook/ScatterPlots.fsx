(*** hide ***)
#I ".."
#load "packages/FsLab.0.0.13-beta/FsLab.fsx"

(**
Scatter Plots
=============

F# code for [Scatter Plots] unit.
*)
[<Measure>] type sqft
[<Measure>] type dollar
let sizes = [1700<sqft>;2100<sqft>;1900<sqft>;1300<sqft>;1600<sqft>;2200<sqft>]
let prices1 = [51000<dollar>;63000<dollar>;57000<dollar>;39000<dollar>;48000<dollar>;66000<dollar>]

open Deedle
let df1 = Frame(["size"; "price"], [ Series.ofValues sizes; Series.ofValues prices1])
(*** include-value:df1 ***)
(*** define-output:chart1 ***)
open FSharp.Charting
Chart.Point(Seq.zip sizes prices1)
(*** include-it:chart1 ***)
(**
Is this [Linear]? **Yes**
*)
(**
Do we believe there is a fixed price per square foot? **Yes**
*)
let pricePerSqrft1 = (Seq.sum prices1)/(Seq.sum sizes)
(*** include-value:pricePerSqrft1 ***)
(** 
** Second data set
*)
let prices2 = [53000<dollar>;65000<dollar>;59000<dollar>;41000<dollar>;50000<dollar>;68000<dollar>]
let df2 = Frame(["size"; "price"], [ Series.ofValues sizes; Series.ofValues prices2])
(*** include-value:df2 ***)
(*** define-output:chart2 ***)
Chart.Point(Seq.zip sizes prices2)
(*** include-it:chart2 ***)
(**
Is this [Linear]? **Yes**
*)
(**
Do we believe there is a fixed price per square foot? **No**
*)
let pricePerSqrft2 = float(Seq.sum prices2)/float(Seq.sum sizes)
(*** include-value:pricePerSqrft2 ***)
let firstHome = pricePerSqrft2*1700.<sqft>
(*** include-value:firstHome***)
(**
How to calculate the price?
*)
30<dollar/sqft>*1700<sqft> + 2000<dollar>
(** 
** 3rd data set
*)
let prices3 = [53000<dollar>;44000<dollar>;59000<dollar>;82000<dollar>;50000<dollar>;68000<dollar>]

let df3 = Frame(["size"; "price"],  [ Series.ofValues sizes; Series.ofValues prices3])
(*** include-value:df3 ***)
(*** define-output:chart3 ***)
Chart.Point(Seq.zip sizes prices3)
(*** include-it:chart3 ***)
(**
Is this [Linear]? **No**, because there are [Outlier]s
*)
(**
[Scatter Plots]: https://www.udacity.com/course/viewer#!/c-st101/l-48646867
[Linear]: https://en.wikipedia.org/wiki/Linear
[Outlier]: https://en.wikipedia.org/wiki/Outlier
*)


