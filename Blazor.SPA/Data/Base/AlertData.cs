/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazor.SPA.Data
{
    public class AlertData
    {
        public string Message { get; set; }

        public bool Enabled { get; set; }

        public string CssType { get; set; }

        public void SetAlertFromDbRaskResult(DbTaskResult result)
        {
            if (result.Type != MessageType.None || result.Type != MessageType.NotImplemented)
            {
                this.CssType = $"alert-{result.Type.ToString().ToLower()}";
                this.Enabled = true;
                this.Message = result.Message;
            }
            else 
                this.Enabled = false;
        }

        public void SetAlert(MessageType messageType, string message)
        {
            if (messageType != MessageType.None || messageType != MessageType.NotImplemented)
            {
                this.CssType = $"alert-{messageType.ToString().ToLower()}";
                this.Enabled = true;
                this.Message = message;
            }
            else this.Enabled = false;
        }

        public void ClearAlert()
        {
            var messageType = MessageType.Success;
                this.CssType = $"alert-{messageType.ToString().ToLower()}";
                this.Enabled = false;
                this.Message = "No Messages";
        }

        public static AlertData GetAlert(MessageType messageType, string message)
        {
            var alert = new AlertData();
            if (messageType != MessageType.None || messageType != MessageType.NotImplemented)
            {
                alert.CssType = $"alert-{messageType.ToString().ToLower()}";
                alert.Enabled = true;
                alert.Message = message;
            }
            else alert.Enabled = false;
            return alert;
        }

    }
}
