using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Blazr.SPA.Components
{
    /// <summary>
    /// A display only Input box for formatted text
    /// </summary>
    public class InputReadOnlyText : ComponentBase
    {
        /// <summary>
        /// The string value that will be displayed
        /// </summary>
        [Parameter]
        public string Value { get; set; }

        /// <summary>
        /// The string value that will be displayed
        /// </summary>
        [Parameter]
        public bool AsMarkup { get; set; } = true;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "input");
            builder.AddAttribute(2, "class", "form-control");
            builder.AddAttribute(2, "readonly", "readonly");
            if (AsMarkup) builder.AddAttribute(4, "value", (MarkupString)this.Value);
            else builder.AddAttribute(4, "value", this.Value);
            builder.CloseElement();
        }

    }
}
