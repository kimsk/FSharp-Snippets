#r @"..\packages\FsCheck.1.0.4\lib\net45\FsCheck.dll"

open FsCheck

// Property-based testing is like an exploratory statement about the function
// Property-based testing <> functional/specificatio testing
// Property-based testing is about writing down assumptions about the system and explore assumptions with lots of data
// Property gets one parameter and returns bool or Property

// we define property of List.rev
// List.rev twice should give the same result
let property list = list = List.rev(List.rev list)

Check.Quick property

Check.Verbose property

let reverseReverseOfFloatsEqualOriginalList (l:float list) = l = List.rev (List.rev l)

// fail as nan <> nan
Check.Quick reverseReverseOfFloatsEqualOriginalList

let rec remove item = function
    | [] -> []
    | hd::tl when hd = item -> tl
    | hd::tl -> hd::(remove item tl)

let rec sort = function
    | [] -> []
    | l -> 
        let value = (List.min l)
        value::(sort(l |> remove value))

let sortingTwiceIsSameAsSortingOnce (list: int list) =
    (sort list) = sort (sort list)

let firstItemSmallest (list:int list) =
    (not list.IsEmpty) ==> // we don't check empty list for this property
        lazy((List.min list) = (List.head (sort list)))

let lastItemLargest (list:int list) =
    (not list.IsEmpty) ==>
        lazy((List.max list) = (List.head (sort list |> List.rev)))

let permutationOf (list:int list) =
    let exists item = List.exists ((=)item)
    let sorted = sort list

    (sorted.Length = list.Length)
        |@ "same length" // label
        .&. // AND
    (sorted |> List.forall(fun x -> list |> (exists x)))
        |@ "all elements exists"

let rec ordered (list:int list) =
    let rec _ordered = function
        | [] -> true
        | hd::[] -> true
        | fst::snd::tl -> (fst <= snd) && (_ordered (snd::tl))

    _ordered (sort list)

Check.Quick sortingTwiceIsSameAsSortingOnce
Check.Quick firstItemSmallest
Check.Quick lastItemLargest
Check.Quick permutationOf
Check.Quick ordered

// Unit Test
type ``sorting a list of numbers`` =
    static member ``once is same as twice``() =
        sortingTwiceIsSameAsSortingOnce
    static member ``first item is the smallest``() =
        firstItemSmallest
    static member ``last item is the largest``() =
        lastItemLargest
    static member ``has same numbers as original``() =
        permutationOf
    static member ``is ordered result``() =
        ordered

// check all
Check.QuickAll typeof<``sorting a list of numbers``>

