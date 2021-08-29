[<RequireQualifiedAccess>]
module App

open Browser.Types
open Fable.Core
open LitExtensions
open Lit
open Lit.Feliz
open Types
open Controllers

[<AttachMembers>]
type private ExApp() as this =
    inherit LitElement()


    let mouseCtrl = MouseController(this)
    let clockCtrl = ClockController(this, 1000)

    let mutable page = Page.Home
    let mutable customSpeed = 3000

    let header () =
        Html.header [
            match page with
            | Page.Home -> Html.h1 "Home"
            | Page.Notes -> Html.h1 "Notes"
        ]

    let nav =
        Html.nav [
            Attr.className "navbar is-fixed-top"
            Html.div [
                Attr.className "navbar-brand"
                Html.a [
                    Attr.className "navbar-item"
                    Ev.onClick (fun _ -> page <- Page.Home)
                    Html.text "Home"
                ]
                Html.a [
                    Attr.className "navbar-item"
                    Html.text "Notes"
                    Ev.onClick (fun _ -> page <- Page.Notes)
                ]
            ]
        ]

    let main () =
        Html.main [
            Html.p $"x: {mouseCtrl.x} - y: {mouseCtrl.y}"
            Html.p $"time: {clockCtrl.time.ToLongTimeString()}"
            match page with
            | Page.Home -> Html.h1 "Home page"
            | Page.Notes -> Html.h1 "Notes page"
            Html.section [
                Html.p "Adjust the clock speed"
                Html.custom (
                    "mwc-slider",
                    [ Attr.min 1
                      Attr.max 10000
                      Attr.step 1
                      Attr.value $"{customSpeed}"
                      Ev.onChange
                          (fun (ev: Event) ->
                              let newSpeed = ev.value |> unbox
                              customSpeed <- newSpeed) ]
                )
            ]
            Html.custom (
                "ex-clock-speed",
                [ Attr.custom ("speed", $"{customSpeed}")
                  Attr.custom ("my-name", "Cursed clock") ]
            )
        ]

    let footer =
        Html.footer [
            Html.p "Cursed Lit + F# App"
        ]

    static member properties = {| page = {| state = true |} |}

    // return this to not use Shadow DOM
    override this.createRenderRoot() = this :> obj

    override _.render() =
        Html.article [
            Attr.className "container is-fluid"
            header ()
            nav
            main ()
            footer
        ]
        |> Feliz.toLit

let register () =
    defineElement ("ex-app", JsInterop.jsConstructor<ExApp>)
