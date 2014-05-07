(*** hide ***)
#I ".."
#load "packages/FsLab.0.0.13-beta/FsLab.fsx"

(**
Bar Charts
==========

F# code for [Bar Charts] unit.
*)
[<Measure>] type sqft
[<Measure>] type dollar
let sizes = [1300<sqft>;1400<sqft>;1600<sqft>;1900<sqft>;2100<sqft>;2300<sqft>]
let prices1 = [88000<dollar>;72000<dollar>;94000<dollar>;86000<dollar>;112000<dollar>;98000<dollar>]

open Deedle
let df1 = Frame(["size"; "price"], [ Series.ofValues sizes; Series.ofValues prices1])
(*** include-value:df1 ***)
(*** define-output:chart1 ***)
open FSharp.Charting
Chart.Point(Seq.zip sizes prices1)
(*** include-it:chart1 ***)
(**
Is this [Linear]? **No**

How much to pay for a 2200 sqft house? Using [Interpolation] we got **10500**, but the number is not trusted because there are [Noise].
*)
112000 + (98000-112000)/2
(**
Using bar chart to average out the noise.
*)
let combinedPrices1 = 
    prices1 
    |> Seq.mapi (fun i p -> i,p) 
    |> Array.ofSeq 
    |> Array.partition (fun (i,p) -> i%2=0)
    ||> Array.mapi2 (fun i x y -> i,(snd(x)+snd(y))/2)
(*** include-value:combinedPrices1 ***)
(*** define-output:chart2 ***)
Chart.Column(combinedPrices1)
(*** include-it:chart2 ***)
(**
[Bar Charts]: https://www.udacity.com/course/viewer#!/c-st101/l-48727696
[Linear]: https://en.wikipedia.org/wiki/Linear
[Outlier]: https://en.wikipedia.org/wiki/Outlier
[Interpolation]: http://en.wikipedia.org/wiki/Interpolation
[Noise]: http://en.wikipedia.org/wiki/Statistical_noise
*)

