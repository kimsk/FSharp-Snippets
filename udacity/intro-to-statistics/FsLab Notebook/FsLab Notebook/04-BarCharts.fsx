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
[Histogram] is a special-case of bar chart. Both aggregate data, but bar chart is showing 2D data while histogram is presenting frequencies in 1D.
*)
let incomes = [132754;137192;122177;147121;143000;126010;129200;124312;128132]
let frequencies =
    incomes 
    |> Seq.fold (fun (x,y,z) i -> 
            if i < 130000 then (x+1,y,z)
            elif i < 140000 then (x,y+1,z)
            else (x,y,z+1)
        ) (0,0,0)
    |> (fun (x,y,z) -> ["120000",x;"130000",y;"140000",z])
(*** include-value:frequencies ***)
(*** define-output:chart3 ***)
Chart.Column(frequencies)
(*** include-it:chart3 ***)
let ages = [21;17;9;27;35;4;32;12;12;14;38;9;19;22;14;21;3;8;31;15;33;29]
let agesDist = 
    ages 
    |> Seq.fold (fun (a,b,c,d) i -> 
            if i < 11 then (a+1,b,c,d)
            elif i < 21 then (a,b+1,c,d)
            elif i < 31 then (a,b,c+1,d)          
            else (a,b,c,d+1)
        ) (0,0,0,0)
    |> (fun (a,b,c,d) -> ["0-10",a;"11-20",b;"21-30",c;"31-40",d])
(*** include-value:agesDist ***)
(*** define-output:chart4 ***)
Chart.Column(agesDist)
(*** include-it:chart4 ***)
(**
[Bar Charts]: https://www.udacity.com/course/viewer#!/c-st101/l-48727696
[Linear]: https://en.wikipedia.org/wiki/Linear
[Outlier]: https://en.wikipedia.org/wiki/Outlier
[Interpolation]: http://en.wikipedia.org/wiki/Interpolation
[Noise]: http://en.wikipedia.org/wiki/Statistical_noise
[Histogram]: https://en.wikipedia.org/wiki/Histogram
*)

