using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Blazor.Database.Components
{
    /// <summary>
    /// Form Control to build a Select in a Text Box based on a DataList

    /// </summary>
    public partial class InputDataListSelect : InputBase<int>
    {
#nullable enable

        [Parameter] public SortedDictionary<int, string>? DataList { get; set; }
        private string dataListId { get; set; } = Guid.NewGuid().ToString();

        private bool _valueSetByTab = false;
        private string? _typedText = string.Empty;
        private ValidationMessageStore? _parsingValidationMessages;
        private bool _previousParsingAttemptFailed = false;

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
                    var val = 0;
                    if (DataList != null && DataList.ContainsValue(value))
                    {

                        // get the key
                        val = DataList.First(item => item.Value.Equals(value)).Key;
                        // assign it to current value - this will kick off a ValueChanged notification on the EditContext
                        this.CurrentValue = val;
                        //var hasChanged = !val.Equals(Value);
                        // Check if the last entry failed validation.  If so notify the EditContext that validation has changed i.e. it's now clear
                        if (_previousParsingAttemptFailed)
                            EditContext.NotifyValidationStateChanged();
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

        /// <summary>
        /// Captures the current entered text typed
        /// </summary>
        /// <param name="e"></param>
        private void UpdateEnteredText(ChangeEventArgs e)
           => _typedText = e.Value?.ToString();

        /// <summary>
        /// Monitors Keyboard entry
        /// Acts on a Tab to autocomplete
        /// </summary>
        /// <param name="e"></param>
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
                }
            }
        }

        // set as blind
        protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out int result, [NotNullWhen(false)] out string validationErrorMessage)
            => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");

#nullable disable
    }
}
