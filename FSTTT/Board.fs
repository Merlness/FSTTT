module FSTTT.Board

type PlayerType =
    | Human
    | AI

type Game =
    { Player1: PlayerType
      Player2: PlayerType
      Token1: string
      Token2: string }

let zero = 0
let one = 1

let initialGrid = [| "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9" |]

let Organize (grid: string[]) : string =
    let formatRow row =
        row |> Array.map string |> String.concat " | "

    grid |> Array.chunkBySize 3 |> Array.map formatRow |> String.concat "\n"

let updateBoard (grid: string[]) (position: int) (player: string) : string[] =
    let updatedGrid = Array.copy grid
    updatedGrid.[position - one] <- player
    updatedGrid

let size (grid: string[]) = int (sqrt (float (Array.length grid)))

let rows gridSize =
    [ for i in zero .. gridSize - one -> [ for j in zero .. gridSize - one -> i * gridSize + j ] ]

let columns gridSize =
    [ for i in zero .. gridSize - one -> [ for j in zero .. gridSize - one -> i + j * gridSize ] ]

let backDiagonal gridSize =
    [ for i in zero .. gridSize - one -> i * (gridSize + one) ]

let frontDiagonal gridSize =
    [ for i in zero .. gridSize - one -> (i + one) * (gridSize - one) ]

let diagonals gridSize =
    [ backDiagonal gridSize; frontDiagonal gridSize ]

let checkLine (grid: string[]) (token: string) (line: int list) =
    List.forall (fun index -> grid.[index] = token) line

let checkLines (grid: string[]) (token: string) (lines: int list list) =
    List.exists (fun line -> checkLine grid token line) lines

let checkWinner (grid: string[]) (token: string) =
    let gridSize = size grid

    checkLines grid token (rows gridSize)
    || checkLines grid token (columns gridSize)
    || checkLines grid token (diagonals gridSize)

let isFull (grid: string[]) : bool =
    Array.forall (fun x -> x = "X" || x = "O") grid

let availableMoves (grid: string[]) : int list =
    let mutable moves = []

    for i = 0 to Array.length grid - 1 do
        if grid.[i] <> "X" && grid.[i] <> "O" then
            moves <- (i + 1) :: moves

    List.rev moves

let isGameOver (grid: string[]) (token1: string) (token2: string) : bool =
    checkWinner grid token1 || checkWinner grid token2 || isFull grid
