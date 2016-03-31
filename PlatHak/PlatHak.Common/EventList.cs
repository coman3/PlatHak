using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PlatHak.Common
{
    public class EventList<T> : IList<T>
    {
        private List<T> Objects { get; set; }

        public delegate void OnAddHandler(CancelEventArgs args, T value);

        public delegate void OnUpdateHandler(CancelEventArgs args, T oldItem, T newItem);
        public delegate void OnRemoveHandler(CancelEventArgs args, T value);
        public delegate void OnClearHandlder();

        public event OnAddHandler OnAdd;
        public event OnRemoveHandler OnRemove;
        public event OnClearHandlder OnClear;
        public event OnUpdateHandler OnUpdate;

        public EventList(int capacity)
        {
            Objects = new List<T>(capacity);
        }
        public EventList()
        {
            Objects = new List<T>();
        }
        public IEnumerator<T> GetEnumerator()
        {
            return Objects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            var args = new CancelEventArgs(false);
            OnAdd?.Invoke(args, item);
            if (args.Cancel) return;
            Objects.Add(item);
        }

        public void Clear()
        {
            OnClear?.Invoke();
            Objects.Clear();
        }

        public bool Contains(T item)
        {
            return Objects.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Objects.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            var args = new CancelEventArgs(false);
            OnRemove?.Invoke(args, item);
            if (args.Cancel) return false;
            return Objects.Remove(item);
        }

        public void CopyTo(Array array, int index)
        {
            Objects.CopyTo(array.Cast<T>().ToArray(), index);
        }

        public int Count => Objects.Count;
        public object SyncRoot => new object();
        public bool IsSynchronized => false;
        public bool IsReadOnly => false;

        public int IndexOf(T item)
        {
            return Objects.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            var args = new CancelEventArgs(false);
            OnAdd?.Invoke(args, item);
            if (args.Cancel) return;
            Objects.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            var args = new CancelEventArgs(false);
            var item = Objects[index];
            OnRemove?.Invoke(args, item);
            if (args.Cancel) return;
            Objects.RemoveAt(index);
        }

        public T this[int index]
        {
            get { return Objects[index]; }
            set
            {
                var args = new CancelEventArgs(false);
                var item = Objects[index];
                OnUpdate?.Invoke(args, value, item);
                if (args.Cancel) return;
                Objects[index] = value;

            }
        }
        public bool RemoveFirst(Predicate<T> match)
        {
            foreach (var o in Objects.Where(match.Invoke))
            {
                if(Remove(o)) return true;
            }
            return false;
        }
        public int RemoveAll(Predicate<T> match)
        {
            var count = 0;
            foreach (var o in Objects.Where(match.Invoke))
            {
                Remove(o);
                count++;
            }
            return count;
        }
    }
}