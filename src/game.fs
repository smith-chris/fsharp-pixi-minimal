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
      x = 0.
      y = 0.
    }
    func = {
      data = {
        x = 20.
        y = 3.
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

let mutable currentState = initialState

let tick (delta: float) : unit =
  totalTimePassed <- totalTimePassed + delta
  let state = getComputedPosition currentState totalTimePassed
  bunny.x <- state.x
  bunny.y <- state.y

  if bunny.x >= viewport.width then
    let newData = {currentState.position.func.data with x = (- abs currentState.position.func.data.x)}
    let newPosition = getNewPointField currentState.position newData totalTimePassed
    currentState <- {currentState with position = newPosition}
    printf "Bounce right"
