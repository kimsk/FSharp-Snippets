(*** hide ***)
#I ".."
#load "packages/FsLab.0.0.13-beta/FsLab.fsx"
open FSharp.Charting
open Deedle
(**
Programming Bayes Rule
======================

F# code for [Programming Bayes Rule] unit.

### Coin Example 1

P(H) = 0.5

What is the probability of exactly one head in three flips?
*)
let ``P(H)`` = 0.5

let ``P(exactly one H)`` = ``P(H)``*(1.-``P(H)``)*(1.-``P(H)``)*3.
(*** include-value:``P(exactly one H)`` ***)
(**
### Coin Example 2

- P1(H) = 0.5
- P2(H) = 0.8

What is the probability of flipping one head each from two coins?

*)
let ``P1(H)`` = 0.5
let ``P2(H)`` = 0.8

``P1(H)``*``P2(H)``
(*** include-value:``P1(H)``*``P2(H)`` ***)
(**
### Coin Example 3

- P(C1) = 0.3
- P(H|C1) = 0.5
- P(H|C2) = 0.9

What is P(H)?

- P(H) = P(C1,H)+P(C2,H)
- P(C1,H) = P(C1)*P(H|C1)
- P(C2,H) = P(C2)*P(H|C2)
*)

let ``P(C1)`` = 0.3
let ``P(H|C1)`` = 0.5
let ``P(H|C2)`` = 0.9

``P(C1)``*``P(H|C1)`` + (1.-``P(C1)``)*``P(H|C2)``
(*** include-value:``P(C1)``*``P(H|C1)`` + (1.-``P(C1)``) * ``P(H|C2)`` ***)
(**
### Cancer Example

- P(C) = 0.1
- P(Pos|C) = 0.9
- P(Neg|~C) = 0.8

What is P(Pos)?

P(Pos) = P(C) * P(Pos|C)+P(~C) * P(Pos|~C)
*)
let ``P(C)`` = 0.1
let ``P(Pos|C)`` = 0.9
let ``P(Neg|~C)`` = 0.8

let ``P(Pos)`` = ``P(C)``*``P(Pos|C)``+(1.-``P(C)``)*(1.-``P(Neg|~C)``)
(*** include-value:``P(C)``*``P(Pos|C)``+(1.-``P(C)``)*(1.-``P(Neg|~C)``) ***)
(**
What is P(C|Pos)?

P(C|Pos) = P(C)*P(Pos|C)/P(Pos)
*)
``P(C)``*``P(Pos|C)``/``P(Pos)``
(*** include-value:``P(C)``*``P(Pos|C)``/``P(Pos)`` ***)
(** 
What is P(C|Neg)?

P(C|Neg) = P(C)*P(Neg|C)/P(Neg)
*)
``P(C)``*(1.-``P(Pos|C)``)/(1.-``P(Pos)``)
(*** include-value:``P(C)``*(1.-``P(Pos|C)``)/(1.-``P(Pos)``) ***)
(**
[Programming Bayes Rule]: https://www.udacity.com/course/viewer#!/c-st101/l-48729374
*)
