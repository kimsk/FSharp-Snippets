
// OO way to do tree
type FoodStuff(name: string, allergenic: bool, foodStuffs: FoodStuff list)=
   member this.Name = name
   member this.FoodStuffs = foodStuffs
   member this.Allergenic = allergenic


let cake = 
        FoodStuff("cake", false, 
            [
                FoodStuff("sugar",false, [])
                FoodStuff("salt",false, [])
                FoodStuff("peanut butter", false,
                    [
                        FoodStuff("peanut", true,[])
                        FoodStuff("butter", false,[])
                    ])
            ])

let rec IsAllergenic (foodStuff:FoodStuff) =
    foodStuff.Allergenic || foodStuff.FoodStuffs |> List.exists IsAllergenic

IsAllergenic cake


// Discriminated Union way
type Tree<'T> =
    | Leaf of 'T
    | Node of Tree<'T> * Tree<'T>

let tree = Node(
            Leaf(0), Node(
                        Node(Leaf(1), 
                            Node(Leaf(2), 
                                Node(Leaf(3), Leaf(4)))), 
                            Node(
                                Node(Leaf(5), Leaf(6)), 
                                Leaf(7))))

let rec traverseTree (tree:Tree<int>) =
    match tree with
    | Leaf(v) -> printfn "%d" v
    | Node(l,r) -> 
        traverseTree l
        traverseTree r

tree |> traverseTree

let rec treeToList acc (tree:Tree<int>) =
    match tree with
    | Leaf(v) -> v::acc
    | Node(l,r) ->
        (treeToList acc l)@(treeToList acc r)


let treeToList' tree =
    let rec loop t =
        [
            match t with
            | Leaf(v) -> yield v
            | Node(l,r) ->
                yield! loop l
                yield! loop r
        ]
    loop tree



let treeToList'' tree =
    let rec loop acc t =
        match t with
        | Leaf(v) -> v::acc
        | Node(l,r)->
            List.concat [(loop acc l); (loop acc r)]
    loop [] tree

tree |> treeToList []
tree |> treeToList'
tree |> treeToList''


let rec prettyPrint level tree =
    match tree with
    | Leaf(v) -> printfn "%s%d" (" ".PadRight(level)) v 
    | Node(l,r) ->
        prettyPrint (level+1) l
        prettyPrint (level+1) r

tree |> prettyPrint 0

let testYield =
     [
         yield 1
         yield 2
         yield! {3..5}
         yield 6
         yield 7
     ]

