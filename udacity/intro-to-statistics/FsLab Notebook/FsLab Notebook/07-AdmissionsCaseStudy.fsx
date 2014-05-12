(*** hide ***)
#I ".."
#load "packages/FsLab.0.0.13-beta/FsLab.fsx"
open FSharp.Charting
(**
Admissions Case Study
=====================

F# code for [Admissions Case Study] unit.
*)
Data:
                MALE                             FEMALE                  
          Applied   Admitted  Rate         Applied   Admitted  Rate      
Major A     900      450      50%            100       80      80%
Major B     100       10      10%            900      180      20%
Both       1000      460      46%           1000      260      26%
(**
If we look at only each major, we'd think female is being favored. But the total numbers say otherwise!

> I never believe in statistics I didn't doctor myself. **Winston Churchill**

*)
(**
[Admissions Case Study]: https://www.udacity.com/course/viewer#!/c-st101/l-48759015
*)
