/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using System;

namespace Blazr.SPA.Core
{
    // IMPORTANT - record, page and block numbering is 0 based i.e. the first page is 0
    public class RecordPager
    {
        public int DisplayPage => this.Page + 1;

        public int DisplayLastPage => this.LastPage + 1;

        public int DisplayLastBlock => this.LastBlock + 1;

        public int DisplayStartBlockPage => this.StartBlockPage + 1;

        public int DisplayEndBlockPage => this.EndBlockPage + 1;

        public bool Enabled { get; set; }

        public int Page { get; private set; } = 0;

        public int RecordCount { get; set; } = 0;

        public int PageSize { get; set; } = 10;

        public int BlockSize { get; set; } = 5;

        public string DefaultSortColumn { get; set; } = "ID";

        public bool Sort { get; set; }

        public bool SortDescending { get; set; }

        public int Block
        {
            get
            {
                var block = (int)Math.Floor((Decimal)(this.Page / this.BlockSize));
                return block < this.LastBlock ? block : LastBlock;
            }
        }

        public int LastPage => ((int)Math.Floor((Decimal)((RecordCount - 1) / PageSize)));

        public int LastBlock => (int)Math.Floor((Decimal)(this.LastPage / this.BlockSize));

        public int StartBlockPage => (Block * BlockSize);

        public int EndBlockPage => (StartBlockPage + (BlockSize - 1)) > LastPage ? LastPage : StartBlockPage + (BlockSize - 1);

        public bool HasBlocks => this.LastPage > BlockSize;

        public bool HasPagination => this.RecordCount > PageSize;

        public string SortColumn
        {
            get => (!string.IsNullOrWhiteSpace(_sortColumn)) ? _sortColumn : DefaultSortColumn;
            set => _sortColumn = value;
        }

        private string _sortColumn = string.Empty;

        public RecordPagingData PagingData => new RecordPagingData()
        {
            Page = this.Page,
            PageSize = this.PageSize,
            Sort = this.Sort,
            SortColumn = this.SortColumn,
            SortDescending = this.SortDescending
        };

        public event EventHandler PageChanged;

        public bool ToPage(int page, bool forceUpdate = false)
        {
            var move = (forceUpdate | !this.Page.Equals(page)) && page >= 0;
            if (move)
            {
                this.Page = page;
                this.PageChanged?.Invoke(this, EventArgs.Empty);
            }
            return move;
        }

        public bool PageMove(int pages)
        {
            var move = this.Page + pages <= this.LastPage && this.Page + pages >= 0;
            if (move)
                this.ToPage(this.Page + pages);
            return move;
        }

        public bool BlockMove(int blocks)
        {
            var move = this.Block + blocks <= this.LastBlock && this.Block + blocks >= 0;
            if (move)
                this.ToPage((this.Block + blocks) * BlockSize);
            return move;
        }

        public void NotifySortingChanged()
           => this.ToPage(1, true);
    }
}
