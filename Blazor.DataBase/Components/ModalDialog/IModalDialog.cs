using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Blazor.Database.Components
{
    public interface IModalDialog
    {
        public ModalOptions Options { get; }

        public Task<ModalResult> ShowAsync<TModal>(ModalOptions options) where TModal : IComponent;

        public void Dismiss();

        public void Close(ModalResult result);

        public void Update(ModalOptions options = null);

        public void Lock(bool setlock);
    }
}
