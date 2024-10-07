module FSTTT.UITest

open Xunit
open System.IO
open FSTTT.UI

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
let ``isValidMove is false for out of range position`` () =
    let board = [| "1"; "2"; "3"; "4"; "X"; "6"; "7"; "8"; "9" |]
    let pos = 10
    Assert.False(isValidMove board pos)
    
[<Fact>]
let ``getMove makes a move`` () =
    let grid = [| "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9" |]
    let expected = 5
    let ref = ref [expected]
    let output = new StringWriter()
    
    let got = getMove (Test (output, ref)) grid

    Assert.Equal(expected, got)
    
    let result = output.ToString()
    Assert.Contains("Select a position (1-9):", result)
    
[<Fact>]
let ``getMove doesn't allow 5 but allows 9`` () =
    let board = [| "1"; "2"; "3"; "4"; "X"; "6"; "7"; "8"; "9" |]
    let output = new StringWriter()
    
    let ref = ref [5 ; 9]
    
    let behavior = Test (output, ref)
    let move = getMove behavior board
    
    Assert.Equal(9, move) 
    
    let result = output.ToString()
    Assert.Contains("Select a position (1-9):", result)
    Assert.Contains("Please try again.", result)
    
[<Fact>]
let ``getMove asks again for out-of-range move`` () =
    let grid = [| "1"; "2"; "3"; "4"; "X"; "6"; "7"; "8"; "9" |]
    let output = new StringWriter()
    let simulatedMoves = ref [10; 7]  

    let got = getMove (Test (output, simulatedMoves)) grid
    
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