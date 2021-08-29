# Lit + Fable!

[lit-html]: https://lit.dev/docs/libraries/standalone-templates/
[lit]: https://lit.dev/docs/

This is an experimental repository with [lit] bindings (the whole thing, not just [lit-html])

Lit is a class based framework for web components that relies on web standards and rather than define a new kind of file extension/language it simply takes javascript to create them.

To create a web component/custom element you only need to define a class and register it on the customElements registry

```fsharp

open Fable.Core
open LitExtensions
open Lit
open Lit.Feliz

[<Emit("customElements.define($0, $1)")>]
let defineElement (name: string, ``component``: obj) = jsNative


[<AttachMembers>]
type private Counter() as this =
    inherit LitElement()
    let mutable counter = 0

    static member properties =
        // tell lit to observe the counter property to know when to update
        {| counter = {| state = true |} |}

    // return `this` to not use Shadow DOM
    override this.createRenderRoot() = this :> obj

    override _.render() =
        Html.div [
            Html.p $"Count: {counter}"
            Html.button [ Ev.onClick(fun _ -> counter <- counter + 1); Html.text "Increment" ]
            Html.button [ Ev.onClick(fun _ -> counter <- counter - 1); Html.text "Decrement" ]
            Html.button [ Ev.onClick(fun _ -> counter <- 0); Html.text "Reset" ]
        ]
        |> Feliz.toLit

defineElement ("my-counter", JsInterop.jsConstructor<Counter>)
```

You can also define `ReactiveControllers` to define shreable pieces of code.

Controllers are basically classes that hook into the life cycle of a Lit component and provide shareable code among components, they are useful for mutable things or high performance updates/animations or fetching resources, allowing your components to simply focus on doing what they are meant to do: render the UI

```fsharp

[<AttachMembers>]
type MouseController(host: LitElement) as this =
    [<DefaultValue>]
    val mutable y: float

    [<DefaultValue>]
    val mutable x: float

    [<DefaultValue>]
    val mutable private _onMouseMove: Event -> unit

    do
        host.addController (this)
        this.x <- 0.
        this.y <- 0.

        this._onMouseMove <-
            fun (e: Event) ->
                let e = e :?> MouseEvent
                this.x <- e.x
                this.y <- e.y
                host.requestUpdate ()


    interface ReactiveController with
        member _.hostConnected() =
            window.addEventListener ("mousemove", this._onMouseMove)

        member _.hostDisconnected() =
            window.removeEventListener ("mousemove", this._onMouseMove)

        member _.hostUpdate() = ()

        member _.hostUpdated() = ()
```
