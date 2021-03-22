---
title: Builing Blazor Editor Input Controls
oneliner: This article describes how to build custom editor Input controls.
precis: The third article in a series describing how to build Blazor edit forms/controls with state management, validation and form locking.  This article focuses on form locking.
date: 2021-03-17
published: 2021-03-22
---

# Building A DataList Control in Blazor

This article describes how to build an input control based on a DataList in Blazor, and make it behave like a Select.  *DataList* apppeared in HTML5.  Some browsers, particularly Safari were slow on the uptake, so using was a bit problematic in the early days of HTML5.  Today, all the major browsers on various platforms support it: you can see the support list [here](https://caniuse.com/?search=datalist).

We'll build two versions of the control using Blazor's `InputBase` as the base class.  Along the way we delve into the inner workings of `InputBase` and explore control binding.

## The Html DataList

When `Input` is linked to a `datalist`, it makes filtered suggestions based on the `datalist`.  Out-of-the-box, the user can select a suggestion or enter any text value.  The basic markup for the control is shown below.  Try it in a page.

```html
<input type="text" list="countrylist" />

<datalist id="countrylist" />
    <option value="Algeria" />
    <option value="Australia" />
    <option value="Austria" />
<datalist>
```

## Exploring binding in a Test Control

Before we build our controls, let's explore what's going on in bindings.  You can skip this section if you know your bind triumvirate.

Start with a standard Razor component and code behind file - *MyInput.razor* and *MyInput.Razor.cs*.

Add the following code to *MyInput.razor.cs*.

1. We have what is known as the "Triumverate" of bind properties.
   1. `Value` is the actual value to display.
   2. `ValueChanged` is a Callback that gets wired up to set the value in the parent.
   3. `ValueExpression` is a lambda expression that points back to the source property in the parent.  It's used to generate a `FieldIdentifier` used in validation and state management to uniquely identify the field.
2. `CurrentValue` is the control internal *Value*.  It updates `Value` and invokes `ValueChanged` when changed.
3. `AdditionalAttributes` is used to capture the class and other attributes added to the control.

```csharp
namespace MyNameSpace.Components
{
    public partial class MyInput
    {
        [Parameter] public string Value { get; set; }
        [Parameter] public EventCallback<string> ValueChanged { get; set; }
        [Parameter] public Expression<Func<string>> ValueExpression { get; set; }
        [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

        protected virtual string CurrentValue
        {
            get => Value;
            set
            {
                if (!value.Equals(this.Value))
                {
                    Value = value;
                    if (ValueChanged.HasDelegate)
                        _ = ValueChanged.InvokeAsync(value);
                }
            }
        }
    }
}
```

Add a Text `input` html control to the razor file.

1. Namespace is added so *Components* can be divided into subfolders as the number of source files grow.
2. `@bind-value` points to the controls `CurrentValue` property.
3. `@attributes` adds the control attributes to `input`.

```html
@namespace MyNameSpace.Components

<input type="text" @bind-value="this.CurrentValue" @attributes="this.AdditionalAttributes" />
```

#### Test Page

Add a Test page to *Pages* - or overwrite index if you're using a test site.

This doesn't need much explanation.  Bootstrap for formatting, classic `EditForm`.  `CheckButton` gives us a easy breakpoint we can hit to check values and objects.

You can see our `MyInput` in the form.

```html
@page "/"

@using MyNameSpace.Components

<EditForm Model="this.model" OnValidSubmit="this.ValidSubmit">
    <div class="container m-5 p-4 border border-secondary">
        <div class="row mb-2">
            <div class="col-12">
                <h2>Test Editor</h2>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-4 form-label" for="txtcountry">
                Country
            </div>
            <div class="col-4">
                <MyInput id="txtcountry" @bind-Value="model.Value" class="form-control"></MyInput>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-6">
            </div>
            <div class="col-6 text-right">
                <button class="btn btn-secondary" @onclick="(e) => this.CheckButton()">Check</button>
                <button type="submit" class="btn btn-primary">Submit</button>
            </div>
        </div>
    </div>
</EditForm>

<div class="container">
    <div class="row mb-2">
        <div class="col-4 form-label">
            Test Value
        </div>
        <div class="col-4 form-control">
            @this.model.Value
        </div>
    </div>
    <div class="row mb-2">
        <div class="col-4 form-label">
            Test Index
        </div>
        <div class="col-4 form-control">
            @this.model.index
        </div>
    </div>
</div>
```
```csharp
@code {

    Model model = new Model() { Value = "Australia", index = 2 };

    private void CheckButton()
    {
        var x = true;
    }

    private void ValidSubmit()
    {
        var x = true;
    }

    class Model
    {
        public string Value { get; set; } = string.Empty;
        public int index { get; set; } = 0;
    }
}
```

This should work and update the model values as you change the text in `MyInput`.

Under the hood the Razor compiler builds the section containing `MyInput` into component code like this:

```csharp
__builder2.OpenComponent<TestBlazorServer.Components.MyInput>(12);
__builder2.AddAttribute(13, "id", "txtcountry");
__builder2.AddAttribute(14, "class", "form-control");
__builder2.AddAttribute(15, "Value", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.String>(model.Value));
__builder2.AddAttribute(16, "ValueChanged", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.String>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.String>(this, Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => model.Value = __value, model.Value))));
__builder2.AddAttribute(17, "ValueExpression", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Linq.Expressions.Expression<System.Func<System.String>>>(() => model.Value));
__builder2.CloseComponent();
```

`@bind-value` has translated into a full mapping to the `Value`, `ValueChanged` and `ValueExpression` triumvirate. The setting of `Value` and `ValueExpression` are easy to understand.  `ValueChanged` uses a code factory to effectively build a runtime method that is mapped to `ValueChanged` and sets model.Value to the value returned by `ValueChanged`.

This explains a common issue raised by many - why can't attach an event handler to `@onchange` like this:

```html
<input type="text" @bind-value ="model.Value" @onchange="(e) => myonchangehandler()"/>
```

There's no `@onchange` event on the control, and the one on the inner control is already bound so can't be bound a second time.  You get no error message, just no triggering of the event.

## Inheriting from InputBase

So far we've build the base control, but there's no interaction with the `EditContext` or validation.  We'll get all that with `InputBase`.

The best way to understand what we're doing is to look at an example.  The implementation of `InputText` is shown below.

The Html *input* `value` is bound to `CurrentValue` and `onchange` event to `CurrentValueAsString`.  Any change in the value calls the setter for `CurrentValueASsString`.  This calls `TryParseValueFromString` and either sets `CurrentValue` to the returned `result`, or logs any parsing errors to the message store.  In `InputText` it's pretty simple, set the value and return true - we're only dealing with a string.

```csharp
public class InputText : InputBase<string?>
{
    [DisallowNull] public ElementReference? Element { get; protected set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "input");
        builder.AddMultipleAttributes(1, AdditionalAttributes);
        builder.AddAttribute(2, "class", CssClass);
        builder.AddAttribute(3, "value", BindConverter.FormatValue(CurrentValue));
        builder.AddAttribute(4, "onchange", EventCallback.Factory.CreateBinder<string?>(this, __value => CurrentValueAsString = __value, CurrentValueAsString));
        builder.AddElementReferenceCapture(5, __inputReference => Element = __inputReference);
        builder.CloseElement();
    }

    protected override bool TryParseValueFromString(string? value, out string? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        result = value;
        validationErrorMessage = null;
        return true;
    }
}
``` 

## Building our DataList Control

First we need a helper class to get a country list.  Get the full list from the Repo.

```csharp
using System.Collections.Generic;

namespace MyNameSpace.Data
{
    public static class Countries
    {
        public static List<KeyValuePair<int, string>> CountryList
        {
            get
            {
                List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
                var x = 1;
                foreach (var v in CountryArray)
                {
                    list.Add(new KeyValuePair<int, string>(x, v));
                    x++;
                }
                return list;
            }
        }

        public static SortedDictionary<int, string> CountryDictionary
        {
            get
            {
                SortedDictionary<int, string> list = new SortedDictionary<int, string>();
                var x = 1;
                foreach (var v in CountryArray)
                {
                    list.Add(x, v);
                    x++;
                }
                return list;
            }
        }

        public static string[] CountryArray = new string[]
        {
            "Afghanistan",
            "Albania",
            "Algeria",
.....
            "Zimbabwe",
        };
    }
}
```

### Build the control

The partial class:

```csharp
public partial class InputDataList : InputBase<string>
{
    [Parameter] public IEnumerable<string> DataList { get; set; }
        
    [Parameter] public bool RestrictToList { get; set; }

    private string dataListId { get; set; } = Guid.NewGuid().ToString();

    private bool _valueSetByTab = false;
    private string _typedText = string.Empty;

    protected string CurrentStringValue
    {
        get
        {
            if (DataList.Any(item => item == this.Value))
                return DataList.First(item => item == this.Value);
            else if (RestrictToList)
                return string.Empty;
            else
                return _typedText;
        }
        set
        {
            if (!_valueSetByTab)
            {
                var val = string.Empty;
                if (DataList.Contains(value))
                    val = DataList.First(item => item == value);
                this.CurrentValue = val;
                var hasChanged = val != Value;
            }
            _valueSetByTab = false;
        }
    }

    private void UpdateEnteredText(ChangeEventArgs e)
        => _typedText = e.Value.ToString();

    private void OnKeyDown(KeyboardEventArgs e)
    {
        Debug.WriteLine($"Key: {e.Key}");
        if (RestrictToList && (!string.IsNullOrWhiteSpace(e.Key)) && e.Key.Equals("Tab") && !string.IsNullOrWhiteSpace(this._typedText))
        {
            if (DataList.Any(item => item.Contains(_typedText, StringComparison.CurrentCultureIgnoreCase)))
            {
                var filteredList = DataList.Where(item => item.Contains(_typedText, StringComparison.CurrentCultureIgnoreCase)).ToList();
                this.CurrentValue = filteredList[0];
                _valueSetByTab = true;
            }
        }
    }

    protected override bool TryParseValueFromString(string value, [MaybeNullWhen(false)] out string result, [NotNullWhen(false)] out string validationErrorMessage)
        => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
}
```

And the Razor:

```html
@namespace MyNameSpace.Components
@inherits InputBase<string>

<input class="@CssClass" type="text" @bind-value="this.CurrentStringValue" @attributes="this.AdditionalAttributes" list="@dataListId" @oninput="UpdateEnteredText" @onkeydown="OnKeyDown" />

<datalist id="@dataListId">
    @foreach (var option in this.DataList)
    {
        <option value="@option" />
    }
</datalist>
```

The code:

1. Inherits from `InputBase`.
2. Declares a `DataList` Parameter.
3. Creates a unique name using GUIDs for the html `DataList` control - we may have several on a form.
4. Adds `UpdateEnteredText` event handler wired into `@oninput`. It updates `_typedText` on any keyboard entry.
5. Adds `OnKeyDown` event handler wired into `@onkeydpwn`.  It monitors keyboard entry and acts on a Tab, setting `CurrentValue` to the first entry in the list that matches the typed text.
6. `_valueSetByTab` controls `CurrentValue`.  When triggered by a Tab, it gets called twice.  Initially by `OnKeyDown` and then by `@onchange` through binding when the control loses focus (through tabbing to the next control). `OnKeyDown` sets `_valueSetByTab`, and stops the second update.


The Razor markup looks like this.

1. Input uses the CSS generated by the control.
2. Binds to `CurrentValue`.
3. Adds the additional Attributes, including the `Aria` generated by the control.
4. Binds `list` to the `datalist`.
5. Hooks up event handlers to `oninput` and `onkeydown`.
2. Builds the `datalist` from the control `DataList` property.

Test the control in the test page

```html
<div class="col-4">
    <InputDataList @bind-Value="model.Value" DataList="Countries.CountryArray" class="form-control" placeholder="Select a country"></InputDataList>
</div>
```

The control doesn't use `CurrentValueAsString` and `TryParseValueFromString`.  We need to add logic to `CurrentValueAsString`, but can't - there's no override.  We set up an equivalent property `CurrentStringValue`, and wire the html input to it.  We're only dealing with strings, so there's no parsing error checking to do.

## Input Search Select Control

The second control builds on `InputDataList`.  We convert it to a select type control by adding key/value pair handling, such as you get from a database, and restricting the selection to only *datalist* values.

Copy `InputSearchControl` and rename it to `InputSearchSelectControl`.

Add the generic declaration.  The control will work with most obvious types as the Key - e.g. int, long, string.
```csharp
    public partial class InputDataListSelectT<TValue> : InputBase<TValue>
    {
#nullable enable
   ...
#nullable disable
    }
```

Change DataList to a `SortedDictionary`.
```csharp
[Parameter] public SortedDictionary<TValue, string> DataList { get; set; }
```

At this point we need some cleverer logic to deal with K/V pair and returning `TValue`.

The extra private properties are as follows.

1. Access to the ValidationMessageStore to log parsing errors - strings entered with no match.
2. A control boolean for parsing errors.

```csharp
private ValidationMessageStore? _parsingValidationMessages;
private bool _previousParsingAttemptFailed = false;

```
`CurrentValue` has grown in complexity.  The inline comments provide a detailed explanation of purpose.  We now log parsing errors to the EditContext and notify on error.

```csharp
protected string CurrentStringValue
{
    get
    {
        // check if we have a match to the datalist and get the value from the K/V pair
        if (DataList != null && DataList.Any(item => item.Key.Equals(this.Value)))
            return DataList.First(item => item.Key.Equals(this.Value)).Value;
        // if not return an empty string
        return string.Empty;
    }
    set
    {
        // Check if the value has already been set by tabbing in OnKeyDown
        if (!_valueSetByTab)
        {
            // Check if we have a ValidationMessageStore
            // Either get one or clear the existing one
            if (_parsingValidationMessages == null)
                _parsingValidationMessages = new ValidationMessageStore(EditContext);
            else
                _parsingValidationMessages?.Clear(FieldIdentifier);

            // Check if we have a K/V match for the value
            TValue val;
            if (DataList != null && DataList.ContainsValue(value))
            {

                // get the key
                val = DataList.First(item => item.Value.Equals(value)).Key;
                // assign it to current value - this will kick off a ValueChanged notification on the EditContext
                this.CurrentValue = val;
                //var hasChanged = !val.Equals(Value);
                // Check if the last entry failed validation.  If so notify the EditContext that validation has changed i.e. it's now clear
                if (_previousParsingAttemptFailed)
                {
                    EditContext.NotifyValidationStateChanged();
                    _previousParsingAttemptFailed = false;
                }
            }
            else
            {
                // No K/V match so add a message to the message store
                _parsingValidationMessages?.Add(FieldIdentifier, "You must choose a valid selection");
                // keep track of validation state for the next iteration
                _previousParsingAttemptFailed = true;
                // notify the EditContext whick will precipitate a Validation Message general update
                EditContext.NotifyValidationStateChanged();
            }
        }
        // Clear the Tab notification flag
        _valueSetByTab = false;
    }
}

```

`OnKeyDown` changes a little to adopt to a key/value pair rather then a string array.  We also reset the parsing errors if needed.

```csharp
private void UpdateEnteredText(ChangeEventArgs e)
    => _typedText = e.Value?.ToString();

private void OnKeyDown(KeyboardEventArgs e)
{
    // Debug.WriteLine($"Key: {e.Key}");
    // Check if we have a Tab with some text already typed
    if ((!string.IsNullOrWhiteSpace(e.Key)) && e.Key.Equals("Tab") && !string.IsNullOrWhiteSpace(this._typedText))
    {
        // Check if we have at least one K/V match in the filtered list
        if (DataList != null && DataList.Any(item => item.Value.Contains(_typedText, StringComparison.CurrentCultureIgnoreCase)))
        {
            // the the first K/V pair
            var filteredList = DataList.Where(item => item.Value.Contains(_typedText, StringComparison.CurrentCultureIgnoreCase)).ToList();
            // Set CurrentValue to the key - this will precipitate a ValueChanged notification on the EditContext
            this.CurrentValue = filteredList[0].Key;
            // tell the currentstringvalue setter we've already set the value
            _valueSetByTab = true;
            // Check if the last entry failed validation.  If so notify the EditContext that validation has changed i.e. it's now clear
            if (_previousParsingAttemptFailed)
            {
                EditContext.NotifyValidationStateChanged();
                _previousParsingAttemptFailed = false;
            }
        }
    }
}

// set as blind
protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string validationErrorMessage)
    => throw new NotSupportedException($"This component does not parse normal string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
```

The Razor is almost the same:

1. `datalist` changes to accommodate a K/V pair list.
2. Add the `@typeparam`.

```html
@namespace Blazor.Database.Components


@inherits InputBase<TValue>
@typeparam TValue

<input class="@CssClass" type="text" @bind-value="this.CurrentStringValue" @attributes="this.AdditionalAttributes" list="@dataListId" @oninput="UpdateEnteredText" @onkeydown="OnKeyDown" />

<datalist id="@dataListId">
    @foreach (var kv in this.DataList)
    {
        <option value="@kv.Value" />
    }
</datalist>
```

Test it by adding a row to the edit table in the test page.  Try entering an invalid string - something like "xxxx".

```csharp
        <div class="row mb-2">
            <div class="col-4 form-label" for="txtcountry">
                Country T Index
            </div>
            <div class="col-4">
                <InputDataListSelectT TValue="int" @bind-Value="model.Index" DataList="Countries.CountryDictionary" class="form-control" placeholder="Select a country"></InputDataListSelectT>
            </div>
            <div class="col-4">
                <ValidationMessage For="(() => model.Index)"></ValidationMessage>
            </div>
        </div>
```

```csharp
```

## Wrap Up

Building edit components is not trivial, but also should not be feared.

The examples I've built leverage off `InputBase`.  If you're going to start building your own I thoroughly recommend taking a little time and getting familiar with `InputBase` and it's siblings.  The code is [here](https://github.com/dotnet/aspnetcore/blob/main/src/Components/Web/src/Forms/InputBase.cs).
