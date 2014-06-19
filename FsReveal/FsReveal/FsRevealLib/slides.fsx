(**
## F# syntax in 60 seconds

The two major differences between F# syntax and a standard C-like syntax are:

- Curly braces are not used to delimit blocks of code. Instead, indentation is used (Python is similar this way).
- Whitespace is used to separate parameters rather than commas.
*)
// The "let" keyword defines an (immutable) value
let myInt = 5
let myFloat = 3.14
let myString = "hello"   //note that no types needed

// ======== Lists ============
let twoToFive = [2;3;4;5]        // Square brackets create a list with
                                 // semicolon delimiters.
let oneToFive = 1 :: twoToFive   // :: creates list with new 1st element
// The result is [1;2;3;4;5]
let zeroToFive = [0;1] @ twoToFive   // @ concats two lists
