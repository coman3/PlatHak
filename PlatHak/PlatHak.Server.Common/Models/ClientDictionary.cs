using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using PlatHak.Common.Maths;
using PlatHak.Server.Common.Annotations;
using PlatHak.Server.Network;

namespace PlatHak.Server.Common.Models
{
    public class ClientDictionary<TItem> : IDictionary<UserClient, ObservableCollection<TItem>>, INotifyCollectionChanged
    {
        private readonly Dictionary<UserClient, ObservableCollection<TItem>> _dictionary = new Dictionary<UserClient, ObservableCollection<TItem>>();

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public ObservableCollection<TItem> this[UserClient key]
        {
            get => _dictionary[key];
            set => _dictionary[key] = value;
        }
        public int Count => _dictionary.Count;
        public bool IsReadOnly => false;

        public ICollection<UserClient> Keys => _dictionary.Keys;
        public ICollection<ObservableCollection<TItem>> Values => _dictionary.Values;
        

        public IEnumerator<KeyValuePair<UserClient, ObservableCollection<TItem>>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<UserClient, ObservableCollection<TItem>> item)
        {
            item.Value.CollectionChanged += ValueOnCollectionChanged;
            _dictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(KeyValuePair<UserClient, ObservableCollection<TItem>> item)
        {
            return _dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<UserClient, ObservableCollection<TItem>>[] array, int arrayIndex)
        {
        }

        public bool Remove(KeyValuePair<UserClient, ObservableCollection<TItem>> item)
        {
            return _dictionary.Remove(item.Key);
        }

       
        public bool ContainsKey(UserClient key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void Add(UserClient key, ObservableCollection<TItem> value)
        {
            _dictionary.Add(key, value);
            value.CollectionChanged += ValueOnCollectionChanged;
        }

        private void ValueOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            CollectionChanged?.Invoke(this, notifyCollectionChangedEventArgs);
        }

        public bool Remove(UserClient key)
        {
            return _dictionary.Remove(key);
        }

        public bool TryGetValue(UserClient key, out ObservableCollection<TItem> value)
        {
            return _dictionary.TryGetValue(key, out value);
        }
        
    }
}