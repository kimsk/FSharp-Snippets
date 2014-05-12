(*** hide ***)
#I ".."
#load "packages/FsLab.0.0.13-beta/FsLab.fsx"
open FSharp.Charting
open Deedle
(**
Probability
===========

F# code for [Probability] unit.
*)
(** 
Data -> Statistics -> Causes

Causes -> Probability -> Data

P(A) = 1 - P(~A)**
*)
type Coin = HEADS | TAILS
let getTT (p:float*float) = 
    let flips = [HEADS;TAILS]
    let getP coin = if coin = HEADS then fst(p) else snd(p)
    [
        for i in flips do
            for j in flips do
                for k in flips ->
                    (i,j,k,(getP(i)*getP(j)*getP(k)))
    ] 

let tt = getTT (0.6, 0.4)
let tt' = 
    tt 
    |> List.rev
    |> Seq.fold (fun acc r -> 
            let f1,f2,f3,p = r
            let l1,l2,l3,l4 = acc
            (sprintf "%A" f1::l1,sprintf "%A" f2::l2,sprintf "%A" f3::l3,p::l4)     
        ) ([],[],[],[])
let df = 
    let l1,l2,l3,l4 = tt'    
    Frame(["flip-1"; "flip-2";"flip-3";"Probability"], 
        [ Series.ofValues l1; Series.ofValues l2; Series.ofValues l3; Series.ofValues l4])
(*** include-value:df ***)
let pOneH = 
    tt 
    |> Seq.filter (fun r -> 
            match r with
            | HEADS, TAILS, TAILS, _ 
            | TAILS, HEADS, TAILS, _
            | TAILS, TAILS, HEADS, _ -> true
            | _ -> false )
    |> Seq.fold (fun acc (_,_,_,p) -> acc+p) 0.
(**
P(Exactly one H in 3 flips) = 
*)
(*** include-value:pOneH ***)
(**
### Summary
- Probability of event : P
- Probability of opposite event : 1-P
- Probability of composite independence event : PxPx..P
*)
(**
[Probability]: https://www.udacity.com/course/viewer#!/c-st101/l-48738235
*)

