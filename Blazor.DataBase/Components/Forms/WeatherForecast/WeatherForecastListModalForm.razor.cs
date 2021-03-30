using Blazor.Database.Data;
using Blazor.Database.Services;
using Blazor.SPA.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazor.Database.Components
{
    public partial class WeatherForecastListModalForm : ListFormBase<WeatherForecast>
    {
        [Inject] private WeatherForecastControllerService ControllerService { get; set; }

        private BaseModalDialog Modal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (this.HasService)
            {
                await this.ControllerService.GetRecordsAsync();
                this.ControllerService.ListHasChanged += OnListChanged;
            }
        }

        protected override async void Edit(int id)
        {
            var options = new ModalOptions();
            options.Set("Id", id);
            await this.Modal.ShowAsync<WeatherForecastEditorForm>(options);
        }
        protected override async void View(int id)
        {
            var options = new ModalOptions();
            options.Set("Id", id);
            await this.Modal.ShowAsync<WeatherForecastViewerForm>(options);
        }

        protected override async void New()
        {
            var options = new ModalOptions();
            options.Set("Id", -1);
            await this.Modal.ShowAsync<WeatherForecastEditorForm>(options);
        }

    }
}