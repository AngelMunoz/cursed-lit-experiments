module Controllers

open Browser.Types
open Browser.Dom
open LitExtensions


type MouseController(host: LitElement) as this =
    [<DefaultValue>]
    val mutable y: float

    [<DefaultValue>]
    val mutable x: float

    [<DefaultValue>]
    val mutable _onMouseMove: Event -> unit

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
