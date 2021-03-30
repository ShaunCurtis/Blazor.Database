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

        public static DbTaskResult OK(int id = 0)
            => new DbTaskResult() { IsOK = true, Type = MessageType.Success, NewID = id };

        public static DbTaskResult NotOK(int id = 0)
            => new DbTaskResult() { IsOK = false, Type = MessageType.Danger};
    }
}
