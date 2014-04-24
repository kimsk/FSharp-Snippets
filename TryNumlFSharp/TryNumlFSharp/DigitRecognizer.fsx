#r @"..\packages\numl.0.7.6\lib\net40\numl.dll"
#r @"..\TennisData\bin\Debug\TennisData.dll"

open TennisData
open numl
open numl.Model
open numl.Supervised
open numl.Supervised.DecisionTree
open numl.Supervised.NaiveBayes
let data = Stuff.GetRecords() |> Seq.cast<obj>

let d = Descriptor.Create<DigitRecognizerRecord>()

let getDecisionTreeModel description (data:seq<obj>) =
    let g = DecisionTreeGenerator(description)
    g.SetHint(false)
    let model = Learner.Learn(data, 0.8, 1000, g).Model
    printfn "%A" model
    model

let getNaiveBayesModel description (data:seq<obj>) =
    let g = NaiveBayesGenerator(2)
    g.Generate(description, data)

let predict (model:IModel) pixels =
    let r = DigitRecognizerRecord(Pixels = pixels)
    model.Predict<DigitRecognizerRecord>(r).Digit

let dtModel = getDecisionTreeModel d data
let nbModel = getNaiveBayesModel d data

let tests = [|
    Stuff.Pixels.[Digit.One]
    Stuff.Pixels.[Digit.Two]
    Stuff.Pixels.[Digit.Three]
    Stuff.Pixels.[Digit.Four]
    Stuff.Pixels.[Digit.Five]
    Stuff.Pixels.[Digit.Six]
    Stuff.Pixels.[Digit.Seven]
    Stuff.Pixels.[Digit.Eight]
    Stuff.Pixels.[Digit.Nine]
    Stuff.Pixels.[Digit.Six]
|] 

tests |> Array.map (fun d -> predict dtModel d)
tests|> Array.map (fun d -> predict nbModel d)