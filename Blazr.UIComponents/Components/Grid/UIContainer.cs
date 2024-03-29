﻿/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Microsoft.AspNetCore.Components;

namespace Blazr.UIComponents
{
    public enum BootstrapSize { ExtraSmall, Small, Medium, Large, XLarge, XXLarge, Fluid }

    public class UIContainer : UIComponent
    {
        [Parameter] public BootstrapSize Size { get; set; } = BootstrapSize.Fluid;

        private string Css => Size switch 
        {
            BootstrapSize.Small => "container-sm",
            BootstrapSize.Medium => "container-md",
            BootstrapSize.Large => "container-lg",
            BootstrapSize.XLarge => "container-xl",
            BootstrapSize.XXLarge => "container-xxl",
            BootstrapSize.Fluid => "container-fluid",
            _ => "container"
        };

        protected override void OnInitialized()
            => CssClasses.Add(Css);
    }
}
