// http://fssnip.net/cr
// http://www.eveandersson.com/pi/monte-carlo-circle
open System

/// Calculates whether a point is within the unit circle
let insideCircle (x:float, y:float) =   
  if (x * x) + (y * y) < 1.0 then true
  else false

/// Generates random X, Y values between 0 and 1
let randomPoints max = seq {
  let rnd = new Random()
  for i in 0 .. max do
    yield rnd.NextDouble(), rnd.NextDouble() }

/// Generate specified number of random points and
/// calculate PI using Monte Carlo simulation
let monteCarloPI size = 
  // TASK #2: Generate specified number of random
  // points and test how many are inside circle
  // (...)
  
  let inside = 
    randomPoints size   
      |> Seq.filter (fun (x,y) -> insideCircle(x,y))
      |> Seq.length
  
  // Estimate the value of PI
  float inside / float size * 4.0

// Test the Monte Carlo PI calculation
#time
monteCarloPI 1000000

// Run the calculation 10 times and calculate average
// Change to 'Array.Parallel.map' to parallelize!
[| for i in 0 .. 10 -> 1000000 |]
|> Array.map monteCarloPI
|> Array.average