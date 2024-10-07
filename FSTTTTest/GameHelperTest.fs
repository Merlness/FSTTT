module FSTTTTest.GameHelperTest

open Xunit
open System.IO
open FSTTT.GameHelper
open FSTTT.Board
open FSTTT.UI

let initialGrid = [| "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9" |]


[<Fact>]
let ``countAvailableMoves returns 9 when the board is empty`` () =
    let grid = [| "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9" |]
    let expectedCount = 9  
    let result = countAvailableMoves grid
    Assert.Equal(expectedCount, result)

[<Fact>]
let ``countAvailableMoves returns 0 when the board is full`` () =
    let grid = [| "X"; "O"; "X"; "O"; "X"; "O"; "X"; "O"; "X" |]
    let expectedCount = 0  
    let result = countAvailableMoves grid
    Assert.Equal(expectedCount, result)
    
[<Fact>]
let ``countAvailableMoves returns correct count of available positions`` () =
    let grid = [| "X"; "O"; "3"; "O"; "5"; "X"; "7"; "X"; "O" |]
    let expectedCount = 3  
    let result = countAvailableMoves grid
    Assert.Equal(expectedCount, result)

[<Fact>]
let ``isPlayerOnesTurn returns true when new game`` () =
    let grid = [| "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9" |]
    let result = isPlayerOnesTurn grid
    Assert.True(result)  

[<Fact>]
let ``isPlayerOnesTurn returns false after first move`` () =
    let grid = [| "X"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9" |]
    let result = isPlayerOnesTurn grid
    Assert.False(result)
    
[<Fact>]
let ``isPlayerOnesTurn returns true after 2 moves`` () =
    let grid = [| "1"; "2"; "3"; "O"; "5"; "6"; "7"; "8"; "X" |]
    let result = isPlayerOnesTurn grid
    Assert.True(result)


[<Fact>]
let ``getMove for human player returns valid move`` () =
    let output = new StringWriter()
    let simulatedMoves = ref ["5"] 
    let behavior = Test (output, simulatedMoves)
    let initialGrid = [| "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9" |] 
    let game = { Player1 = Human; Player2 = Human; Token1 = "X"; Token2 = "O" }

    let move = getMove behavior initialGrid game

    Assert.Equal(5, move) 
    let result = output.ToString()
    Assert.Contains("Select a position (1-9):", result)
    
[<Fact>]
let ``getMove for AI player returns valid move`` () =
    let output = new StringWriter()
    let initialGrid = [| "X"; "X"; "3"; "4"; "O"; "6"; "7"; "O"; "9" |] 
    let game = { Player1 = AI; Player2 = Human; Token1 = "X"; Token2 = "O" }

    let move = getMove (Test(output, ref [])) initialGrid game

    Assert.Equal(move, 3) 
    let result = output.ToString()
    Assert.Contains("AI's move", result)

[<Fact>]
let ``getMove alternates between players`` () =
    let output = new StringWriter()
    let simulatedMoves = ref ["5"; "3"] 
    let behavior = Test (output, simulatedMoves)
    let initialGrid = [| "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9" |] 
    let game = { Player1 = Human; Player2 = Human; Token1 = "X"; Token2 = "O" }

    let move1 = getMove behavior initialGrid game
    Assert.Equal(5, move1) 

    let updatedGrid = updateBoard initialGrid move1 game.Token1
    let move2 = getMove behavior updatedGrid game
    Assert.Equal(3, move2)

    let result = output.ToString()
    Assert.Contains("Select a position (1-9):", result)

[<Fact>]
let ``getMove re-prompts after invalid move`` () =
    let output = new StringWriter()
    let simulatedMoves = ref ["5"; "9"] 
    let behavior = Test (output, simulatedMoves)
    let initialGrid = [| "1"; "2"; "3"; "4"; "X"; "6"; "7"; "8"; "9" |] 
    let game = { Player1 = Human; Player2 = Human; Token1 = "X"; Token2 = "O" }

    let move = getMove behavior initialGrid game

    Assert.Equal(9, move) 
    let result = output.ToString()
    Assert.Contains("Please try again.", result)
    Assert.Contains("Select a position (1-9):", result)
    
[<Fact>]
let ``getMove for AI selects best move`` () =
    let output = new StringWriter()
    let initialGrid = [| "X"; "O"; "X"; "O"; "5"; "O"; "X"; "8"; "9" |] 
    let game = { Player1 = AI; Player2 = Human; Token1 = "X"; Token2 = "O" }

    let move = getMove (Test(output, ref [])) initialGrid game

    Assert.InRange(move, 1, 9)
    let result = output.ToString()
    Assert.Contains("AI's move", result)

