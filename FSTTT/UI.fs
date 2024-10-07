module FSTTT.UI

open System.IO
open FSTTT.Board

type Display =
    | Console
    | Test of StringWriter * int list ref


let display (behavior: Display) (message: string) =
    match behavior with
    | Console -> printfn "%s" message
    | Test(writer, _) -> writer.WriteLine(message)

let updateBoard (grid: string[]) (position: int) (player: string) : string[] =
    let updatedGrid = Array.copy grid
    updatedGrid.[position - 1] <- player
    updatedGrid

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
            Some move

let validateMove (grid: string[]) (move: int option) : bool =
    match move with
    | Some pos when isValidMove grid pos -> true
    | _ -> false

let rec getMove (behavior: Display) (grid: string[]) =
    display behavior "Select a position (1-9):"
    let move = readInput behavior

    if validateMove grid move then
        match move with
        | Some pos -> pos
        | None -> failwith "Please try again"
    else
        display behavior "Please try again."
        getMove behavior grid      
        
let endgameResult (grid: string[]) (token1: string) (token2: string) : string =
    if checkWinner grid token1 then
        token1 + " is the winner!"
    elif checkWinner grid token2 then
        token2 + " is the winner!"
    else
        "Womp, it's a tie!"

