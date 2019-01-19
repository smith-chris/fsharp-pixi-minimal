module Game

open Fable.Import.Pixi
open State

type Size = {
  width: float
  height: float
}

let viewport = {
  width = 128.
  height = 128.
}

type InitialState = {
  position: Field<Point>
}

let initialState = {
  position = {
    value = {
      x = 50.
      y = 0.
    }
    func = {
      data = {
        x = -20.
        y = 20.
      }
      time = 0.
    }
  }
}


let getComputedPosition (state: InitialState) (time: float) : Point =
  let {value=v; func=f} = state.position
  let timePassed = time - f.time

  if timePassed < 0.0 then
    printf "Warning: Time doesnt go backwards! (target: %f is smaller than start: %f)" time f.time
    state.position.value

  else 
    applyPointValue v f.data timePassed

let bunny = PIXI.Sprite.fromImage("bunny.png")
bunny.anchor.set(0.5)

let mutable totalTimePassed = 0.

let newPosition = getComputedPosition initialState 20.0

let log point =
  // printf "x: %f, y: %f" point.x point.y
  printf "%O" point

log newPosition

let mutable currentState = initialState

let modifyPositionData state getNewData =
  let newData = getNewData state.position.func.data
  let newPosition = getNewPointField state.position newData totalTimePassed
  {state with position = newPosition}


let tick (delta: float) : unit =
  totalTimePassed <- totalTimePassed + delta
  let state = getComputedPosition currentState totalTimePassed
  bunny.x <- state.x
  bunny.y <- state.y

  if bunny.x >= viewport.width then
    printf "Bounce right"
    currentState <- modifyPositionData currentState (fun data -> {data with x = -abs data.x})
  if bunny.x <= 0. then
    printf "Bounce left"
    currentState <- modifyPositionData currentState (fun data -> {data with x = abs data.x})
  if bunny.y <= 0. then
    printf "Bounce top"
    currentState <- modifyPositionData currentState (fun data -> {data with y = abs data.y})
  if bunny.y >= viewport.height then
    printf "Bounce bottom"
    currentState <- modifyPositionData currentState (fun data -> {data with y = -abs data.y})
    
