namespace AssetManagementAPI.Services.Helpers
{
    public class DisposableCollection<T> : ICollection<T>, IDisposable where T : IDisposable
    {
        private readonly ICollection<T> _items;

        public DisposableCollection()
        {
            _items = new List<T>();
        }

        protected DisposableCollection(ICollection<T> collection)
        {
            _items = collection;
        }

        public void Add(T item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return _items.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public static implicit operator DisposableCollection<T>(List<T> list)
        {
            var disposableCollection = new DisposableCollection<T>();
            foreach (var item in list)
            {
                disposableCollection.Add(item);
            }
            return disposableCollection;
        }

        public void Dispose()
        {
            foreach (var item in _items)
            {
                item.Dispose();
            }
            _items.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
