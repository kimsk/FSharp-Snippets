//  http://weblogs.asp.net/podwysocki/f-duck-typing-and-structural-typing
//  https://msdn.microsoft.com/en-us/library/dd548046.aspx

type IAnimal = abstract member Roar:unit -> string

let dog = { new IAnimal with member x.Roar() = "DEE" }

type Cat() = member this.Roar() = "CEE"

dog.Roar() |> printfn "%A"

let Roar (animal:IAnimal) = animal.Roar()

dog |> Roar  |> printfn "%A"

let roar2 (animal: ^A when ^A:> IAnimal ):string =
    animal.Roar()

roar2 dog |> printfn "%A"

let cat = Cat()
cat.Roar()

//roar2 cat

let inline roar3 (animal: ^A when ^A: (member Roar:unit -> string)):string =
   (^A:(member Roar:unit->string) animal)
   //animal

let inline roar4 animal = (^A: (member Roar:unit -> string) animal)



dog |> roar3 |> printfn "%A"
cat |> roar3 |> printfn "%A"
dog |> roar4 |> printfn "%A"
cat |> roar4 |> printfn "%A"
   
  
   

