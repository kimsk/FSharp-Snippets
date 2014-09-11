open System

// imperative version
type SimpleStack<'T>()=
  let mutable _stack : List<'T> = []

  static let mutable _x = 0
  static member X with get() = _x and set(x) = _x <- x

  member self.Push value =
    lock _stack (fun() -> 
      _stack <- value::_stack
    )

  member this.Pop() =
    lock _stack (fun() -> 
                  let result::remainder = _stack
                  _stack <- remainder
                  result
                )

  member this.TryPop() =
    match _stack with
    | [] -> None
    | result::remainder -> 
      _stack <- remainder
      Some result

   member this.SafeSwap() =
    match _stack with
    | a::b::tail -> _stack <- b::a::tail
    | _ -> failwith "failed"
  
  

let stack = SimpleStack<float>()

stack.Push Math.PI
stack.Push 1.

stack.SafeSwap()

stack.Pop()
stack.TryPop()

SimpleStack<_>.X <- 20
SimpleStack<int>.X <- -1
SimpleStack<float>.X <- 1


// implement iter
let MyListIter f list =
  let rec loop l =
    match l with
    | head::tail -> 
        f head
        loop tail
    | [] -> ()
  loop list


// list -> list1, list2

let PartitionUtil predicate input =
  let rec loop acc list =
    printfn "%A %A" acc list
    match list with
    | head::tail when predicate head -> acc|>List.rev, (head::tail)
    | head::tail -> loop (head::acc) tail
    //| [] -> printfn "HERE"; acc|>List.rev, []
    | [] -> input,[]

  loop [] input

[1;2;3;0;1;1;0;1]
|> PartitionUtil ((=)0) 

[1;2;3;4]
|> PartitionUtil ((=)0) 

[1;2;3;4;0]
|> PartitionUtil ((=)0) 

[0;1;2]
|> PartitionUtil ((=)0) 

[]
|> PartitionUtil ((=)0) 



