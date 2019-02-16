module Game

open Fable.Import.Pixi
open State

let log point = printf "%O" point

type Time = float

type Size =
    { width : float
      height : float }

type Direction =
    | N
    | E
    | W
    | S

type Action =
    | Bounce of Direction
    | Nothing

let viewport =
    { width = 128.
      height = 128. }

let bounceScreen (position : Point) =
    match position with
    | { x = x } when x >= viewport.width -> Action.Bounce Direction.E
    | { x = x } when x <= 0. -> Action.Bounce Direction.W
    | { y = y } when y >= viewport.height -> Action.Bounce Direction.S
    | { y = y } when y <= 0. -> Action.Bounce Direction.N
    | _ -> Action.Nothing

type State =
    { position : Field<Point> }

let initialState =
    { position =
          { value =
                { x = 50.
                  y = 0. }
            func =
                { data =
                      { x = -20.
                        y = 20. }
                  time = 0. } } }

let getComputedPosition (state : State) (time : float) : Point =
    let { value = v; func = f } = state.position
    let timePassed = time - f.time
    if timePassed < 0.0 then
        printf
            "Warning: Time doesnt go backwards! (target: %f is smaller than start: %f)"
            time f.time
        state.position.value
    else applyPointValue v f.data timePassed

let bunny = PIXI.Sprite.fromImage ("bunny.png")

bunny.anchor.set (0.5)

let mutable totalTimePassed = 0.
let newPosition = getComputedPosition initialState 20.0

log newPosition

let mutable currentState = initialState

let changePosition (state: State) (time: Time) (getNewData: Point -> Point) =
    let newData = getNewData state.position.func.data
    let newPosition = getNewPointField state.position newData totalTimePassed
    { state with position = newPosition }

let setPositionData (state : State) (time: Time) (direction : Direction): State =
    match direction with
    | Direction.E -> changePosition state time (fun data -> { data with x = -abs data.x })
    | Direction.W -> changePosition state time (fun data -> { data with x = abs data.x })
    | Direction.N -> changePosition state time (fun data -> { data with y = abs data.y })
    | Direction.S -> changePosition state time (fun data -> { data with y = -abs data.y })

let update (state : State) (action : Action) (time : Time) =
    match action with
    | Action.Bounce direction -> setPositionData state time direction
    | _ -> state

let tick (delta : float) : unit =
    totalTimePassed <- totalTimePassed + delta
    let position = getComputedPosition currentState totalTimePassed
    bunny.x <- position.x
    bunny.y <- position.y
    let action = bounceScreen position
    currentState <- update currentState action totalTimePassed
