You need JSInterop to handle off page mouseup.

Add a script file including the following functions - I believe it needs to be above the Blazor js file in your base html SPA startup page.  Note you will need your assembly name.  We provide functionality to turn it on and off as we need it, as we don't need it all the time.

```js
window.blazor_setMouseUpEvent = function (show) {
    if (show) {
        window.addEventListener("mouseup", CallBlazorButtonMouseUp);
    }
    else {
        window.removeEventListener("mouseup", CallBlazorButtonMouseUp);
    }
}

function CallBlazorMouseUp(event) {
    DotNet.invokeMethodAsync("[YourAssemblyName]", "ButtonMouseUp");
}
```

An Index.razor page to demo.

When the user mousedowns, the event handler turns on the JS event handler by calling `SetMouseUp`, which then picks up the mouseup event from anywhere on the screen.  It invokes `ButtonMouseUp` which invokes `mouseUpAction`, both static.  The page instance registers `JsMouseUp` with `mouseUpAction`.
`JsMouseUp` does it's business (in our case changing the button colour and writing to output), invokes `StateHasChanged` (the mouse and thus focus could be anywhere) and finally unregisters the js event handler.


```razor
@page "/"

@using System.Diagnostics

<button class="btn @buttoncolour" @onmousedown="(e) => OnButton(e)">Test Click</button>

@code {

    [Inject] private IJSRuntime _js { get; set; }

    [JSInvokable]
    public static Task ButtonMouseUp()
    {
        mouseUpAction?.Invoke();
        return Task.CompletedTask;
    }

    private static Action mouseUpAction;
    private string buttoncolour = "btn-success";

    protected override Task OnInitializedAsync()
    {
        mouseUpAction = JsMouseUp;
        return base.OnInitializedAsync();
    }

    private void OnButton(MouseEventArgs e)
    {
        SetMouseUp(true);
        Debug.WriteLine("Down");
        buttoncolour = "btn-danger";
    }

    private void JsMouseUp()
    {
        Debug.WriteLine("Up");
        buttoncolour = "btn-success";
        _ = InvokeAsync(StateHasChanged);
        SetMouseUp(false);
    }

    private void SetMouseUp(bool action)
    => _js.InvokeAsync<bool>("blazor_setMouseUpEvent", action);

}

```
[![enter image description here][1]][1]

  [1]: https://i.stack.imgur.com/twd0j.png

```