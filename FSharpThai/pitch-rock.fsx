open System

type GenericType() =
    member x.GenericFunc1<'T> (p:'T) =
        p, typeof<'T>.ToString()

    member x.GenericFunc2<'T, 'R> (p:'T):'T * 'R * string =
        p, Unchecked.defaultof<'R>, typeof<'R>.ToString()



let genType = GenericType()

"Hello" |> fun x -> genType.GenericFunc2<string, int> x
"Hello" |> genType.GenericFunc1

let f':string->string*'R*string = genType.GenericFunc2
let result:string*int*string = "Hello" |> f

"Hello" |> genType.GenericFunc2

let f a b = a.ToString()
let g a b = a + b
let h a b = a <> b

let (<-->) a b = (b,a)
10 <--> 20 // (20,10)

let addFive = (+) 5
addFive 10 // 15
