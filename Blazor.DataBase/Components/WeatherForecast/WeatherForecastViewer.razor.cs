using Blazor.Database.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazor.Database.Components
{
    public partial class WeatherForecastViewer : ComponentBase
    {
        [Inject] private NavigationManager NavManager { get; set; }
        
        [CascadingParameter] IModalDialog Modal { get; set; }

        [Parameter]
        public int ID
        {
            get => this._Id;
            set => this._Id = value;
        }

        private bool HasServices => this.ControllerService != null;

        [Inject] private WeatherControllerService ControllerService { get; set; }

        private bool IsLoaded => this.ControllerService != null && this.ControllerService.Record != null;
        
        private bool _isModal => this.Modal != null;
        private int _Id = -1;

        protected async override Task OnInitializedAsync()
        {
            this.TryGetModalID();
            await this.ControllerService.GetRecordAsync(this._Id);
        }

        private bool TryGetModalID()
        {
            if (this._isModal && this.Modal.Options.TryGet<int>("Id", out int value))
            {
                this._Id = value;
                return true;
            }
            return false;
        }

        private void Exit()
        {
            if (this._isModal)
                this.Modal.Close(ModalResult.OK());
            else
             this.NavManager.NavigateTo("/fetchdata");
        }
    }
}
