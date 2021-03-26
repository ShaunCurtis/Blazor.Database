/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

namespace Blazor.SPA.Data
{
    /// <summary>
    /// Class defining the return information from a CRUD database operation
    /// </summary>
    public class DbTaskResult
    {
        public string Message { get; set; } = "New Object Message";

        public MessageType Type { get; set; } = MessageType.None;

        public bool IsOK { get; set; } = true;

        public int NewID { get; set; } = 0;

        public object Data { get; set; } = null;

    }
}
