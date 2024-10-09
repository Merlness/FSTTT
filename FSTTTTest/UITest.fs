module FSTTT.UITest

open Xunit
open System
open System.IO
open FSTTT.UI
open FSTTT.Board

[<Fact>]
let ``updateBoard places X at position 5`` () =
    let expectedGrid = [| "1"; "2"; "3"; "4"; "X"; "6"; "7"; "8"; "9" |]

    let updatedGrid = updateBoard initialGrid 5 "X"

    Assert.Equal<string[]>(expectedGrid, updatedGrid)

[<Fact>]
let ``updateBoard places O at position 9`` () =
    let expectedGrid = [| "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "O" |]

    let updatedGrid = updateBoard initialGrid 9 "O"

    Assert.Equal<string[]>(expectedGrid, updatedGrid)

[<Fact>]
let ``updateBoard places O at position 3`` () =
    let expectedGrid = [| "1"; "2"; "O"; "4"; "5"; "6"; "7"; "8"; "9" |]

    let updatedGrid = updateBoard initialGrid 3 "O"

    Assert.Equal<string[]>(expectedGrid, updatedGrid)

[<Fact>]
let ``isValidMove is true for valid move`` () =
    let grid = [| "1"; "2"; "3"; "4"; "X"; "6"; "7"; "8"; "9" |]
    let pos = 3
    Assert.True(isValidMove grid pos)

[<Fact>]
let ``isValidMove is false for occupied position`` () =
    let grid = [| "1"; "2"; "3"; "4"; "X"; "6"; "7"; "8"; "9" |]
    let pos = 5
    Assert.False(isValidMove grid pos)

[<Fact>]
let ``isValidMove is false for out of range position, higher`` () =
    let grid = [| "1"; "2"; "3"; "4"; "X"; "6"; "7"; "8"; "9" |]
    let pos = 10
    Assert.False(isValidMove grid pos)

[<Fact>]
let ``isValidMove is false for out of range position, lower`` () =
    let grid = [| "1"; "2"; "3"; "4"; "X"; "6"; "7"; "8"; "9" |]
    let pos = 0
    Assert.False(isValidMove grid pos)

[<Fact>]
let ``getMove makes a move`` () =
    let expected = 5
    let ref = ref [ string expected ]
    let output = new StringWriter()

    let got = getHumanMove (Test(output, ref)) initialGrid

    Assert.Equal(expected, got)

    let result = output.ToString()
    Assert.Contains("Select a position (1-9):", result)

[<Fact>]
let ``getMove doesn't allow 5 but allows 9`` () =
    let grid = [| "1"; "2"; "3"; "4"; "X"; "6"; "7"; "8"; "9" |]
    let output = new StringWriter()

    let ref = ref [ "5"; "9" ]

    let behavior = Test(output, ref)
    let move = getHumanMove behavior grid

    Assert.Equal(9, move)

    let result = output.ToString()
    Assert.Contains("Select a position (1-9):", result)
    Assert.Contains("Please try again.", result)

[<Fact>]
let ``getMove asks again for out-of-range move`` () =
    let grid = [| "1"; "2"; "3"; "4"; "X"; "6"; "7"; "8"; "9" |]
    let output = new StringWriter()
    let simulatedMoves = ref [ "10"; "7" ]

    let got = getHumanMove (Test(output, simulatedMoves)) grid

    Assert.Equal(7, got)

    let result = output.ToString()
    Assert.Contains("Select a position (1-9):", result)
    Assert.Contains("Please try again.", result)

[<Fact>]
let ``endgameResult returns message when X wins`` () =
    let grid = [| "X"; "X"; "X"; "4"; "5"; "6"; "7"; "8"; "9" |]
    let result = endgameResult grid "X" "O"
    Assert.Equal("X is the winner!", result)

[<Fact>]
let ``endgameResult returns message when O wins`` () =
    let grid = [| "1"; "2"; "O"; "X"; "O"; "X"; "O"; "8"; "9" |]
    let result = endgameResult grid "X" "O"
    Assert.Equal("O is the winner!", result)

[<Fact>]
let ``endgameResult returns tie message when tied`` () =
    let grid = [| "X"; "O"; "X"; "O"; "O"; "X"; "X"; "X"; "O" |]
    let result = endgameResult grid "X" "O"
    Assert.Equal("Womp, it's a tie!", result)

[<Fact>]
let ``askPlayerToken prompts for token selection`` () =
    let output = new StringWriter()
    let simulatedMoves = ref [ "X" ]
    let behavior = Test(output, simulatedMoves)

    let token = askPlayerToken behavior

    Assert.Equal("X", token)
    let result = output.ToString()
    Assert.Contains("Choose a token for Player 1 (X or O):", result)

[<Fact>]
let ``askPlayerToken re-prompts until valid token is chosen`` () =
    let output = new StringWriter()
    let movesRef = ref [ "Z"; "P"; "X" ]
    let behavior = Test(output, movesRef)

    let token = askPlayerToken behavior

    Assert.Equal("X", token)

    let result = output.ToString()
    Assert.Contains("Choose a token for Player 1 (X or O):", result)
    Assert.Contains("Choose a token for Player 1 (X or O):", result)
    Assert.Contains("Choose a token for Player 1 (X or O):", result)

[<Fact>]
let ``askPlayerKind prompts for player type`` () =
    let output = new StringWriter()
    let simulatedMoves = ref [ "human" ]
    let behavior = Test(output, simulatedMoves)

    let playerType = askPlayerKind behavior "Player 1"

    Assert.Equal(Human, playerType)
    let result = output.ToString()
    Assert.Contains("Is Player 1 a Human or AI?", result)

[<Fact>]
let ``askPlayerKind re-prompts until valid player type is chosen`` () =
    let output = new StringWriter()
    let movesRef = ref [ "alien"; "cat"; "ai" ]
    let behavior = Test(output, movesRef)

    let playerType = askPlayerKind behavior "Player 1"

    Assert.Equal(AI, playerType)

    let result = output.ToString()
    Assert.Contains("Is Player 1 a Human or AI?", result)
    Assert.Contains("Is Player 1 a Human or AI?", result)
    Assert.Contains("Is Player 1 a Human or AI?", result)


[<Fact>]
let ``readInput reads valid integer from console`` () =
    let input = new StringReader("5\n")
    Console.SetIn(input)  
    
    let result = readInput Console
    
    Assert.Equal(Some 5, result)

[<Fact>]
let ``readInput returns None for invalid console input`` () =
    let input = new StringReader("\n")
    Console.SetIn(input)  
    
    let result = readInput Console
    
    Assert.Equal(None, result)

[<Fact>]
let ``readInput returns None for test mode input`` () =
    let simulatedMoves = ref ["invalid"]  
    let behavior = Test (new StringWriter(), simulatedMoves)
    
    let result = readInput behavior
    
    Assert.Equal(None, result)


[<Fact>]
let ``readInput returns None when ref is empty`` () =
    let simulatedMoves = ref []  
    let behavior = Test (new StringWriter(), simulatedMoves)

    let result = readInput behavior

    Assert.Equal(None, result)
    
[<Fact>]
let ``handleInput processes input from Console`` () =
    let simulatedConsoleInput = "5"
    let inputStream = new StringReader(simulatedConsoleInput)
    System.Console.SetIn(inputStream)

    let processInput (input: string) =
        match input with
        | "5" -> 5
        | _ -> failwith "Unexpected input"

    let fallback () = 0 
    let result = handleInput Console processInput fallback

    Assert.Equal(5, result)


[<Fact>]
let ``handleSimulatedInput triggers fallback when no inputs remain`` () =
    let simulatedMoves = ref []

    let processInput (input: string) =
        failwith ""

    let fallback () = -1

    let result = handleSimulatedInput simulatedMoves processInput fallback

    Assert.Equal(-1, result)
