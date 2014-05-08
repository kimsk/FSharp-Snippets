(*** hide ***)
#I ".."
#load "packages/FsLab.0.0.13-beta/FsLab.fsx"
open FSharp.Charting
(**
Programming Charts
==================

F# code for [Programming Charts] unit.

How to plot:
*)
let data = [3;4;2;4;3;5;3;6;4;3]
(*** define-output:chart1 ***)
data 
|> Seq.groupBy id 
|> Seq.map (fun (l,v) -> l.ToString(),(Seq.length v))
|> Seq.sortBy fst
|> Chart.Column
(*** include-it:chart1 ***)
(**
 More complex data set:
*)
let height= [65.78; 71.52; 69.4; 68.22; 67.79; 68.7; 69.8; 70.01; 67.9; 66.78;
 66.49; 67.62; 68.3; 67.12; 68.28; 71.09; 66.46; 68.65; 71.23; 67.13; 67.83; 
68.88; 63.48; 68.42; 67.63; 67.21; 70.84; 67.49; 66.53; 65.44; 69.52; 65.81; 
67.82; 70.6; 71.8; 69.21; 66.8; 67.66; 67.81; 64.05; 68.57; 65.18; 69.66; 67.97; 
65.98; 68.67; 66.88; 67.7; 69.82; 69.09]

// Divide to 5 sub lists
let heightGroups = 
    [
        let take = (height |> Seq.length)/5
        for i in 0..4 ->
            height |> Seq.sort |> Seq.skip (i*5) |> Seq.take take
    ]

heightGroups 
|> Seq.map (fun (s:seq<float>) -> 
        sprintf "%.2f-%.2f" (s |> Seq.head) (s |> Seq.last), (s |> Seq.length)
    )

let weight= [112.99; 136.49; 153.03; 142.34; 144.3; 123.3; 141.49; 136.46; 
112.37; 120.67; 127.45; 114.14; 125.61; 122.46; 116.09; 140.0; 129.5; 142.97; 
137.9; 124.04; 141.28; 143.54; 97.9; 129.5; 141.85; 129.72; 142.42; 131.55; 
108.33; 113.89; 103.3; 120.75; 125.79; 136.22; 140.1; 128.75; 141.8; 121.23; 
131.35; 106.71; 124.36; 124.86; 139.67; 137.37; 106.45; 128.76; 145.68; 116.82; 
143.62; 134.93]


(**
[Programming Charts]: https://www.udacity.com/course/viewer#!/c-st101/l-48704340
[Linear]: https://en.wikipedia.org/wiki/Linear
[Outlier]: https://en.wikipedia.org/wiki/Outlier
[Interpolation]: http://en.wikipedia.org/wiki/Interpolation
[Noise]: http://en.wikipedia.org/wiki/Statistical_noise
[Histogram]: https://en.wikipedia.org/wiki/Histogram
*)
