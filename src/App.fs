module App

open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.Pixi
open Game

let options = jsOptions<PIXI.ApplicationOptions> (fun o ->
  o.backgroundColor <- Some 0x000000
)
let app = PIXI.Application(128., 128., options)
Browser.document.body.appendChild(app.view) |> ignore

app.stage.addChild(bunny) |> ignore

// Listen for animate update
app.ticker.add(tick) |> ignore