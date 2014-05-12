(*** hide ***)
#I ".."
#load "packages/FsLab.0.0.13-beta/FsLab.fsx"
open FSharp.Charting
open Deedle
(**
Conditional Probability
=======================

F# code for [Conditional Probability] unit.

P(CANCER) = 0.1, P(~CANCER) = 0.9

P(POSITIVE | CANCER) = 0.9, P(NEGATIVE | CANCER) = 0.1

P(POSITIVE | ~CANCER) = 0.2, P(NEGATIVE | ~CANCER) = 0.8

P(P) = P(P|C)xP(C) + P(P|~C)xP(~C)

*)
type Coin = HEADS | TAILS

let p1 = (0.5,0.5)
let p2 = (0.9,0.1)
let getP v c p1 p2 = 
    if v = 1 then
        if c = HEADS then fst(p1) else snd(p1)
    else
        if c = HEADS then fst(p2) else snd(p2)

let pC = (0.5,0.5)
let getPc v pC = if v = 1 then fst(pC) else snd(pC)

let tt = 
    [
        for i in 1..2 do
            for j in [HEADS;TAILS] do
                for k in [HEADS;TAILS] ->
                    (i,j,k, (getPc i pC)*(getP i j p1 p2)*(getP i k p1 p2))
    ]

let pHT =
    tt 
    |> Seq.filter (fun r -> 
            match r with
            | _, HEADS, TAILS, _ -> true
            | _ -> false )
    |> Seq.fold (fun acc (_,_,_,p) -> acc+p) 0.
(**
What is the probability of getting HEADS and TAILS?
*)
(*** include-value:pHT ***)
let p3 = (1.0,0.)
let p4 = (0.6,0.4)
let pC1 = (0.5,0.5)
let tt2 = 
    [
        for i in 1..2 do
            for j in [HEADS;TAILS] do
                for k in [HEADS;TAILS] ->
                    (i,j,k, (getPc i pC1)*(getP i j p3 p4)*(getP i k p3 p4))
    ]

let pTT =
    tt2 
    |> Seq.filter (fun r -> 
            match r with
            | _, TAILS, TAILS, _ -> true
            | _ -> false )
    |> Seq.fold (fun acc (_,_,_,p) -> acc+p) 0.
(**
What is the probability of getting TAILS and TAILS?
*)
(*** include-value:pTT ***)
(**
P(TEST) = P(TEST|DISEASE)xP(DISEASE) + P(TEST|~DISEASE)xP(~DISEASE)
*)
(**
[Conditional Probability]: https://www.udacity.com/course/viewer#!/c-st101/l-48729372
*)


