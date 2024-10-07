module FSTTT.UI

open System.IO
open FSTTT.Board

type Display =
    | Console
    | Test of StringWriter * string list ref


let display (behavior: Display) (message: string) =
    match behavior with
    | Console -> printfn "%s" message
    | Test(writer, _) -> writer.WriteLine(message)

let isAvailable (board: string[]) (pos: int) : bool =
    board.[pos - 1] <> "X" && board.[pos - 1] <> "O"

let isValidMove (board: string[]) (pos: int) : bool =
    pos >= 1 && pos <= 9 && isAvailable board pos

let readInput (behavior: Display) : int option =
    match behavior with
    | Console ->
        try
            let input = System.Console.ReadLine()
            Some(int input)
        with :? System.FormatException ->
            None
    | Test(_, simulatedMovesRef) ->
        match !simulatedMovesRef with
        | [] -> None
        | move :: remainingMoves ->
            simulatedMovesRef := remainingMoves
            match System.Int32.TryParse(move) with  
            | true, parsedMove -> Some parsedMove  
            | false, _ -> None
            
let validateMove (grid: string[]) (move: int option) : bool =
    match move with
    | Some pos when isValidMove grid pos -> true
    | _ -> false

let rec getHumanMove (behavior: Display) (grid: string[]) =
    display behavior "Select a position (1-9):"
    let move = readInput behavior

    if validateMove grid move then
        match move with
        | Some pos -> pos
        | None -> failwith "Please try again"
    else
        display behavior "Please try again."
        getHumanMove behavior grid      
        
let endgameResult (grid: string[]) (token1: string) (token2: string) : string =
    if checkWinner grid token1 then
        token1 + " is the winner!"
    elif checkWinner grid token2 then
        token2 + " is the winner!"
    else
        "Womp, it's a tie!"

let handleSimulatedInput (simulatedRef: string list ref) (behavior: Display) (processInput: string -> 'T) (fallback: unit -> 'T) : 'T =
    match !simulatedRef with
    | input :: remainingInputs ->
        simulatedRef := remainingInputs
        processInput input
    | [] -> fallback ()  


let handleInput (behavior: Display) (processInput: string -> 'T) (fallback: unit -> 'T) : 'T =
    match behavior with
    | Console ->
        let input = System.Console.ReadLine() : string
        processInput input
    | Test (_, simulatedRef) ->
        handleSimulatedInput simulatedRef behavior processInput fallback


let rec askPlayerKind (behavior: Display) (playerNumber: string) : PlayerType =
    display behavior (sprintf "Is %s a Human or AI? (Type 'human' or 'ai')" playerNumber)

    let processInput (input: string) =
        match input.ToLower() with
        | "human" -> Human
        | "ai" -> AI
        | _ -> askPlayerKind behavior playerNumber

    handleInput behavior processInput (fun () -> askPlayerKind behavior playerNumber)


let rec askPlayerToken (behavior: Display) : string =
    display behavior "Choose a token for Player 1 (X or O):"

    let processInput (input: string) =
        match input.ToUpper() with
        | "X" | "O" as token -> token
        | _ -> askPlayerToken behavior

    handleInput behavior processInput (fun () -> askPlayerToken behavior)
