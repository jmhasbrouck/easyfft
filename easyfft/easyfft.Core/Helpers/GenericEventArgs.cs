using System;
using System.Collections.Generic;
using System.Text;

namespace easyfft.Core.Helpers
{
    public class EventArgs<T> : EventArgs
    {

        private T item;

        public EventArgs(T item)
        {
            this.item = item;
        }

        public T Item
        {
            get { return item; }
        }
    }
}
