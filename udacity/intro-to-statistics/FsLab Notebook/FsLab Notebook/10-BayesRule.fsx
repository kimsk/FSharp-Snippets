(*** hide ***)
#I ".."
#load "packages/FsLab.0.0.13-beta/FsLab.fsx"
open FSharp.Charting
open Deedle
(**
Bayes Rule
==========

F# code for [Bayes Rule] unit. Bayes Rule is named after [Thomas Bayes].

P(C) = 0.01

TEST: 

- 90% it is positive if you have C ([Sensitivity])
- 90% it is negative if you don't have C ([Specificity])

QUESTION: Positive Test, What is probability that you have a cancer? **~8%**

### Bayes Rule

Prior probability . Test Evidence -> [Posterior probability]

#### prior: 

- P(C) = 0.01, P(~C) = 0.99
- P(Pos|C) = 0.9 (Sensitivity), P(Neg|C) = 0.1
- P(Neg|~C) = 0.9 (Specificity), P(Pos|~C) = 0.1 

#### joint: 

- P(C, Pos) = P(C).P(Pos|C) = 0.009, P(C,Neg) = P(C).P(Neg|C) = 0.001
- P(~C, Pos) = P(~C).P(Pos|~C) = 0.099, P(~C,Neg) = P(~C).P(Neg|~C) = 0.891

#### normalizer:

- P(Pos) = P(C,Pos)+P(~C,Pos) = 0.108
- P(Neg) = P(C,Neg)+P(~C,Neg) = 0.892

#### posterior:

- P(C|Pos) = P(C, Pos)/P(Pos) = 0.0833
- P(~C|Pos) = P(~C, Pos)/P(Pos) = 0.9167
- P(C|Neg) = P(C,Neg)/P(Neg) = 0.001121
- P(~C|Neg) = P(~C,Neg)/P(Neg) = 0.998879

*)
(*** hide ***)
0.01*0.1 + 0.99*0.9
0.01*0.9 + 0.99*0.1
0.01*0.9/0.108 + 0.99*0.1/0.108
0.01*0.1/0.892 + 0.891/0.892

0.1*0.1/0.46 + 0.9*0.5/0.46

0.1*0.9/0.54 + 0.9*0.5/0.54
(**
### Robot Sensing problems

A robot in green and red fields.

P(R) = P(G) = 0.5
P(see R| R) = 0.8
P(see G| G) = 0.8

*)

let ``P(R)``,``P(G)`` = (0.5,0.5) // prior
let ``P(see R|R)`` = 0.8 // sensitivity
let ``P(see G|G)`` = 0.8 // specificity
let ``P(see R|G)`` = 0.2

// 2nd data set
//let ``P(R)``,``P(G)`` = (0.0,1.0) // prior
//let ``P(see R|R)`` = 0.8 // sensitivity
//let ``P(see G|G)`` = 0.5 // specificity
//let ``P(see R|G)`` = 0.5

// joint
let ``P(R,see R)`` = ``P(R)``*``P(see R|R)``
let ``P(G,see R)`` = ``P(G)``*``P(see R|G)``

// normalizer
let ``P(see R)`` = ``P(R,see R)`` + ``P(G,see R)``

// posterior
let ``P(R|see R)`` = ``P(R,see R)``/``P(see R)``
let ``P(G|see R)`` = ``P(G,see R)``/``P(see R)``
(**
What are the posterior probabilities P(R|see R) and P(G|see R)?
*)
(*** include-value:``P(R|see R)`` ***)
(*** include-value:``P(G|see R)`` ***)
(**
[Bayes Rule]: https://www.udacity.com/course/viewer#!/c-st101/l-48703346
[Thomas Bayes]: https://en.wikipedia.org/wiki/Thomas_Bayes
[Sensitivity]: https://en.wikipedia.org/wiki/Sensitivity_and_specificity#Sensitivity
[Specificity]: https://en.wikipedia.org/wiki/Sensitivity_and_specificity#Specificity
[Posterior probability]: https://en.wikipedia.org/wiki/Posterior_probability
*)
