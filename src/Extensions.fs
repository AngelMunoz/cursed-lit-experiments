[<AutoOpen>]
module Extensions

open Fable.Core
open Browser.Types

type Event with
    member this.value = (this.target :?> HTMLInputElement).value

[<Emit("customElements.define($0, $1)")>]
let defineElement (name: string, ``component``: obj) = jsNative
