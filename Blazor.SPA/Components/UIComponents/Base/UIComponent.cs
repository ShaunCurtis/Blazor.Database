/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;

namespace Blazor.SPA.Components
{
    public class UIComponent : AppComponentBase
    {

        [Parameter] public bool Show { get; set; } = true;

        [Parameter] public bool Disabled { get; set; } = false;

        [Parameter] public string Tag { get; set; } = null;

        [Parameter] public EventCallback<MouseEventArgs> ClickEvent { get; set; }

        protected virtual List<string> CssClasses { get; private set; } = new List<string>();

        protected virtual string HtmlTag => this.Tag ?? "div";

        protected override List<string> UnwantedAttributes { get; set; } = new List<string>() { "class" };

        protected string CssClass
            => CSSBuilder.Class()
                .AddClass(CssClasses)
                .AddClassFromAttributes(this.UserAttributes)
                .Build();

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (this.Show)
            {
                builder.OpenElement(0, this.HtmlTag);
                if (!string.IsNullOrWhiteSpace(this.CssClass)) builder.AddAttribute(1, "class", this.CssClass);
                if (Disabled) builder.AddAttribute(2, "disabled");
                if (ClickEvent.HasDelegate)
                    builder.AddAttribute(3, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, ClickEvent));
                builder.AddMultipleAttributes(3, this.SplatterAttributes);
                builder.AddContent(4, ChildContent);
                builder.CloseElement();
            }
        }
    }
}
