/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using System;

namespace Blazor.SPA.Data
{
    public class PaginatorData
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 25;
        public int BlockSize { get; set; } = 10;
        public int RecordCount { get; set; } = 0;
        public string SortColumn { get; set; } = string.Empty;
        public bool SortDescending { get; set; } = false;
    }
}
