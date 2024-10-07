module FSTTT.GameHelper

open FSTTT.UI
open FSTTT.Board
open FSTTT.MiniMax

let countAvailableMoves (grid: string[]) : int =
    availableMoves grid |> List.length 

let isPlayerOnesTurn (grid: string[]) : bool =
    countAvailableMoves grid % 2 <> 0 

let rec getMove (behavior: Display) (grid: string[]) (game: Game) : int =
    let currentPlayer = 
        if isPlayerOnesTurn grid then game.Player1 
        else game.Player2

    match currentPlayer with
    | Human -> getHumanMove behavior grid
    | AI ->
        let maxToken = if isPlayerOnesTurn grid then game.Token1 else game.Token2
        let minToken = if maxToken = game.Token1 then game.Token2 else game.Token1

        display behavior "AI's move"
        let _, bestAction = maximize grid maxToken minToken 1
        bestAction
