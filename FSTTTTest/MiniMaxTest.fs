module FSTTTTest.MiniMaxTest

open Xunit
open FSTTT.MiniMax
open FSTTT.Board

let gameResult (grid: string[]) (maximizingToken: string) (minimizingToken: string) : string option =
    if checkWinner grid maximizingToken then Some "win"
    elif checkWinner grid minimizingToken then Some "loss"
    elif isFull grid then Some "tie"
    else None

let placeAIMove (board: string[]) (maximizingToken: string) (minimizingToken: string) : string[] =
    let _, move = maximize board maximizingToken minimizingToken 1
    updateBoard board move maximizingToken

let rec collectAIResults
    (results: string list)
    (board: string[])
    (maximizingToken: string)
    (minimizingToken: string)
    : string list =
    let boardAfterAIMove = placeAIMove board maximizingToken minimizingToken

    match gameResult boardAfterAIMove maximizingToken minimizingToken with
    | Some result -> result :: results
    | None ->
        playGameComputerSecond boardAfterAIMove maximizingToken minimizingToken
        @ results

and playGameComputerSecond (board: string[]) (maximizingToken: string) (minimizingToken: string) : string list =
    let availableMoves = availableMoves board
    let mutable results = []

    for move in availableMoves do
        results <- collectMoveResults results move board maximizingToken minimizingToken

    results

and collectMoveResults
    (results: string list)
    (move: int)
    (board: string[])
    (maximizingToken: string)
    (minimizingToken: string)
    : string list =
    let newBoard = updateBoard board move minimizingToken

    match gameResult newBoard maximizingToken minimizingToken with
    | Some result -> result :: results
    | None -> collectAIResults results newBoard maximizingToken minimizingToken

let playGameComputerFirst (board: string[]) (maximizingToken: string) (minimizingToken: string) : string list =
    playGameComputerSecond (placeAIMove board maximizingToken minimizingToken) maximizingToken minimizingToken


[<Fact>]
let ``valueMax should be -100000`` () = Assert.Equal(-100000, valueMax)

[<Fact>]
let ``valueMin should be 100000`` () = Assert.Equal(100000, valueMin)

[<Fact>]
let ``initialAction should be -1`` () = Assert.Equal(-1, initialAction)

[<Fact>]
let ``maximize selects the last and only move`` () =
    let grid = [| "X"; "O"; "X"; "O"; "X"; "O"; "O"; "X"; "9" |]
    let _, result = maximize grid "X" "O" 1
    Assert.Equal(9, result)

[<Fact>]
let ``maximize selects the second to last move`` () =
    let grid = [| "1"; "2"; "X"; "X"; "X"; "O"; "O"; "X"; "O" |]
    let _, result = maximize grid "X" "O" 1
    Assert.Equal(2, result)

[<Fact>]
let ``maximize selects the winning move for X`` () =
    let grid = [| "X"; "O"; "X"; "O"; "X"; "6"; "O"; "X"; "9" |]
    let _, result = maximize grid "X" "O" 1
    Assert.Equal(9, result)

[<Fact>]
let ``maximize blocks end move for O`` () =
    let grid = [| "X"; "2"; "3"; "O"; "X"; "6"; "O"; "8"; "9" |]
    let _, result = maximize grid "X" "O" 1
    Assert.Equal(9, result)

[<Fact>]
let ``maximize blocks end move for O second test`` () =
    let grid = [| "O"; "2"; "3"; "X"; "O"; "6"; "X"; "8"; "9" |]
    let _, result = maximize grid "X" "O" 1
    Assert.Equal(9, result)

[<Fact>]
let ``maximize chooses between two moves`` () =
    let grid = [| "X"; "O"; "X"; "O"; "X"; "6"; "7"; "8"; "9" |]
    let _, result = maximize grid "X" "O" 1
    Assert.Contains(result, [ 7; 9 ])

[<Fact>]
let ``maximize finds optimal blocking move for X`` () =
    let grid = [| "1"; "2"; "3"; "O"; "X"; "X"; "O"; "8"; "9" |]
    let _, result = maximize grid "X" "O" 1
    Assert.Equal(1, result)

[<Fact>]
let ``maximize finds winning move for O`` () =
    let grid = [| "1"; "2"; "3"; "O"; "X"; "X"; "O"; "8"; "9" |]
    let _, result = maximize grid "O" "X" 1
    Assert.Equal(1, result)

[<Fact>]
let ``maximize finds optimal move for X`` () =
    let grid = [| "X"; "2"; "3"; "O"; "5"; "6"; "7"; "8"; "9" |]
    let _, result = maximize grid "X" "O" 1
    Assert.Equal(2, result)

[<Fact>]
let ``maximize finds first move for O`` () =
    let grid = [| "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9" |]
    let _, result = maximize grid "O" "X" 1
    Assert.Contains(result, [ 1; 3; 7; 9 ])

[<Fact>]
let ``minimax AI going first never loses`` () =
    let results =
        playGameComputerFirst [| "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9" |] "X" "O"

    Assert.DoesNotContain("loss", results)

[<Fact>]
let ``minimax AI going second never loses`` () =
    let results =
        playGameComputerSecond [| "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9" |] "X" "O"

    Assert.DoesNotContain("loss", results)
