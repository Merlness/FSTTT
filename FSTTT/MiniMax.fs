module FSTTT.MiniMax

open FSTTT.Board

let valueMax = -100000
let valueMin = 100000

let computerWins (board: string[]) (token: string) =
    checkWinner board token

let opponentWins (board: string[]) (token: string) =
    checkWinner board token

let findValue (board: string[]) (depth: int) (maxToken: string) (minToken: string) : int =
    if computerWins board maxToken then
        12 / depth
    elif opponentWins board minToken then
        -12 / depth
    else
        0

let rec minimize (board: string[]) (maxToken: string) (minToken: string) (depth: int) : int * int =
    let mutable bestValue = valueMin
    let mutable bestAction = -1
    let gameOver = isGameOver board maxToken minToken

    if gameOver  then
        let value = findValue board depth maxToken minToken
        value, bestAction
    else
        let availableMoves = availableMoves board
        for action in availableMoves do
            let newGrid = updateBoard board (int action) minToken
            let value, _ = maximize newGrid maxToken minToken (depth + 1)
            if value < bestValue then
                bestValue <- value
                bestAction <- action
        bestValue, bestAction

and maximize (board: string[]) (maxToken: string) (minToken: string) (depth: int) : int * int =
    let mutable bestValue = valueMax
    let mutable bestAction = -1
    let gameOver = isGameOver board maxToken minToken

    if gameOver then
        let value = findValue board depth maxToken minToken
        value, bestAction
    else
        let availableMoves = availableMoves board
        for action in availableMoves do
            let newGrid = updateBoard board (int action) maxToken
            let value, _ = minimize newGrid maxToken minToken (depth + 1)
            if value > bestValue then
                bestValue <- value
                bestAction <- action
        bestValue, bestAction
