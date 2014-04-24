#r @"..\packages\numl.0.7.6\lib\net40\numl.dll"
#r @"..\TennisData\bin\Debug\TennisData.dll"

open TennisData
open numl
open numl.Model
open numl.Supervised.DecisionTree
open numl.Supervised.Regression
let data = Tennis.GetData() |> Seq.cast<obj>

let d = Descriptor.Create<Tennis>()
let g = DecisionTreeGenerator(d)
g.SetHint(false)

let model = Learner.Learn(data, 0.8, 1000, g)
printfn "%A" model

let tennis1 = Tennis(Outlook = Outlook.Sunny, Temperature = Temperature.Low, Windy = false)
let play1 = model.Model.Predict<Tennis>(tennis1).Play

let tennis2 = Tennis(Outlook = Outlook.Rainy, Temperature = Temperature.Low, Windy = true)
let play2 = model.Model.Predict<Tennis>(tennis2).Play

printfn "%A" play2


