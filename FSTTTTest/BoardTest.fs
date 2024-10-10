module FSTTT.BoardTest

open Xunit
open FSTTT.Board

[<Fact>]
let ``zero should be 0`` () = Assert.Equal(0, zero)

[<Fact>]
let ``one should be 1`` () = Assert.Equal(1, one)

[<Fact>]
let ``displays the default board`` () =
    let got = Organize(initialGrid)
    let want = "1 | 2 | 3\n4 | 5 | 6\n7 | 8 | 9"
    Assert.Equal(want, got)

[<Fact>]
let ``displays a mix of numbers and tokens`` () =
    let grid = [| "X"; "O"; "3"; "X"; "5"; "O"; "7"; "8"; "9" |]
    let got = Organize(grid)
    let want = "X | O | 3\nX | 5 | O\n7 | 8 | 9"
    Assert.Equal(want, got)

[<Fact>]
let ``displays all tokens`` () =
    let grid = [| "X"; "O"; "X"; "X"; "O"; "X"; "O"; "X"; "O" |]
    let got = Organize(grid)
    let want = "X | O | X\nX | O | X\nO | X | O"
    Assert.Equal(want, got)

[<Fact>]
let ``checks X wins top row`` () =
    let grid = [| "X"; "X"; "X"; "4"; "5"; "6"; "7"; "8"; "9" |]
    let result = checkWinner grid "X"
    Assert.True(result)

[<Fact>]
let ``checks X wins middle row`` () =
    let grid = [| "1"; "2"; "3"; "X"; "X"; "X"; "7"; "8"; "9" |]
    let result = checkWinner grid "X"
    Assert.True(result)

[<Fact>]
let ``checks O wins bottom row`` () =
    let grid = [| "1"; "2"; "3"; "4"; "5"; "6"; "O"; "O"; "O" |]
    let result = checkWinner grid "O"
    Assert.True(result)

[<Fact>]
let ``checks X wins left column`` () =
    let grid = [| "1"; "X"; "3"; "4"; "X"; "6"; "7"; "X"; "9" |]
    let result = checkWinner grid "X"
    Assert.True(result)

[<Fact>]
let ``checks X wins right column`` () =
    let grid = [| "1"; "2"; "X"; "4"; "5"; "X"; "7"; "8"; "X" |]
    let result = checkWinner grid "X"
    Assert.True(result)

[<Fact>]
let ``checks O wins middle column`` () =
    let grid = [| "1"; "O"; "3"; "4"; "O"; "6"; "7"; "O"; "9" |]
    let result = checkWinner grid "O"
    Assert.True(result)

[<Fact>]
let ``checks X wins back diagonal`` () =
    let grid = [| "1"; "2"; "X"; "4"; "X"; "6"; "X"; "8"; "9" |]
    let result = checkWinner grid "X"
    Assert.True(result)

[<Fact>]
let ``checks O wins front diagonal`` () =
    let grid = [| "O"; "2"; "3"; "4"; "O"; "6"; "7"; "8"; "O" |]
    let result = checkWinner grid "O"
    Assert.True(result)

[<Fact>]
let ``checks O doesn't win with random setup`` () =
    let grid = [| "1"; "2"; "3"; "4"; "5"; "6"; "O"; "X"; "O" |]
    let result = checkWinner grid "O"
    Assert.False(result)

[<Fact>]
let ``correct row indices for 3x3 grid`` () =
    let gridSize = 3
    let expectedRows = [ [ 0; 1; 2 ]; [ 3; 4; 5 ]; [ 6; 7; 8 ] ]
    let result: int list list = rows gridSize
    Assert.Equal<int list list>(expectedRows, result)

[<Fact>]
let ``rows function doesn't produce wrong row indices`` () =
    let gridSize = 3
    let wrongRows: int list list = [ [ 0; 2; 3 ]; [ 4; 5; 6 ]; [ 8; 7; 1 ] ]
    let result: int list list = rows gridSize
    Assert.NotEqual<int list list>(wrongRows, result)

[<Fact>]
let ``columns generates correct column indices for 3x3 grid`` () =
    let gridSize = 3
    let expectedColumns = [ [ 0; 3; 6 ]; [ 1; 4; 7 ]; [ 2; 5; 8 ] ]
    let result: int list list = columns gridSize
    Assert.Equal<int list list>(expectedColumns, result)

[<Fact>]
let ``columns function doesn't produce incorrect column indices`` () =
    let gridSize = 3
    let wrongColumns: int list list = [ [ 0; 2; 1 ]; [ 3; 5; 4 ]; [ 6; 8; 7 ] ]
    let result: int list list = columns gridSize
    Assert.NotEqual<int list list>(wrongColumns, result)

[<Fact>]
let ``correct diagonals for 3x3 grid`` () =
    let gridSize = 3
    let expectedDiagonals = [ [ 0; 4; 8 ]; [ 2; 4; 6 ] ]
    let result: int list list = diagonals gridSize
    Assert.Equal<int list list>(expectedDiagonals, result)

[<Fact>]
let ``diagonals function doesn't produce wrong diagonal indices`` () =
    let gridSize = 3
    let wrongDiagonals: int list list = [ [ 0; 3; 6 ]; [ 2; 5; 8 ] ]
    let result: int list list = diagonals gridSize
    Assert.NotEqual<int list list>(wrongDiagonals, result)

[<Fact>]
let ``availableMoves returns all positions when the board is empty`` () =
    let expectedMoves = [ 1; 2; 3; 4; 5; 6; 7; 8; 9 ]
    let result = availableMoves initialGrid
    Assert.Equal<int list>(expectedMoves, result)

[<Fact>]
let ``availableMoves returns no positions when the board is full`` () =
    let grid = [| "X"; "O"; "X"; "O"; "X"; "O"; "X"; "O"; "X" |]
    let expectedMoves = []
    let result = availableMoves grid
    Assert.Equal<int list>(expectedMoves, result)

[<Fact>]
let ``availableMoves returns correct available positions`` () =
    let grid = [| "1"; "X"; "3"; "O"; "5"; "X"; "7"; "8"; "O" |]
    let expectedMoves = [ 1; 3; 5; 7; 8 ]
    let result = availableMoves grid
    Assert.Equal<int list>(expectedMoves, result)
