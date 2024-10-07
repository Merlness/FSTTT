module FSTTT.MainTest

open Xunit
open System.IO
open FSTTT.Main
open FSTTT.UI

let initialGrid = [| "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9" |]

[<Fact>]
let ``displayInitialBoard shows greeting and initial board`` () =
    let output = new StringWriter()
    let grid = [| "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9" |]

    displayGreeting (Test(output, ref [])) grid

    let result = output.ToString()
    Assert.Contains("Welcome to Tic Tac Toe!", result)
    Assert.Contains("1 | 2 | 3\n4 | 5 | 6\n7 | 8 | 9", result)


[<Fact>]
let ``playGame prints the board after a valid move`` () =
    let output = new StringWriter()
    let behavior = Test(output, ref [ 3 ])

    let newBoard, _ = playTurn behavior initialGrid "X"

    let result = output.ToString()

    Assert.Contains("1 | 2 | X\n4 | 5 | 6\n7 | 8 | 9", result)

[<Fact>]
let ``gameOver is true when board is full`` () =
    let fullBoard = [| "X"; "O"; "X"; "O"; "X"; "O"; "O"; "X"; "O" |]
    let result = isGameOver fullBoard
    Assert.True(result)

[<Fact>]
let ``isGameOver is false when game is not over`` () =
    let grid = [| "X"; "2"; "3"; "O"; "5"; "6"; "7"; "O"; "9" |]
    let result = isGameOver grid
    Assert.False(result)

[<Fact>]
let ``playGame ends in a tie when board is full`` () =
    let output = new StringWriter()
    let behavior = Test(output, ref [ 1; 4; 2; 5; 6; 3; 7; 8; 9 ])

    playGame behavior initialGrid "X"
    let result = output.ToString()

    Assert.Contains("Womp, it's a tie", result)

[<Fact>]
let ``gameOver returns true when X wins`` () =
    let grid = [| "X"; "X"; "X"; "4"; "5"; "6"; "7"; "8"; "9" |]
    let result = isGameOver grid
    Assert.True(result)

[<Fact>]
let ``gameOver returns true when O wins`` () =
    let grid = [| "O"; "X"; "X"; "4"; "O"; "6"; "7"; "8"; "O" |]
    let result = isGameOver grid
    Assert.True(result)

[<Fact>]
let ``gameOver returns true when tie`` () =
    let grid = [| "X"; "O"; "X"; "O"; "O"; "X"; "X"; "X"; "O" |]
    let result = isGameOver grid
    Assert.True(result)

[<Fact>]
let ``gameOver returns false when board is not full and no winner`` () =
    let grid = [| "1"; "2"; "X"; "O"; "5"; "6"; "7"; "8"; "O" |]
    let result = isGameOver grid
    Assert.False(result)

[<Fact>]
let ``playGame ends when X wins`` () =
    let output = new StringWriter()
    let behavior = Test(output, ref [ 7; 4; 8; 5; 9 ])

    playGame behavior initialGrid "X"
    let result = output.ToString()

    Assert.Contains("X is the winner!", result)

[<Fact>]
let ``playGame ends when O wins`` () =
    let output = new StringWriter()
    let behavior = Test(output, ref [ 1; 3; 2; 6; 7; 9 ])

    playGame behavior initialGrid "X"
    let result = output.ToString()

    Assert.Contains("O is the winner!", result)
