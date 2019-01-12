module Game

open Fable.Import.Pixi
open State

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
        x = 1.
        y = 1.
      }
      time = 0.
    }
  }
}

let frameTime = 1000.0 / 60.0

let getComputedPosition (state: InitialState) (time: float) : Point =
  let {value=v; func=f} = state.position
  let timePassed = time - f.time

  if timePassed < 0.0 then
    printf "Warning: Time doesnt go backwards! (target: %f is smaller than start: %f)" time f.time
    state.position.value

  else 
    let strength = timePassed / frameTime
    let x = applyValue v.x f.data.x strength
    let y = applyValue v.y f.data.y strength
    {x = x; y = y}

let bunny = PIXI.Sprite.fromImage("bunny.png")
bunny.anchor.set(0.5)

let mutable totalTimePassed = 0.

let tick (delta: float) =
  totalTimePassed <- totalTimePassed + delta
  let state = getComputedPosition initialState totalTimePassed
  bunny.x <- state.x
  bunny.y <- state.y
