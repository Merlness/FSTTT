module FSTTT.MiniMax

open FSTTT.Board

let valueMax = -100000
let valueMin = 100000
let initialAction = -1

let computerWins (board: string[]) (token: string) = checkWinner board token

let opponentWins (board: string[]) (token: string) = checkWinner board token

let findValue (board: string[]) (depth: int) (maxToken: string) (minToken: string) : int =
    if computerWins board maxToken then 12 / depth
    elif opponentWins board minToken then -12 / depth
    else 0

let evaluateMove
    (board: string[])
    (action: int)
    (token: string)
    (evaluate: string[] -> string -> string -> int -> int * int)
    (maxToken: string)
    (minToken: string)
    (depth: int)
    : int =
    let newGrid = updateBoard board action token
    let value, _ = evaluate newGrid maxToken minToken (depth + 1)
    value

let compareBestAction
    (value: int)
    (bestValue: int)
    (bestAction: int)
    (action: int)
    (compare: int -> int -> bool)
    : int * int =
    if compare value bestValue then
        value, action
    else
        bestValue, bestAction

let rec evaluateAllMoves
    (board: string[])
    (availableMoves: int list)
    (token: string)
    (evaluate: string[] -> string -> string -> int -> int * int)
    (maxToken: string)
    (minToken: string)
    (depth: int)
    (bestValue: int)
    (bestAction: int)
    (compare: int -> int -> bool)
    : int * int =
    match availableMoves with
    | [] -> bestValue, bestAction
    | action :: remainingMoves ->
        let value = evaluateMove board action token evaluate maxToken minToken depth

        let bestValue, bestAction =
            compareBestAction value bestValue bestAction action compare

        evaluateAllMoves board remainingMoves token evaluate maxToken minToken depth bestValue bestAction compare

let rec minimax
    (board: string[])
    (token: string)
    (evaluate: string[] -> string -> string -> int -> int * int)
    (maxToken: string)
    (minToken: string)
    (depth: int)
    (initialValue: int)
    (compare: int -> int -> bool)
    : int * int =
    if isGameOver board maxToken minToken then
        findValue board depth maxToken minToken, initialAction
    else
        let availableMoves = availableMoves board
        evaluateAllMoves board availableMoves token evaluate maxToken minToken depth initialValue initialAction compare

and minimize (board: string[]) (maxToken: string) (minToken: string) (depth: int) : int * int =
    minimax board minToken maximize maxToken minToken depth valueMin (<)

and maximize (board: string[]) (maxToken: string) (minToken: string) (depth: int) : int * int =
    minimax board maxToken minimize maxToken minToken depth valueMax (>)
