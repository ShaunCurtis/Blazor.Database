/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;

namespace Blazr.SPA.Components
{
    public partial class ButtonSelectControl : InputBase<int>
    {
        public enum ButtonSize { Large, Normal, Small }

        [Parameter] public SortedDictionary<int, string> DataList { get; set; }

        [Parameter] public string ButtonColour { get; set; }

        [Parameter] public ButtonSize ButtonGroupSize { get; set; }

        private string btnSize => this.ButtonGroupSize switch
        {
            ButtonSize.Large => "btn-group-lg",
            ButtonSize.Small => "btn-group-sm",
            _ => ""
        };


        /// <summary>
        /// Method to Get the individual button class
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetClass(int key)
        {
            if (key == this.Value)
                return $"btn btn-{this.ButtonColour}";
            else
                return this.CleanUpCss($"btn btn-outline-{this.ButtonColour}");
        }

        /// <summary>
        /// Method to change the value
        /// </summary>
        /// <param name="key"></param>
        private void OnButtonSelect(int key)
            => this.CurrentValue = key;

        /// <summary>
        /// Method to clean up the Css - remove leading and trailing spaces and any multiple spaces
        /// </summary>
        /// <param name="css"></param>
        /// <returns></returns>
        protected string CleanUpCss(string css)
        {
            while (css.Contains("  ")) css = css.Replace("  ", " ");
            return css.Trim();
        }

        protected override bool TryParseValueFromString(string value, out int result, out string validationErrorMessage)
            => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");

    }
}
