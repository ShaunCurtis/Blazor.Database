/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.SPA.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Blazr.UIComponents
{
    public class FormDataListControl<TValue> : BaseFormEditControl<TValue>
    {
        [Parameter] public List<String> DataList { get; set; }

        [Parameter] public bool RestrictToList { get; set; }

        protected override RenderFragment ControlFragment => (builder) =>
        {
            builder.OpenComponent(210, typeof(InputDataList));
            builder.AddAttribute(220, "class", this.ControlCss);
            builder.AddAttribute(230, "Value", this.Value);
            builder.AddAttribute(230, "DataList", this.DataList);
            builder.AddAttribute(230, "RestrictToList", this.RestrictToList);
            builder.AddAttribute(240, "ValueChanged", EventCallback.Factory.Create(this, this.ValueChanged));
            builder.AddAttribute(250, "ValueExpression", this.ValueExpression);
            builder.CloseComponent();
        };
    }
}
