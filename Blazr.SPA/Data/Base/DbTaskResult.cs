/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

namespace Blazr.SPA.Data
{
    /// <summary>
    /// Class defining the return information from a CRUD database operation
    /// </summary>
    public class DbTaskResult
    {
        public string Message { get; set; } = null;

        public MessageType Type { get; set; } = MessageType.None;

        public bool IsOK { get; set; } = true;

        public object Data { get; set; } = null;

        public static DbTaskResult OK(object data = null)
            => new DbTaskResult() { IsOK = true, Type = MessageType.Success, Data = data };

        public static DbTaskResult NotOK(object data = null)
            => new DbTaskResult() { IsOK = false, Type = MessageType.Danger, Data = data };
    }
}
