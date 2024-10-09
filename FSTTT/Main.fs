module FSTTT.Main

open FSTTT.UI
open FSTTT.Board
open FSTTT.GameHelper


let playTurn (behavior: Display) (grid: string[]) (game: Game) : string[] =
    let currentToken = if isPlayerOnesTurn grid then game.Token1 else game.Token2
    let move = getMove behavior grid game

    let newBoard = updateBoard grid move currentToken
    display behavior (Organize(newBoard))
    newBoard

let rec playGame (behavior: Display) (grid: string[]) (game: Game) =
    if isGameOver grid game.Token1 game.Token2 then
        let resultMessage = endgameResult grid game.Token1 game.Token2
        display behavior resultMessage
    else
        let newBoard = playTurn behavior grid game
        playGame behavior newBoard game

let setupGame (behavior: Display) (grid: string[]) : Game =
    display behavior "Welcome to Tic Tac Toe!"

    let token1 = askPlayerToken behavior
    let token2 = if token1 = "X" then "O" else "X"
    let player1 = askPlayerKind behavior "Player 1"
    let player2 = askPlayerKind behavior "Player 2"

    display behavior (Organize(grid))

    { Player1 = player1
      Player2 = player2
      Token1 = token1
      Token2 = token2 }

let runGame (behavior: Display) =
    let game = setupGame behavior initialGrid
    playGame behavior initialGrid game

[<EntryPoint>]
let main argv =
    runGame Console
    0