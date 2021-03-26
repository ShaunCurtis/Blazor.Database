using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Blazor.SPA.Components
{
    /// <summary>
    /// Form Control to build a Select in a Text Box based on a DataList

    /// </summary>
    public partial class InputDataListSelect<TValue> : InputBase<TValue>
    {
        [Parameter] public SortedDictionary<TValue, string> DataList { get; set; }

        private string dataListId { get; set; } = Guid.NewGuid().ToString();

        private bool _setValueByTab = false;
        private string _typedText = string.Empty;
        private ValidationMessageStore _parsingValidationMessages;
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
                // Check if we have a ValidationMessageStore
                // Either get one or clear the existing one
                if (_parsingValidationMessages == null)
                    _parsingValidationMessages = new ValidationMessageStore(EditContext);
                else
                    _parsingValidationMessages?.Clear(FieldIdentifier);

                // Set defaults
                TValue val = default;
                var _havevalue = false;
                // check if we have a previous valid value - we'll stick with this is the current attempt to set the value is invalid
                var _havepreviousvalue = DataList != null && DataList.ContainsKey(this.Value);

                // Set the value by tabbing.  We need to select the first entry in the DataList
                if (_setValueByTab)
                {
                    if (!string.IsNullOrWhiteSpace(this._typedText))
                    {
                        // Check if we have at least one K/V match in the filtered list
                        _havevalue = DataList != null && DataList.Any(item => item.Value.Contains(_typedText, StringComparison.CurrentCultureIgnoreCase));
                        if (_havevalue)
                        {
                            // the the first K/V pair
                            var filteredList = DataList.Where(item => item.Value.Contains(_typedText, StringComparison.CurrentCultureIgnoreCase)).ToList();
                            val = filteredList[0].Key;
                        }
                    }
                }
                // Normal set
                else
                {
                    // Check if we have a match and set it if we do
                    _havevalue = DataList != null && DataList.ContainsValue(value);
                    if (_havevalue)
                        val = DataList.First(item => item.Value.Equals(value)).Key;
                }

                // check if we have a valid value
                if (_havevalue)
                {
                    // assign it to current value - this will kick off a ValueChanged notification on the EditContext
                    this.CurrentValue = val;
                    // Check if the last entry failed validation.  If so notify the EditContext that validation has changed i.e. it's now clear
                    if (_previousParsingAttemptFailed)
                    {
                        EditContext.NotifyValidationStateChanged();
                        _previousParsingAttemptFailed = false;
                    }
                }
                // We don't have a valid value
                else
                {
                    // check if we're reverting to the last entry.  If we don't have one the generate error message
                    if (!_havepreviousvalue)
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
                _setValueByTab = false;
            }
        }

        /// <summary>
        /// Captures the current entered text typed
        /// </summary>
        /// <param name="e"></param>
        private void UpdateEnteredText(ChangeEventArgs e)
        {
            _typedText = e.Value?.ToString();
            // Debug.WriteLine($"Text: {this._typedText}");
        }

        /// <summary>
        /// Monitors Keyboard entry
        /// Acts on a Tab to autocomplete
        /// </summary>
        /// <param name="e"></param>
        private void OnKeyDown(KeyboardEventArgs e)
        {
            // Debug.WriteLine($"Key: {e.Key} = Text: { this._typedText}");
            // Check if we have a Tab with some text already typed
            _setValueByTab = ((!string.IsNullOrWhiteSpace(e.Key)) && e.Key.Equals("Tab") && !string.IsNullOrWhiteSpace(this._typedText));
        }

        // set as blind
        protected override bool TryParseValueFromString(string value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string validationErrorMessage)
            => throw new NotSupportedException($"This component does not parse normal string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");

    }
}
