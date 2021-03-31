using Blazor.Database.Data;
using Blazor.Database.Services;
using Blazor.SPA.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazor.Database.Components
{
    public partial class WeatherForecastListForm : ListFormBase<WeatherForecast>
    {
        [Inject] private WeatherForecastControllerService ControllerService { get; set; }

        [Parameter] public bool IsModal {get; set;}

        private BaseModalDialog Modal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.Service = this.ControllerService;
            await base.OnInitializedAsync();
        }
        protected override async void Edit(int id)
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
        protected override async void View(int id)
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