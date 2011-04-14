using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iMposter.Model
{
    public class BufferList<T>
    {
        public int Capacity { get { return _capacity; } }
        protected int _capacity;
        public bool IsFilled { get { return _list.Count == Capacity; } }
        public List<T> List { get { return _list; } }
        protected List<T> _list;

        public BufferList()
            : this(10)
        {
        }

        public BufferList(int capacity)
        {
            _capacity = capacity;
            _list = new List<T>(_capacity);
        }

        public void Add(T obj)
        {
            if (_list.Count >= Capacity)
            {
                _list.RemoveRange(0, 1);
            }
            _list.Add(obj);
        }
    }
}
