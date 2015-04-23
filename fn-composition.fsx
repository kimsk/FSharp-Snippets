//http://blogs.msdn.com/b/chrsmith/archive/2008/06/14/function-composition.aspx
let negate x = x * -1
let sqr x = x * x
let print x = printfn "The number is %d" x

let sqr_negate_print = sqr >> negate >> print

10
|> sqr
|> negate
|> print

10 |> sqr_negate_print

let a() = printfn "A"
let b() = printfn "B"
let c() = printfn "C"

let abc = a >> b >> c