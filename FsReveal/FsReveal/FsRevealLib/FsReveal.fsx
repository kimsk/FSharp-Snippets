(*** slide-start ***)
(**
***
# F#Reveal
***
#### Karlkim Suwanmongkol
#### @kimsk
#### [http://karlk.im](http://karlk.im)
#### to@karlk.im
*)
(*** slide-end ***)
(*** slide-start ***)
(*** hide ***)
let Markdown = "markdown"
let ``F# code`` = "code"
type Awesome = { name: string}
let FsReveal markdown fsharp = 
  "awesome"
(** 
## What is F#Reveal?

- [Reveal.js](http://lab.hakim.se/reveal-js/#/) by _Hakim El Hattab_
- [FSharp.Formatting](https://github.com/tpetricek/FSharp.Formatting) by _Tomas Petricek_
- F#Reveal parse markdown and F# code to reveal.js presentation  
  
  
#### How about some F# code..
*)

let output = (Markdown, ``F# code``) ||> FsReveal
(*** slide-end ***)
(*** slide-start ***)
(**
## Why F#Reveal?

- F# Type inference
- Don't need to specify type (most of the time)
- Less verbose and **Easy** to write
- Can be **hard** to read without help from an IDE
*)
(*** slide-end ***)
(*** slide-start ***)
(*** slide-start ***)
(** 
## Card Game Sample

> Domain Driven Design with F# type System by *Scott Wlaschin*
*)
(*** slide-end ***)
(*** slide-start ***)
(** 
#### F# Type Definitions
*)
type Suit = Club|Diamond|Spade|Heart
type Rank = Two|Three|Four|Five|Six|Seven|Eight
            |Nine|Ten|Jack|Queen|King|Ace
type Card = Suit * Rank   
type Person = { Name: string; Cards: seq<Card>}
(*** slide-end ***)
(*** slide-start ***)
(**
### Some random F# function
*)
let pickHighRanks player suit =
  player.Cards 
  |> Seq.filter (fun c -> fst(c) = suit)
  |> Seq.filter(fun (s,r) -> 
      match r with
      | Ace
      | King
      | Queen
      | Jack -> true
      | _ -> false  )
(** 
Need to think like a compiler to understand the code above..
*)
(*** slide-end ***)
(*** slide-end ***)

