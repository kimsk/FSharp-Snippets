(*** hide ***)
#I ".."
#load "packages/FsLab.0.0.13-beta/FsLab.fsx"
open FSharp.Charting
(**
Pie Charts
==========

F# code for [Pie Charts] unit. Pie chart is used to show relative data.
*)
let votes1 = ["A",724000;"B",181000]
(** 
Find the percentages:
*)
let percentages1 = 
    let sum = votes1 |> Seq.map snd |> Seq.sum
    votes1 |> Seq.map (fun (l,v) -> l,(float(v)/float(sum)))
(*** define-output:chart1 ***)
Chart.Pie(votes1)
(*** include-it:chart1 ***)
let aVote = 0.8*23000./0.2
(*** include-value:aVote ***)
let classes = ["13-19",12000;"20-32",96000;"33-999",36000]
(*** define-output:chart2 ***)
Chart.Pie(classes)
(*** include-it:chart2 ***)
let votes2 = ["A",175000;"B",50000;"C",25000;"D",50000]
(** 
Find the percentages for votes2.
*)
let percentages2 = 
    let sum = votes2 |> Seq.map snd |> Seq.sum
    votes2 |> Seq.map (fun (l,v) -> l,(float(v)/float(sum)))
(*** define-output:chart3 ***)
Chart.Pie(percentages2)
(*** include-it:chart3 ***)
(**
If the total votes are 240,000, what are votes for A,B,C, and D?
*)
let votes3 = percentages2 |> Seq.map (fun (_,v) -> v*240000.)
(*** include-value:votes3 ***)
(**
[Pie Charts]: https://www.udacity.com/course/viewer#!/c-st101/l-48646868
[Linear]: https://en.wikipedia.org/wiki/Linear
[Outlier]: https://en.wikipedia.org/wiki/Outlier
[Interpolation]: http://en.wikipedia.org/wiki/Interpolation
[Noise]: http://en.wikipedia.org/wiki/Statistical_noise
[Histogram]: https://en.wikipedia.org/wiki/Histogram
*)
