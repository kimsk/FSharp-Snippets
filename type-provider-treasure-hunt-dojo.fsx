#r "System.Xml.Linq.dll"
#r "packages/FSharp.Data.2.0.4/lib/net40/FSharp.Data.dll"
open FSharp.Data

// ------------------------------------------------------------------
// WORD #1
//
// Use the WorldBank type provider to get all countries in the 
// "North America" region, then find country "c" with the smallest
// "Life expectancy at birth, total (years)" value in the year 2000
// and return the first two letters of the "c.Code" property
// ------------------------------------------------------------------

// Create connection to the WorldBank service
let wb = WorldBankData.GetDataContext()
// Get specific indicator for a specific country at a given year
wb.Countries.``Czech Republic``.Indicators.``Population (Total)``.[2000]
// Get a list of countries in a specified region
wb.Regions.``Euro area``.Countries

let c = wb.Regions.``North America``.Countries
            |> Seq.minBy (fun c -> c.Indicators.``Life expectancy at birth, total (years)``.[2000])
let ``word#1`` = c.Code.Substring(0,2)   
// US

// ------------------------------------------------------------------
// WORD #2
//
// Read the RSS feed in "data/bbc.xml" using XmlProvider and return
// the last word of the title of an article that was published at
// 9:05am (the date does not matter, just hours & minutes)
// ------------------------------------------------------------------

// Create a type for working with XML documents based on a sample file
type Sample = XmlProvider<"data/Writers.xml">
// Load the sample document - explore properties using "doc."
let doc = Sample.GetSample()

open System
//let bbcFile = File.ReadAllText(@"C:\Users\Karlkim\Documents\GitHub\Dojo-Type-Provider-Treasure-Hunt\Dojo\data\bbc.xml")
//let bbc = Sample.Parse(bbcFile)
//
//bbc.Authors

type bbc = XmlProvider<"data/bbc.xml">
let bbcDoc = bbc.GetSample()
let title = 
    bbcDoc.Channel.Items
        |> Seq.filter (fun i -> i.PubDate.Hour = 9 && i.PubDate.Minute = 5)
        |> Seq.map (fun i -> i.Title)
        |> Seq.head
let lastWord = title.Split(' ') |> Seq.last
let ``word#2`` = lastWord
// all?

// ------------------------------------------------------------------
// WORD #3
//
// Get the ID of a director whose name contains "Haugerud" and then
// search for all movie credits where he appears. Then return the 
// second (last) word from the movie he directed (the resulting type
// has properties "Credits" and "Crew" - you need movie from the 
// Crew list (there is only one).
// ------------------------------------------------------------------

// Using The MovieDB REST API
// Make HTTP request to /3/search/person
let key = "6ce0ef5b176501f8c07c634dfa933cff"
let name = "craig"
let data = 
  Http.RequestString
    ( "http://api.themoviedb.org/3/search/person",
      query = [ ("query", "Haugerud"); ("api_key", key) ],
      headers = [ HttpRequestHeaders.Accept HttpContentTypes.Json ] )

// Parse result using JSON provider
// (using sample result to generate types)
type PersonSearch = JsonProvider<"data/personsearch.json">
let sample = PersonSearch.Parse(data)

let first = sample.Results |> Seq.head
first.Name

// Request URL: "http://api.themoviedb.org/3/person/<id>/movie_credits
// (You can remove the 'query' parameter because it is not needed here;
// you need to put the director's ID in place of the <id> part of the URL)

// Use JsonProvider with sample file "data/moviecredits.json" to parse
let data2 =
    Http.RequestString
        ( "http://api.themoviedb.org/3/person/1097147/movie_credits",
            query = [("api_key", key) ],
            headers = [ HttpRequestHeaders.Accept HttpContentTypes.Json ] )
type MovieCredit = JsonProvider<"data/moviecredits.json">
let credits = MovieCredit.Parse(data2)
credits.Crew |> Array.filter (fun crew -> crew.Job = "Director")

// ------------------------------------------------------------------
// WORD #4
//
// Use CsvProvider to parse the file "data/librarycalls.csv" which
// contains information about some PHP library calls (got it from the
// internet :-)). Note that the file uses ; as the separator.
//
// Then find row where 'params' is equal to 2 and 'count' is equal to 1
// and the 'name' column is longer than 6 characters. Return first such
// row and get the last word of the function name (which is separated
// by underscores). Make the word plural by adding 's' to the end.
// ------------------------------------------------------------------

// Demo - using CSV provider to read CSV file with custom separator
type Lib = CsvProvider<"data/mortalityny.tsv">
// Read the sample file - explore the collection of rows in "lib.Data"
let lib = new Lib()

type LibCall = CsvProvider<"data/librarycalls.csv", ";">
let libCall = new LibCall()
let row = libCall.Rows
            |> Seq.find (fun r -> r.``params`` = 2 && r.count = 1 && r.name.Length > 6)
            
let ``word#4`` = row.name.Split([|'_'|]) |> Seq.last
// type

// ------------------------------------------------------------------
// WORD #5
//
// Use Freebase type provider to find chemical element with 
// "Atomic number" equal to 36 and then return the 3rd and 2nd 
// letter from the *end* of its name (that is 5th and 6th letter
// from the start).
// ------------------------------------------------------------------

// Create connection to the Freebase service
let fb = FreebaseData.GetDataContext()

// Query stars in the science & technology category
// (You'll need "Science and Technology" and then "Chemistry")
query { for e in fb.``Science and Technology``.Astronomy.Stars do 
        where e.Distance.HasValue
        select (e.Name, e.Distance) } 
|> Seq.toList

let arr = 
    query { for e in fb.``Science and Technology``.Chemistry.``Chemical Elements`` do
            where (e.``Atomic number``.Value = 36)
            select e.Name } |> Array.ofSeq
let ``word#5`` =  (arr.[0].Substring(4,1)) + (arr.[0].Substring(5,1))
// to

// ------------------------------------------------------------------
// WORD #6
//
// Use CsvProvider to load the Titanic data set from "data/Titanic.csv"
// (using the default column separator) and find the name of a female
// passenger who was 19 years old and embarked in the prot marked "Q"
// Then return Substring(19, 3) from her name.
// ------------------------------------------------------------------

// Use CsvProvider with the "data/titanic.csv" file as a sample
type Titanic = CsvProvider<"data/titanic.csv">
let titanic = new Titanic()

let ``word#6`` = 
    titanic.Rows
    |> Seq.filter (fun r -> r.Age = 19. && r.Embarked = "Q" && r.Sex = "female" )    
    |> Seq.map (fun r -> r.Name.Substring(19,3))
// are

// ------------------------------------------------------------------
// WORD #7
//
// Using the same RSS feed as in Word #2 ("data/bbc.xml"), find
// item that contains "Duran Duran" in the title and return the
// 14th word from its description (split the description using ' '
// as the separator and get item at index 13).
// ------------------------------------------------------------------

// (...)
let ``word#7`` = 
    bbcDoc.Channel.Items
        |> Seq.filter (fun i -> i.Title.Contains("Duran Duran"))
        |> Seq.map (fun i -> i.Description.Split(' '))        
        |> Seq.nth 0
        |> Seq.nth 13
// your
