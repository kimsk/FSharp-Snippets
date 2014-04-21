#I "packages/FSharp.Charting.0.90.6"
#I "packages/Deedle.0.9.12"
#load "FSharp.Charting.fsx"
#load "Deedle.fsx"

open System
open Deedle
open FSharp.Charting

// Create from sequence of keys and sequence of values
let dates  = 
  [ DateTime(2013,1,1); 
    DateTime(2013,1,4); 
    DateTime(2013,1,8) ]
let values = [ 10.0; 20.0; 30.0 ]
let first = Series(dates, values)

// Create from a single list of observations
Series.ofObservations
  [ DateTime(2013,1,1) => 10.0
    DateTime(2013,1,4) => 20.0
    DateTime(2013,1,8) => 30.0 ]

// Shorter alternative to 'Series.ofObservations'
series [ 1 => 1.0; 2 => 2.0 ]

// Create series with implicit (ordinal) keys
Series.ofValues [ 10.0; 20.0; 30.0 ]
// OR
// 

/// Generate date range from 'first' with 'count' days
let dateRange (first:System.DateTime) count = seq { for i in 0..(count-1) -> first.AddDays(float i)}
/// Generate 'count' number of random doubles
let rand count = 
    let rnd = System.Random()
    seq { for i in 0..(count-1) -> rnd.NextDouble()}
// A series with values for 10 days 
let second = Series(dateRange (DateTime(2013,1,1)) 10, rand 10)

let df1 = Frame(["first"; "second"], [first; second])

// The same as previously
let df2 = Frame.ofColumns ["first" => first; "second" => second]

// Transposed - here, rows are "first" and "second" & columns are dates
let df3 = Frame.ofRows ["first" => first; "second" => second]

// Create from individual observations (row * column * value)
let df4 = 
  [ ("Monday", "Tomas", 1.0); ("Tuesday", "Adam", 2.1)
    ("Tuesday", "Tomas", 4.0); ("Wednesday", "Tomas", -5.4) ]
  |> Frame.ofValues

df4.Rows.["Monday"]?Tomas

// Assuming we have a record 'Price' and a collection 'values'
type Price = { Day : DateTime; Open : float }
let prices = 
  [ { Day = DateTime.Now; Open = 10.1 }
    { Day = DateTime.Now.AddDays(1.0); Open = 15.1 }
    { Day = DateTime.Now.AddDays(2.0); Open = 9.1 } ]

// Creates a data frame with columns 'Day' and 'Open'
let df5 = Frame.ofRecords prices

let msftCsv = Frame.ReadCsv(__SOURCE_DIRECTORY__ + "/data/MSFT.csv")
let fbCsv = Frame.ReadCsv(__SOURCE_DIRECTORY__ + "/data/FB.csv")

// change indexRows
let msftOrd = 
    msftCsv
    |> Frame.indexRowsDate "Date"
    |> Frame.orderRows

// choose columns, Open and Close
let msft = msftOrd.Columns.[["Open";"Close"]]
// or
// msftOrd |> Frame.getCols["Open";"Close"]

// add new column
msft?Difference <- msft?Open - msft?Close
// OR
// msft.Columns.["Difference"]
// msft$Difference in R

let fb = 
    fbCsv
    |> Frame.indexRowsDate "Date"
    |> Frame.orderRows
    |> Frame.getCols["Open";"Close"]
fb?Difference <- fb?Open - fb?Close

Chart.Combine 
    [
        Chart.Line(msft?Difference |> Series.observations)
        Chart.Line(fb?Difference |> Series.observations)
    ]

// change index column names
let msftNames = ["MsftOpen"; "MsftClose"; "MsftDiff"]
let msftRen = msft |> Frame.indexColsWith msftNames

let fbNames = ["FbOpen"; "FbClose"; "FbDiff"]
let fbRen = fb |> Frame.indexColsWith fbNames

// outer join (align & fill w/ missing values)
let joinedOut = msftRen.Join(fbRen, kind=JoinKind.Outer)

// inner join (remove rows w/ missing values)
let joinedIn = msftRen.Join(fbRen, kind=JoinKind.Inner)

Chart.Rows 
    [
        Chart.Line(joinedIn?MsftDiff |> Series.observations)
        Chart.Line(joinedIn?FbDiff |> Series.observations)
    ]

// Look for a row
joinedIn.Rows.[DateTime(2013,1,2)] // ObjectSeries<string>
joinedIn.Rows.[DateTime(2013,1,2)]?FbOpen
joinedIn.Rows.[DateTime(2013,1,2)].GetAs<int>("FbOpen")

// look for rows
let janDates = [ for d in 2..4 -> DateTime(2013, 1, d)]
let jan234 = joinedIn.Rows.[janDates]

// calculate mean
jan234?MsftOpen |> Series.mean

let jan = joinedIn.Rows.[DateTime(2013,1,1)..DateTime(2013,1,31)]
jan?FbOpen |> Series.mean
jan?MsftOpen |> Series.mean

// ordered time series
let daysSeries = Series(dateRange DateTime.Today 10, rand 10)
let obsSeries = Series(dateRange DateTime.Now 10, rand 10)

daysSeries.Get(DateTime.Now, Lookup.NearestSmaller)
obsSeries.Get(DateTime.Today, Lookup.NearestSmaller)

let daysFrame = [1 => daysSeries] |> Frame.ofColumns
let obsFrame = [2 => obsSeries] |> Frame.ofColumns

// exact join
let obsDaysExact = daysFrame.Join(obsFrame, kind=JoinKind.Left)

// join with nearest smaller
let obsDaysPrev =
    (daysFrame, obsFrame)
    ||> Frame.joinAlign JoinKind.Left Lookup.NearestSmaller

let obsDaysNext =
    (daysFrame, obsFrame)
    ||> Frame.joinAlign JoinKind.Left Lookup.NearestGreater

joinedOut?Comparison <- joinedOut 
    |> Frame.mapRowValues (fun row ->
            if row?MsftOpen > row ? FbOpen then "MSFT" else "FB"
        )

// 
joinedOut.GetSeries<string>("Comparison")
    |> Series.filterValues((=)"MSFT") |> Series.countValues

joinedOut.GetSeries<string>("Comparison")
    |> Series.filterValues((=)"FB") |> Series.countValues

let joinedOpens = joinedOut.Columns.[["MsftOpen"; "FbOpen"]]

// RowsDense only returns rows that have no missing values.
joinedOpens.RowsDense
    |> Series.filterValues (fun row -> row?MsftOpen > row?FbOpen)
    |> Series.countValues

let monthly =
    joinedIn
    |> Frame.groupRowsUsing (fun k _ -> DateTime(k.Year, k.Month, 1))

monthly.Rows.[DateTime(2013, 5, 1), *] |> Frame.mean

monthly |> Frame.meanLevel fst

monthly.RowKeys