module Main

open Fable.Core
open Fable.Core.JsInterop
open Browser.Types

open Lit
open Lit.Feliz

importSideEffects "./styles.css"

type Event with
    member this.value = (this.target :?> HTMLInputElement).value

[<Emit("customElements.define($0, $1)")>]
let defineElement (name: string, ``component``: obj) = jsNative


[<ImportMember("lit")>]
type ReactiveController =

    abstract member hostConnected : unit -> unit

    abstract member hostDisconnected : unit -> unit

    abstract member hostUpdate : unit -> unit

    abstract member hostUpdated : unit -> unit

[<ImportMember("lit")>]
type ReactiveControllerHost =

    abstract member updateComplete : bool


    abstract member addController : ReactiveController -> unit

    abstract member removeController : ReactiveController -> unit

    abstract member requestUpdate : unit -> unit


[<ImportMember("lit")>]
type LitElement() =

    abstract member attributeChangedCallback :
        string * string option * string option -> unit

    default _.attributeChangedCallback
        (
            name: string,
            ?_old: string,
            ?value: string
        ) : unit =
        jsNative

    abstract member addController : ReactiveController -> unit
    default _.addController(controller: ReactiveController) = jsNative

    abstract member removeController : ReactiveController -> unit
    default _.removeController(controller: ReactiveController) = jsNative

    abstract member connectedCallback : unit -> unit
    default _.connectedCallback() : unit = jsNative

    abstract member disconnectedCallback : unit -> unit
    default _.disconnectedCallback() : unit = jsNative

    abstract member createRenderRoot : unit -> Element
    default _.createRenderRoot() : Element = jsNative
    abstract member render : unit -> Lit.TemplateResult
    default _.render() : Lit.TemplateResult = jsNative
    abstract member firstUpdated : obj -> unit
    default _.firstUpdated(_changedProperties: obj) : unit = jsNative

    abstract member getUpdateComplete : unit -> JS.Promise<bool>
    default _.getUpdateComplete() : JS.Promise<bool> = jsNative

    abstract member performUpdate : unit -> JS.Promise<obj>
    default _.performUpdate() : JS.Promise<obj> = jsNative

    abstract member requestUpdate :
        string option * obj option * obj option -> unit

    default _.requestUpdate(?name: string, ?oldValue: obj, ?options: obj) =
        jsNative

    abstract member shouldUpdate : obj option -> bool
    default _.shouldUpdate(?_changedProperties: obj) : bool = jsNative

    abstract member update : obj option -> unit
    default _.update(?changedProperties: obj) : unit = jsNative

    abstract member updated : obj option -> unit
    default _.updated(?_changedProperties: obj) : unit = jsNative

    abstract member willUpdate : obj option -> unit
    default _.willUpdate(?_changedProperties: obj) : unit = jsNative

    member _.renderRoot: Element = jsNative
    member _.hasUpdated: bool = jsNative
    member _.isUpdatePending: bool = jsNative
    member _.updateComplete: bool = jsNative



type MyElement() =
    inherit LitElement()

    let mutable counter: int = 0

    override _.render() =
        html
            $"""
            <h1>Hello, world!</h1>
            <button @click={fun _ -> counter <- counter + 1}>
                Clicked {counter} times
            </button>
            """

type MyFelizLitElement() =
    inherit LitElement()

    let mutable email = ""
    let mutable password = ""

    let passwordField =
        Html.section [
            Html.label [ Html.text "Password" ]
            Html.input [
                Attr.typePassword
                Attr.placeholder "password"
                Ev.onInput (fun ev -> password <- ev.value)
            ]
        ]

    let emailField =
        Html.section [
            Html.label [ Html.text "Email" ]
            Html.input [
                Attr.typeEmail
                Attr.placeholder "email"
                Ev.onInput (fun ev -> email <- ev.value)
            ]
        ]

    override _.render() =
        Html.article [
            Html.h1 "Hello, world!"
            Html.form [
                Ev.onSubmit
                    (fun ev ->
                        ev.preventDefault ()
                        printfn $"Email: {email}, Password: {password}")
                Html.fieldSet [
                    emailField
                    passwordField
                    Html.button [ Html.text "Login" ]
                ]
            ]
        ]
        |> Feliz.toLit

[<Emit("MyElement.properties = { counter: { state: true } }")>]
let MyElementProperties: unit = jsNative

[<Emit("MyFelizLitElement.properties = { email: { state: true }, password: { state: true } }")>]
let MyFelizLitElementProperties: unit = jsNative


MyElementProperties
MyFelizLitElementProperties

[<Emit("MyElement")>]
let MyElementConstructor: unit = jsNative

[<Emit("MyFelizLitElement")>]
let MyFelizLitElement: unit = jsNative

defineElement ("x-olv", MyElementConstructor)
defineElement ("x-olv-2", MyFelizLitElement)
