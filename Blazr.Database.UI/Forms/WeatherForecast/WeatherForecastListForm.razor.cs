/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.Database.Core;
using Blazr.SPA.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazr.Database.Forms
{
    public partial class WeatherForecastListForm : ListFormBase<WeatherForecast>
    {
        [Inject] private WeatherForecastViewService ViewService { get; set; }

        [Parameter] public bool IsModal { get; set; }

        private BaseModalDialog Modal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.Service = this.ViewService;
            await base.OnInitializedAsync();
        }

        protected override async void Edit(Guid id)
        {
            if (this.IsModal)
            {
                var options = new ModalOptions();
                options.Set("Id", id);
                await this.Modal.ShowAsync<WeatherForecastEditorForm>(options);
            }
            else
                base.Edit(id);
        }
        protected override async void View(Guid id)
        {
            if (this.IsModal)
            {
                var options = new ModalOptions();
                options.Set("Id", id);
                await this.Modal.ShowAsync<WeatherForecastViewerForm>(options);
            }
            else
                base.View(id);
        }

        protected override async void New()
        {
            if (this.IsModal)
            {
                var options = new ModalOptions();
                options.Set("Id", -1);
                await this.Modal.ShowAsync<WeatherForecastEditorForm>(options);
            }
            else
                base.New();
        }
    }
}