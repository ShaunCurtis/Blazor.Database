/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.SPA.Components
{
    public class AppComponentBase : ComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object> UserAttributes { get; set; } = new Dictionary<string, object>();

        protected virtual List<string> UnwantedAttributes { get; set; } = new List<string>();

        protected Dictionary<string, object> SplatterAttributes
            => UserAttributes.Where(item => !item.Key.Equals(UnwantedAttributes.Contains(item.Key)))
               .ToDictionary(item => item.Key, item => item.Value);

    }
}
