(**
let! x = 1
let! y = 2 
let! z = x + y

is compiled to

Bind(1, fun x ->
Bind(2, fun y ->
Bind(x + y, fun z -> z)))

Bind is the same as pipeInto

and let! is just syntactic sugar for Bind...

Sometime you might see

let (>>=) m f = pipeInto(m,f)
*)

let (>>=) m f =
  printfn "expression is %A" m
  f m

let loggingWorkflow =
  1 >>= (+) 2 >>= (*) 42 >>= id