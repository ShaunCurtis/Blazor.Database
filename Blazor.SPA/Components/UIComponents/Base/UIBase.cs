/// =================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: MIT
/// ==================================

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;

namespace Blazor.SPA.Components
{
    public class UIBase : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> UserAttributes { get; set; } = new Dictionary<string, object>();

        [Parameter] public bool Show { get; set; } = true;

        [Parameter] public bool Disabled { get; set; } = false;

        [Parameter] public virtual string AdditionalClasses { get; set; } = string.Empty;

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public RenderFragment HideContent { get; set; }

        protected virtual string PrimaryClass => string.Empty;

        protected List<string> SecondaryClass { get; private set; } = new List<string>() ;

        protected string CssClass
        => CSSBuilder.Class(this.PrimaryClass)
            .AddClass(SecondaryClass)
            .AddClass(AdditionalClasses)
            .AddClassFromAttributes(this.UserAttributes)
            .Build();

        protected virtual string HtmlTag => "div";

        /// <summary>
        /// inherited
        /// </summary>
        /// <param name="builder"></param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (this.Show)
            {
                builder.OpenElement(0, this.HtmlTag);
                if (!string.IsNullOrWhiteSpace(this.CssClass)) builder.AddAttribute(1, "class", this.CssClass);
                if (Disabled) builder.AddAttribute(2, "disabled");
                builder.AddMultipleAttributes(3, this.UserAttributes);
                if (this.ChildContent != null) builder.AddContent(4, ChildContent);
                else if (this.HideContent != null) builder.AddContent(5, HideContent);
                builder.CloseElement();
            }
        }

    }
}
