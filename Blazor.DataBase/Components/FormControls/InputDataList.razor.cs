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
    public partial class InputDataList : InputBase<string>
    {
        [Parameter] public IEnumerable<string>? DataList { get; set; }

        [Parameter] public bool RestrictToList { get; set; }

        private string dataListId { get; set; } = Guid.NewGuid().ToString();

        private bool _setValueByTab = false;
        private string _typedText = string.Empty;
        private ValidationMessageStore _parsingValidationMessages;
        private bool _previousParsingAttemptFailed = false;

        protected string CurrentStringValue
        {
            get
            {
                // check if we have a match to the datalist and get the value from the list
                if (DataList != null && DataList.Any(item => item == this.Value))
                    return DataList.First(item => item == this.Value);
                // if not return an empty string
                else if (RestrictToList)
                    return string.Empty;
                else
                    return _typedText;
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
                string val = string.Empty;
                var _havevalue = false;
                // check if we have a previous valid value - we'll stick with this is the current attempt to set the value is invalid
                var _havepreviousvalue = DataList != null && DataList.Contains(value);

                // Set the value by tabbing in Strict mode.  We need to select the first entry in the DataList
                if (_setValueByTab)
                {
                    if (!string.IsNullOrWhiteSpace(this._typedText))
                    {
                        // Check if we have at least one match in the filtered list
                        _havevalue = DataList != null && DataList.Any(item => item.Contains(_typedText, StringComparison.CurrentCultureIgnoreCase));
                        if (_havevalue)
                        {
                            // the the first value
                            var filteredList = DataList.Where(item => item.Contains(_typedText, StringComparison.CurrentCultureIgnoreCase)).ToList();
                            val = filteredList[0];
                        }
                    }
                }
                // Normal set
                else if (this.RestrictToList)
                {
                    // Check if we have a match and set it if we do
                    _havevalue = DataList != null && DataList.Contains(value);
                    if (_havevalue)
                        val = DataList.First(item => item.Equals(value));
                }
                else
                {
                    _havevalue = true;
                    val = value;
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
                        // No match so add a message to the message store
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
            => _typedText = e.Value?.ToString();

        /// <summary>
        /// Monitors Keyboard entry
        /// Acts on a Tab to autocomplete
        /// </summary>
        /// <param name="e"></param>
        private void OnKeyDown(KeyboardEventArgs e)
        {
            Debug.WriteLine($"Key: {e.Key}");
            // Check if we have a Tab with some text already typed and are in RestrictToList Mode
            _setValueByTab = RestrictToList && (!string.IsNullOrWhiteSpace(e.Key)) && e.Key.Equals("Tab") && !string.IsNullOrWhiteSpace(this._typedText);
        }

        protected override bool TryParseValueFromString(string value, [MaybeNullWhen(false)] out string? result, [NotNullWhen(false)] out string validationErrorMessage)
            => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
    }
}
