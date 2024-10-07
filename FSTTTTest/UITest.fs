module FSTTT.UITest

open Xunit
open System.IO
open FSTTT.UI
open FSTTT.Board

[<Fact>]
let ``updateBoard places X at position 5`` () =
    let grid = [| "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9" |]
    let expectedGrid = [| "1"; "2"; "3"; "4"; "X"; "6"; "7"; "8"; "9" |]

    let updatedGrid = updateBoard grid 5 "X"

    Assert.Equal<string[]>(expectedGrid, updatedGrid)

[<Fact>]
let ``updateBoard places O at position 9`` () =
    let grid = [| "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9" |]
    let expectedGrid = [| "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "O" |]

    let updatedGrid = updateBoard grid 9 "O"

    Assert.Equal<string[]>(expectedGrid, updatedGrid)

[<Fact>]
let ``updateBoard places O at position 3`` () =
    let grid = [| "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9" |]
    let expectedGrid = [| "1"; "2"; "O"; "4"; "5"; "6"; "7"; "8"; "9" |]

    let updatedGrid = updateBoard grid 3 "O"

    Assert.Equal<string[]>(expectedGrid, updatedGrid)

[<Fact>]
let ``isValidMove is true for valid move`` () =
    let board = [| "1"; "2"; "3"; "4"; "X"; "6"; "7"; "8"; "9" |]
    let pos = 3
    Assert.True(isValidMove board pos)

[<Fact>]
let ``isValidMove is false for occupied position`` () =
    let board = [| "1"; "2"; "3"; "4"; "X"; "6"; "7"; "8"; "9" |]
    let pos = 5
    Assert.False(isValidMove board pos)

[<Fact>]
let ``isValidMove is false for out of range position, higher`` () =
    let board = [| "1"; "2"; "3"; "4"; "X"; "6"; "7"; "8"; "9" |]
    let pos = 10
    Assert.False(isValidMove board pos)

[<Fact>]
let ``isValidMove is false for out of range position, lower`` () =
    let board = [| "1"; "2"; "3"; "4"; "X"; "6"; "7"; "8"; "9" |]
    let pos = 0
    Assert.False(isValidMove board pos)

[<Fact>]
let ``getMove makes a move`` () =
    let grid = [| "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9" |]
    let expected = 5
    let ref = ref [ string expected ]
    let output = new StringWriter()

    let got = getHumanMove (Test(output, ref)) grid

    Assert.Equal(expected, got)

    let result = output.ToString()
    Assert.Contains("Select a position (1-9):", result)

[<Fact>]
let ``getMove doesn't allow 5 but allows 9`` () =
    let board = [| "1"; "2"; "3"; "4"; "X"; "6"; "7"; "8"; "9" |]
    let output = new StringWriter()

    let ref = ref [ "5"; "9" ]

    let behavior = Test(output, ref)
    let move = getHumanMove behavior board

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
    let movesRef = ref [ "alien"; "cat"; "human" ]
    let behavior = Test(output, movesRef)

    let playerType = askPlayerKind behavior "Player 1"

    Assert.Equal(Human, playerType)

    let result = output.ToString()
    Assert.Contains("Is Player 1 a Human or AI?", result)
    Assert.Contains("Is Player 1 a Human or AI?", result)
    Assert.Contains("Is Player 1 a Human or AI?", result)
