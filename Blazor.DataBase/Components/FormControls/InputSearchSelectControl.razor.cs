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

namespace Blazor.Database.Components
{
    public partial class InputSearchSelectControl : BaseInputControl<int>
    {
        [Parameter] public SortedDictionary<int, string> DataList { get; set; }

        private string dataListId { get; set; } = Guid.NewGuid().ToString();

        private bool _valueSetByTab = false;
        private string _typedText = string.Empty;

        protected string CurrentStringValue
        {
            get
            {
                if (DataList.Any(item => item.Key == this.Value))
                    return DataList.First(item => item.Key == this.Value).Value;
                return string.Empty;
            }
            set
            {
                if (!_valueSetByTab)
                {
                    var val = 0;
                    if (DataList.ContainsValue(value))
                        val = DataList.First(item => item.Value == value).Key;
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
                if (DataList.Any(item => item.Value.Contains(_typedText, StringComparison.CurrentCultureIgnoreCase)))
                {
                    var filteredList = DataList.Where(item => item.Value.Contains(_typedText, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    this.CurrentValue = filteredList[0].Key;
                    _valueSetByTab = true;
                }
            }
        }
    }
}
