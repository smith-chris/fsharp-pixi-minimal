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
      time = 0
    }
  }
}

let bunny = PIXI.Sprite.fromImage("bunny.png")
bunny.anchor.set(0.5)

let frameTime = 1000.0 / 60.0

let mutable timePassed = 0.

let tick (delta: float) =
  timePassed <- timePassed + delta
  let strength = timePassed / frameTime
  let {value=v; func=f} = initialState.position
  let x = applyValue v.x f.data.x strength
  let y = applyValue v.y f.data.y strength
  bunny.x <- x
  bunny.y <- y
