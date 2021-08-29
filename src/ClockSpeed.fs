[<RequireQualifiedAccess>]
module ClockSpeed

open Fable.Core
open LitExtensions
open Lit
open Lit.Feliz
open Controllers


[<AttachMembers>]
type private ExClockSpeed() as this =
    inherit LitElement()
    let mutable speed = 2000

    let mutable myName = "ExClockSpeed"

    let clockCtrl = ClockController(this, speed)

    static member properties =
        {| speed = {| ``type`` = JS.Constructors.Number |}
           myName =
               {| ``type`` = JsString
                  attribute = "my-name"
                  reflect = true |} |}

    // return this to not use Shadow DOM
    override this.createRenderRoot() = this :> obj

    // we need to monitor the attributes because Feliz doesn't update properties only attributes
    // otherwise you can skip this if you are using lit-html strings
    override _.attributeChangedCallback(name, _, newValue) =
        if name = "speed" then
            speed <-
                newValue
                |> Option.map int
                |> Option.defaultValue speed

            clockCtrl.updateSpeed (speed)

        if name = "my-name" then
            myName <-
                newValue
                |> Option.map unbox
                |> Option.defaultValue myName


    override _.render() =
        Html.div [
            Html.h1 [
                Html.text $"{myName} with custom speed update"
                Html.small " (inspect DOM and console as well)"
            ]
            Html.p $"time: {clockCtrl.time.ToLongTimeString()}"
            Html.p $"Clock interval Speed: {clockCtrl.speed}ms"
            Html.p
                "If you notice Lit won't update the DOM if the value is the same"
            Html.p
                "The update can be called each 1ms and yet the clock value is updated each 1s!"
        ]
        |> Feliz.toLit

let register () =
    defineElement ("ex-clock-speed", JsInterop.jsConstructor<ExClockSpeed>)
