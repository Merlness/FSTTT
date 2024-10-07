module FSTTTTest.MiniMaxTest

open Xunit
open FSTTT.MiniMax

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
