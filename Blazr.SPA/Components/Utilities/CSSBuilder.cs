﻿/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Blazr.SPA.Components
{
    public class CSSBuilder
    {
        private Queue<string> _cssQueue = new Queue<string>();

        public static CSSBuilder Class(string cssFragment = null)
        {
            var builder = new CSSBuilder(cssFragment);
            return builder.AddClass(cssFragment);
        }

        public CSSBuilder()
        {
        }

        public CSSBuilder (string cssFragment)
        {
            AddClass(cssFragment);
        }

        public CSSBuilder AddClass(string cssFragment)
        {
            if (!string.IsNullOrWhiteSpace(cssFragment)) _cssQueue.Enqueue(cssFragment);
            return this;
        }

        public CSSBuilder AddClass(IEnumerable<string> cssFragments)
        {
            if (cssFragments != null)
                cssFragments.ToList().ForEach(item => _cssQueue.Enqueue(item));
            return this;
        }

        public CSSBuilder AddClass(string cssFragment, bool WhenTrue)
            => WhenTrue ? this.AddClass(cssFragment) : this;

        public CSSBuilder AddClass(string trueCssFragment, string falseCssFragment, bool WhenTrue)
            => WhenTrue ? this.AddClass(trueCssFragment) : this.AddClass(falseCssFragment);

        public CSSBuilder AddClassFromAttributes(IReadOnlyDictionary<string, object> additionalAttributes)
        {
            if (additionalAttributes != null && additionalAttributes.TryGetValue("class", out var val))
                _cssQueue.Enqueue(val.ToString());
            return this;
        }

        public CSSBuilder AddClassFromAttributes(IDictionary<string, object> additionalAttributes)
        {
            if (additionalAttributes != null && additionalAttributes.TryGetValue("class", out var val))
                _cssQueue.Enqueue(val.ToString());
            return this;
        }

        public string Build(string CssFragment = null)
        {
            if (!string.IsNullOrWhiteSpace(CssFragment)) _cssQueue.Enqueue(CssFragment);
            if (_cssQueue.Count == 0)
                return string.Empty;
            var sb = new StringBuilder();
            foreach(var str in _cssQueue)
            {
                if (!string.IsNullOrWhiteSpace(str)) sb.Append($" {str}");
            }
            return sb.ToString().Trim();
        }
    }
}
