/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Database.Components
{
    public partial class InputSearchControl : BaseInputControl<string>
    {
        [Parameter] public IEnumerable<string> DataList { get; set; }

        private string dataListId { get; set; } = Guid.NewGuid().ToString();

        private bool _valueSetByTab = false;
        private string _typedText = string.Empty;

        protected override void UpdateValue(string value)
        {
            if (!this._valueSetByTab)
            {
                if (DataList.Any(item => item.Contains(value, StringComparison.CurrentCultureIgnoreCase)))
                    base.UpdateValue(value);
                else
                    base.UpdateValue(string.Empty);
            }
            _valueSetByTab = false;
        }

        /// <summary>
        /// Captures the current entered text typed
        /// </summary>
        /// <param name="e"></param>
        private void UpdateEnteredText(ChangeEventArgs e)
           => _typedText = e.Value.ToString();

        /// <summary>
        /// Monitors Keyboard entry
        /// Acts on a Tab to autocomplete
        /// </summary>
        /// <param name="e"></param>
        private void OnKeyDown(KeyboardEventArgs e)
        {
            Debug.WriteLine($"Key: {e.Key}");
            if ((!string.IsNullOrWhiteSpace(e.Key)) && e.Key.Equals("Tab") && !string.IsNullOrWhiteSpace(this._typedText))
            {
                if (DataList.Any(item => item.Contains(_typedText, StringComparison.CurrentCultureIgnoreCase)))
                {
                    var filteredList = DataList.Where(item => item.Contains(_typedText, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    this.CurrentValue = filteredList[0];
                    _valueSetByTab = true;
                }
            }
        }
    }
}
