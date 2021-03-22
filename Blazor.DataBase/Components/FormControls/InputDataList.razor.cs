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
#nullable enable
        [Parameter] public IEnumerable<string>? DataList { get; set; }

        [Parameter] public bool RestrictToList { get; set; }

        private string dataListId { get; set; } = Guid.NewGuid().ToString();

        private bool _valueSetByTab = false;
        private string? _typedText = string.Empty;

        protected string? CurrentStringValue
        {
            get
            {
                if (DataList != null && DataList.Any(item => item == this.Value))
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
                    if (DataList != null && DataList.Contains(value))
                        val = DataList.First(item => item == value);
                    this.CurrentValue = val;
                    var hasChanged = val != Value;
                }
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
            Debug.WriteLine($"Key: {e.Key}");
            if (RestrictToList && (!string.IsNullOrWhiteSpace(e.Key)) && e.Key.Equals("Tab") && !string.IsNullOrWhiteSpace(this._typedText))
            {
                if (DataList != null && DataList.Any(item => item.Contains(_typedText, StringComparison.CurrentCultureIgnoreCase)))
                {
                    var filteredList = DataList.Where(item => item.Contains(_typedText, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    this.CurrentValue = filteredList[0];
                    _valueSetByTab = true;
                }
            }
        }

        protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out string? result, [NotNullWhen(false)] out string validationErrorMessage)
            => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
#nullable disable
    }
}
