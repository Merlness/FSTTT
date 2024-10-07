module FSTTT.Main

open FSTTT.Board
open FSTTT.UI

let isGameOver (grid: string[]) : bool =
    checkWinner grid "X" || checkWinner grid "O" || isFull grid

let playTurn (behavior: Display) (grid: string[]) (currentPlayer: string) : string[] * string =
    let move = getMove behavior grid
    let newBoard = updateBoard grid move currentPlayer
    display behavior (Organize(newBoard))

    let nextPlayer = if currentPlayer = "X" then "O" else "X"
    (newBoard, nextPlayer)

let rec playGame (behavior: Display) (grid: string[]) (currentPlayer: string) =
    if isGameOver grid then
        let resultMessage = endgameResult grid "X" "O"
        display behavior resultMessage
    else
        let newBoard, nextPlayer = playTurn behavior grid currentPlayer
        playGame behavior newBoard nextPlayer

let displayGreeting (behavior: Display) (grid: string[]) =
    display behavior "Welcome to Tic Tac Toe!"
    display behavior (Organize(grid))

[<EntryPoint>]
let main argv =
    let initialGrid = [| "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9" |]
    displayGreeting Console initialGrid

    playGame Console initialGrid "X"
    0
