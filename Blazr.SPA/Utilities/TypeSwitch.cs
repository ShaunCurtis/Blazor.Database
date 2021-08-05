/// =================================
/// Author: stackoverflow: cdiggins
/// ==================================

using System;
using System.Collections.Generic;

namespace Blazr.SPA.Components
{
    public class TypeSwitch
    {
        public TypeSwitch Case<T>(Action<T> action) { matches.Add(typeof(T), (x) => action((T)x)); return this; }

        private Dictionary<Type, Action<object>> matches = new Dictionary<Type, Action<object>>();

        public void Switch(object x) { matches[x.GetType()](x); }
    }
}
