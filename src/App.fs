[<RequireQualifiedAccess>]
module App

open Fable.Core
open LitExtensions
open Lit
open Lit.Feliz
open Types
open Controllers

type private ExApp() as this =
    inherit LitElement()

    let controller = MouseController(this)

    let mutable page = Page.Home

    let header () =
        Html.header [
            match page with
            | Page.Home -> Html.h1 "Home"
            | Page.Notes -> Html.h1 "Notes"
        ]

    let nav =
        Html.nav [
            Html.ul [
                Html.li [
                    Html.button [
                        Html.text "Home"
                        Ev.onClick (fun _ -> page <- Page.Home)
                    ]
                ]
                Html.li [
                    Html.button [
                        Html.text "Notes"
                        Ev.onClick (fun _ -> page <- Page.Notes)
                    ]
                ]
            ]
        ]

    let main () =
        Html.main [
            Html.p $"x: {controller.x} - y: {controller.y}"
            match page with
            | Page.Home -> Html.h1 "Home"
            | Page.Notes -> Html.h1 "Notes"
        ]

    let footer =
        Html.footer [
            Html.p "Cursed Lit + F# App"
        ]

    override _.render() =
        Html.article [
            header ()
            nav
            main ()
            footer
        ]
        |> Feliz.toLit

    // return this to not use Shadow DOM
    override this.createRenderRoot() = this :> obj

[<Emit("""
      ExApp.properties = {
          page: { state: true }
      }
      """)>]
let private ExAppProperties: unit = jsNative

ExAppProperties



[<Emit("ExApp")>]
let ExApp: unit = jsNative

let register () = defineElement ("ex-app", ExApp)
