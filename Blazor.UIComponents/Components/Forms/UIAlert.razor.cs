using Blazor.SPA.Data;
using Microsoft.AspNetCore.Components;

namespace Blazor.UIComponents
{
    public partial class UIAlert : ComponentBase
    {
        [Parameter] public AlertData AlertData { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        private string CssClass => AlertData != null ? $"alert alert-sm {this.AlertData.CssType}" : string.Empty;

        private bool Show => AlertData?.Enabled ?? false;



    }
}
