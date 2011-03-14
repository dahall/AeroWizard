using System.Collections.ObjectModel;
using System.ComponentModel;

namespace System.Collections.Generic
{
	/// <summary>
	/// A generic list that provides event for changes to the list.
	/// </summary>
	/// <typeparam name="T">Type for the list.</typeparam>
	[Serializable]
	public class EventedList<T> : IList<T>, IList
	{
		// Fields
		private const int _defaultCapacity = 4;

		private static T[] _emptyArray;

		private T[] _items;
		private int _size;
		[NonSerialized]
		private object _syncRoot;
		private int _version;

		/// <summary>
		/// Initializes the <see cref="EventedList&lt;T&gt;"/> class.
		/// </summary>
		static EventedList()
		{
			EventedList<T>._emptyArray = new T[0];
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EventedList&lt;T&gt;"/> class.
		/// </summary>
		public EventedList()
		{
			this._items = EventedList<T>._emptyArray;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EventedList&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="collection">The collection.</param>
		public EventedList(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			ICollection<T> is2 = collection as ICollection<T>;
			if (is2 != null)
			{
				int count = is2.Count;
				this._items = new T[count];
				is2.CopyTo(this._items, 0);
				this._size = count;
			}
			else
			{
				this._size = 0;
				this._items = new T[4];
				using (IEnumerator<T> enumerator = collection.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						this.Add(enumerator.Current);
					}
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EventedList&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		public EventedList(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity");
			}
			this._items = new T[capacity];
		}

		/// <summary>
		/// Occurs when an item has been added.
		/// </summary>
		public event EventHandler<ListChangedEventArgs<T>> ItemAdded;

		/// <summary>
		/// Occurs when an item has changed.
		/// </summary>
		public event EventHandler<ListChangedEventArgs<T>> ItemChanged;

		/// <summary>
		/// Occurs when an item has been deleted.
		/// </summary>
		public event EventHandler<ListChangedEventArgs<T>> ItemDeleted;

		/// <summary>
		/// Occurs when the list has been reset.
		/// </summary>
		public event EventHandler<ListChangedEventArgs<T>> Reset;

		/// <summary>
		/// Gets or sets the capacity.
		/// </summary>
		/// <value>The capacity.</value>
		public int Capacity
		{
			get
			{
				return this._items.Length;
			}
			set
			{
				if (value != this._items.Length)
				{
					if (value < this._size)
					{
						throw new ArgumentOutOfRangeException("value");
					}
					if (value > 0)
					{
						T[] destinationArray = new T[value];
						if (this._size > 0)
						{
							Array.Copy(this._items, 0, destinationArray, 0, this._size);
						}
						this._items = destinationArray;
					}
					else
					{
						this._items = EventedList<T>._emptyArray;
					}
				}
			}
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <value>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</value>
		public int Count
		{
			get
			{
				return this._size;
			}
		}

		/// <summary>
		/// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
		/// </summary>
		/// <value></value>
		/// <returns>true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.</returns>
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
		/// </summary>
		/// <value></value>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.</returns>
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					System.Threading.Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.</returns>
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.IList"/> has a fixed size.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Collections.IList"/> has a fixed size; otherwise, false.</returns>
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.</returns>
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="System.Object"/> at the specified index.
		/// </summary>
		/// <value></value>
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				EventedList<T>.VerifyValueType(value);
				this[index] = (T)value;
			}
		}

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <value>The element at the specified index.</value>
		public T this[int index]
		{
			get
			{
				if (index >= this._size)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return this._items[index];
			}
			set
			{
				if (index >= this._size)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				T oldValue = this._items[index];
				this._items[index] = value;
				this._version++;
				OnItemChanged(index, oldValue, value);
			}
		}

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		public void Add(T item)
		{
			if (this._size == this._items.Length)
			{
				this.EnsureCapacity(this._size + 1);
			}
			this._items[this._size++] = item;
			this._version++;
			OnItemAdded(this._size, item);
		}

		/// <summary>
		/// Adds the range.
		/// </summary>
		/// <param name="collection">The collection.</param>
		public void AddRange(IEnumerable<T> collection)
		{
			this.InsertRange(this._size, collection);
		}

		/// <summary>
		/// Ases the read only.
		/// </summary>
		/// <returns></returns>
		public ReadOnlyCollection<T> AsReadOnly()
		{
			return new ReadOnlyCollection<T>(this);
		}

		/// <summary>
		/// Binaries the search.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		public int BinarySearch(T item)
		{
			return this.BinarySearch(0, this.Count, item, null);
		}

		/// <summary>
		/// Binaries the search.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns></returns>
		public int BinarySearch(T item, IComparer<T> comparer)
		{
			return this.BinarySearch(0, this.Count, item, comparer);
		}

		/// <summary>
		/// Binaries the search.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="count">The count.</param>
		/// <param name="item">The item.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns></returns>
		public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
		{
			if ((index < 0) || (count < 0))
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count");
			}
			if ((this._size - index) < count)
			{
				throw new ArgumentException();
			}
			return Array.BinarySearch<T>(this._items, index, count, item, comparer);
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
		public void Clear()
		{
			Array.Clear(this._items, 0, this._size);
			this._size = 0;
			this._version++;
			OnReset();
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <returns>
		/// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
		/// </returns>
		public bool Contains(T item)
		{
			if (item == null)
			{
				for (int j = 0; j < this._size; j++)
				{
					if (this._items[j] == null)
					{
						return true;
					}
				}
				return false;
			}
			EqualityComparer<T> comparer = EqualityComparer<T>.Default;
			for (int i = 0; i < this._size; i++)
			{
				if (comparer.Equals(this._items[i], item))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Converts all.
		/// </summary>
		/// <typeparam name="TOutput">The type of the output.</typeparam>
		/// <param name="converter">The converter.</param>
		/// <returns></returns>
		public EventedList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
		{
			if (converter == null)
			{
				throw new ArgumentNullException("converter");
			}
			EventedList<TOutput> list = new EventedList<TOutput>(this._size);
			for (int i = 0; i < this._size; i++)
			{
				list._items[i] = converter(this._items[i]);
			}
			list._size = this._size;
			return list;
		}

		/// <summary>
		/// Copies to.
		/// </summary>
		/// <param name="array">The array.</param>
		public void CopyTo(T[] array)
		{
			this.CopyTo(array, 0);
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="array"/> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="arrayIndex"/> is less than 0.</exception>
		/// <exception cref="T:System.ArgumentException">
		/// 	<paramref name="array"/> is multidimensional.-or-<paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type <c>T</c> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(this._items, 0, array, arrayIndex, this._size);
		}

		/// <summary>
		/// Copies to.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="array">The array.</param>
		/// <param name="arrayIndex">Index of the array.</param>
		/// <param name="count">The count.</param>
		public void CopyTo(int index, T[] array, int arrayIndex, int count)
		{
			if ((this._size - index) < count)
			{
				throw new ArgumentException();
			}
			Array.Copy(this._items, index, array, arrayIndex, count);
		}

		/// <summary>
		/// Existses the specified match.
		/// </summary>
		/// <param name="match">The match.</param>
		/// <returns></returns>
		public bool Exists(Predicate<T> match)
		{
			return (this.FindIndex(match) != -1);
		}

		/// <summary>
		/// Finds the specified match.
		/// </summary>
		/// <param name="match">The match.</param>
		/// <returns></returns>
		public T Find(Predicate<T> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			for (int i = 0; i < this._size; i++)
			{
				if (match(this._items[i]))
				{
					return this._items[i];
				}
			}
			return default(T);
		}

		/// <summary>
		/// Finds all.
		/// </summary>
		/// <param name="match">The match.</param>
		/// <returns></returns>
		public EventedList<T> FindAll(Predicate<T> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			EventedList<T> list = new EventedList<T>();
			for (int i = 0; i < this._size; i++)
			{
				if (match(this._items[i]))
				{
					list.Add(this._items[i]);
				}
			}
			return list;
		}

		/// <summary>
		/// Finds the index.
		/// </summary>
		/// <param name="match">The match.</param>
		/// <returns></returns>
		public int FindIndex(Predicate<T> match)
		{
			return this.FindIndex(0, this._size, match);
		}

		/// <summary>
		/// Finds the index.
		/// </summary>
		/// <param name="startIndex">The start index.</param>
		/// <param name="match">The match.</param>
		/// <returns></returns>
		public int FindIndex(int startIndex, Predicate<T> match)
		{
			return this.FindIndex(startIndex, this._size - startIndex, match);
		}

		/// <summary>
		/// Finds the index.
		/// </summary>
		/// <param name="startIndex">The start index.</param>
		/// <param name="count">The count.</param>
		/// <param name="match">The match.</param>
		/// <returns></returns>
		public int FindIndex(int startIndex, int count, Predicate<T> match)
		{
			if (startIndex > this._size)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			if ((count < 0) || (startIndex > (this._size - count)))
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			int num = startIndex + count;
			for (int i = startIndex; i < num; i++)
			{
				if (match(this._items[i]))
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Finds the last.
		/// </summary>
		/// <param name="match">The match.</param>
		/// <returns></returns>
		public T FindLast(Predicate<T> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			for (int i = this._size - 1; i >= 0; i--)
			{
				if (match(this._items[i]))
				{
					return this._items[i];
				}
			}
			return default(T);
		}

		/// <summary>
		/// Finds the last index.
		/// </summary>
		/// <param name="match">The match.</param>
		/// <returns></returns>
		public int FindLastIndex(Predicate<T> match)
		{
			return this.FindLastIndex(this._size - 1, this._size, match);
		}

		/// <summary>
		/// Finds the last index.
		/// </summary>
		/// <param name="startIndex">The start index.</param>
		/// <param name="match">The match.</param>
		/// <returns></returns>
		public int FindLastIndex(int startIndex, Predicate<T> match)
		{
			return this.FindLastIndex(startIndex, startIndex + 1, match);
		}

		/// <summary>
		/// Finds the last index.
		/// </summary>
		/// <param name="startIndex">The start index.</param>
		/// <param name="count">The count.</param>
		/// <param name="match">The match.</param>
		/// <returns></returns>
		public int FindLastIndex(int startIndex, int count, Predicate<T> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			if (this._size == 0)
			{
				if (startIndex != -1)
				{
					throw new ArgumentOutOfRangeException("startIndex");
				}
			}
			else if (startIndex >= this._size)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			if ((count < 0) || (((startIndex - count) + 1) < 0))
			{
				throw new ArgumentOutOfRangeException("count");
			}
			int num = startIndex - count;
			for (int i = startIndex; i > num; i--)
			{
				if (match(this._items[i]))
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Fors the each.
		/// </summary>
		/// <param name="action">The action.</param>
		public void ForEach(Action<T> action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("match");
			}
			for (int i = 0; i < this._size; i++)
			{
				action(this._items[i]);
			}
		}

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns></returns>
		public EventedList<T>.Enumerator GetEnumerator()
		{
			return new EventedList<T>.Enumerator((EventedList<T>)this);
		}

		/// <summary>
		/// Gets the range of items and returns then in another list.
		/// </summary>
		/// <param name="index">The starting index.</param>
		/// <param name="count">The count of items to place in the list.</param>
		/// <returns>An <see cref="EventedList&lt;T&gt;"/> with the requested items.</returns>
		public EventedList<T> GetRange(int index, int count)
		{
			if ((index < 0) || (count < 0))
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count");
			}
			if ((this._size - index) < count)
			{
				throw new ArgumentException();
			}
			EventedList<T> list = new EventedList<T>(count);
			Array.Copy(this._items, index, list._items, 0, count);
			list._size = count;
			return list;
		}

		/// <summary>
		/// Copies to.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="arrayIndex">Index of the array.</param>
		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			if ((array != null) && (array.Rank != 1))
			{
				throw new ArgumentException();
			}
			try
			{
				Array.Copy(this._items, 0, array, arrayIndex, this._size);
			}
			catch (ArrayTypeMismatchException)
			{
				throw new ArgumentException();
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new EventedList<T>.Enumerator((EventedList<T>)this);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new EventedList<T>.Enumerator((EventedList<T>)this);
		}

		/// <summary>
		/// Adds the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		int IList.Add(object item)
		{
			EventedList<T>.VerifyValueType(item);
			this.Add((T)item);
			return (this.Count - 1);
		}

		/// <summary>
		/// Determines whether [contains] [the specified item].
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>
		/// 	<c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
		/// </returns>
		bool IList.Contains(object item)
		{
			return (EventedList<T>.IsCompatibleObject(item) && this.Contains((T)item));
		}

		/// <summary>
		/// Indexes the of.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		int IList.IndexOf(object item)
		{
			if (EventedList<T>.IsCompatibleObject(item))
			{
				return this.IndexOf((T)item);
			}
			return -1;
		}

		/// <summary>
		/// Inserts the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="item">The item.</param>
		void IList.Insert(int index, object item)
		{
			EventedList<T>.VerifyValueType(item);
			this.Insert(index, (T)item);
		}

		/// <summary>
		/// Removes the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		void IList.Remove(object item)
		{
			if (EventedList<T>.IsCompatibleObject(item))
			{
				this.Remove((T)item);
			}
		}

		/// <summary>
		/// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
		/// <returns>
		/// The index of <paramref name="item"/> if found in the list; otherwise, -1.
		/// </returns>
		public int IndexOf(T item)
		{
			return Array.IndexOf<T>(this._items, item, 0, this._size);
		}

		/// <summary>
		/// Indexes the of.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public int IndexOf(T item, int index)
		{
			if (index > this._size)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return Array.IndexOf<T>(this._items, item, index, this._size - index);
		}

		/// <summary>
		/// Indexes the of.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="index">The index.</param>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		public int IndexOf(T item, int index, int count)
		{
			if (index > this._size)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if ((count < 0) || (index > (this._size - count)))
			{
				throw new ArgumentOutOfRangeException("count");
			}
			return Array.IndexOf<T>(this._items, item, index, count);
		}

		/// <summary>
		/// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
		/// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
		public void Insert(int index, T item)
		{
			if (index > this._size)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (this._size == this._items.Length)
			{
				this.EnsureCapacity(this._size + 1);
			}
			if (index < this._size)
			{
				Array.Copy(this._items, index, this._items, index + 1, this._size - index);
			}
			this._items[index] = item;
			this._size++;
			this._version++;
			OnItemAdded(index, item);
		}

		/// <summary>
		/// Inserts the range.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="collection">The collection.</param>
		public void InsertRange(int index, IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (index > this._size)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			ICollection<T> is2 = collection as ICollection<T>;
			if (is2 != null)
			{
				int count = is2.Count;
				if (count > 0)
				{
					this.EnsureCapacity(this._size + count);
					if (index < this._size)
					{
						Array.Copy(this._items, index, this._items, index + count, this._size - index);
					}
					if (this == is2)
					{
						Array.Copy(this._items, 0, this._items, index, index);
						Array.Copy(this._items, (int)(index + count), this._items, (int)(index * 2), (int)(this._size - index));
					}
					else
					{
						T[] array = new T[count];
						is2.CopyTo(array, 0);
						array.CopyTo(this._items, index);
					}
					this._size += count;
					for (int i = index; i < index + count; i++)
						OnItemAdded(i, this._items[i]);
				}
			}
			else
			{
				using (IEnumerator<T> enumerator = collection.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						this.Insert(index++, enumerator.Current);
					}
				}
			}
			this._version++;
		}

		/// <summary>
		/// Lasts the index of.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		public int LastIndexOf(T item)
		{
			return this.LastIndexOf(item, this._size - 1, this._size);
		}

		/// <summary>
		/// Lasts the index of.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public int LastIndexOf(T item, int index)
		{
			if (index >= this._size)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return this.LastIndexOf(item, index, index + 1);
		}

		/// <summary>
		/// Lasts the index of.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="index">The index.</param>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		public int LastIndexOf(T item, int index, int count)
		{
			if (this._size == 0)
			{
				return -1;
			}
			if ((index < 0) || (count < 0))
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count");
			}
			if ((index >= this._size) || (count > (index + 1)))
			{
				throw new ArgumentOutOfRangeException((index >= this._size) ? "index" : "count");
			}
			return Array.LastIndexOf<T>(this._items, item, index, count);
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <returns>
		/// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		public bool Remove(T item)
		{
			int index = this.IndexOf(item);
			if (index >= 0)
			{
				this.RemoveAt(index);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Removes all.
		/// </summary>
		/// <param name="match">The match.</param>
		/// <returns></returns>
		public int RemoveAll(Predicate<T> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			int index = 0;
			while ((index < this._size) && !match(this._items[index]))
			{
				index++;
			}
			if (index >= this._size)
			{
				return 0;
			}
			int num2 = index + 1;
			while (num2 < this._size)
			{
				while ((num2 < this._size) && match(this._items[num2]))
				{
					num2++;
				}
				if (num2 < this._size)
				{
					T oldVal = this._items[index + 1];
					this._items[index++] = this._items[num2++];
					this.OnItemDeleted(index, oldVal);
				}
			}
			Array.Clear(this._items, index, this._size - index);
			int num3 = this._size - index;
			this._size = index;
			this._version++;
			return num3;
		}

		/// <summary>
		/// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
		public void RemoveAt(int index)
		{
			if (index >= this._size)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this._size--;
			T oldVal = this._items[index];
			if (index < this._size)
			{
				Array.Copy(this._items, index + 1, this._items, index, this._size - index);
			}
			this._items[this._size] = default(T);
			this._version++;
			this.OnItemDeleted(index, oldVal);
		}

		/// <summary>
		/// Removes the range.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="count">The count.</param>
		public void RemoveRange(int index, int count)
		{
			if ((index < 0) || (count < 0))
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count");
			}
			if ((this._size - index) < count)
			{
				throw new ArgumentException();
			}
			if (count > 0)
			{
				this._size -= count;
				T[] array = new T[count];
				Array.Copy(this._items, index, array, 0, count);
				if (index < this._size)
				{
					Array.Copy(this._items, index + count, this._items, index, this._size - index);
				}
				Array.Clear(this._items, this._size, count);
				this._version++;
				for (int i = index; i < index + count; i++)
					OnItemDeleted(i, array[i - index]);
			}
		}

		/// <summary>
		/// Reverses this instance.
		/// </summary>
		public void Reverse()
		{
			this.Reverse(0, this.Count);
		}

		/// <summary>
		/// Reverses the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="count">The count.</param>
		public void Reverse(int index, int count)
		{
			if ((index < 0) || (count < 0))
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count");
			}
			if ((this._size - index) < count)
			{
				throw new ArgumentException();
			}
			Array.Reverse(this._items, index, count);
			this._version++;
		}

		/// <summary>
		/// Sorts this instance.
		/// </summary>
		public void Sort()
		{
			this.Sort(0, this.Count, null);
		}

		/// <summary>
		/// Sorts the specified comparer.
		/// </summary>
		/// <param name="comparer">The comparer.</param>
		public void Sort(IComparer<T> comparer)
		{
			this.Sort(0, this.Count, comparer);
		}

		/// <summary>
		/// Sorts the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="count">The count.</param>
		/// <param name="comparer">The comparer.</param>
		public void Sort(int index, int count, IComparer<T> comparer)
		{
			if ((index < 0) || (count < 0))
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count");
			}
			if ((this._size - index) < count)
			{
				throw new ArgumentException();
			}
			Array.Sort<T>(this._items, index, count, comparer);
			this._version++;
		}

		/// <summary>
		/// Toes the array.
		/// </summary>
		/// <returns></returns>
		public T[] ToArray()
		{
			T[] destinationArray = new T[this._size];
			Array.Copy(this._items, 0, destinationArray, 0, this._size);
			return destinationArray;
		}

		/// <summary>
		/// Trims the excess.
		/// </summary>
		public void TrimExcess()
		{
			int num = (int)(this._items.Length * 0.9);
			if (this._size < num)
			{
				this.Capacity = this._size;
			}
		}

		/// <summary>
		/// Trues for all.
		/// </summary>
		/// <param name="match">The match.</param>
		/// <returns></returns>
		public bool TrueForAll(Predicate<T> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			for (int i = 0; i < this._size; i++)
			{
				if (!match(this._items[i]))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Called when [insert].
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		protected virtual void OnItemAdded(int index, T value)
		{
			EventHandler<ListChangedEventArgs<T>> h = ItemAdded;
			if (h != null)
				h(this, new EventedList<T>.ListChangedEventArgs<T>(ListChangedType.ItemAdded, value, index));
		}

		/// <summary>
		/// Called when [set].
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		protected virtual void OnItemChanged(int index, T oldValue, T newValue)
		{
			EventHandler<ListChangedEventArgs<T>> h = ItemChanged;
			if (h != null)
				h(this, new EventedList<T>.ListChangedEventArgs<T>(ListChangedType.ItemChanged, newValue, index, oldValue));
		}

		/// <summary>
		/// Called when [remove].
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		protected virtual void OnItemDeleted(int index, T value)
		{
			EventHandler<ListChangedEventArgs<T>> h = ItemDeleted;
			if (h != null)
				h(this, new EventedList<T>.ListChangedEventArgs<T>(ListChangedType.ItemDeleted, value, index));
		}

		/// <summary>
		/// Called when [clear].
		/// </summary>
		protected virtual void OnReset()
		{
			EventHandler<ListChangedEventArgs<T>> h = Reset;
			if (h != null)
				h(this, new EventedList<T>.ListChangedEventArgs<T>(ListChangedType.Reset));
		}

		/// <summary>
		/// Determines whether [is compatible object] [the specified value].
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		/// 	<c>true</c> if [is compatible object] [the specified value]; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsCompatibleObject(object value)
		{
			if (!(value is T) && ((value != null) || typeof(T).IsValueType))
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Verifies the type of the value.
		/// </summary>
		/// <param name="value">The value.</param>
		private static void VerifyValueType(object value)
		{
			if (!EventedList<T>.IsCompatibleObject(value))
			{
				throw new ArgumentException("value");
			}
		}

		// Methods
		/// <summary>
		/// Ensures the capacity.
		/// </summary>
		/// <param name="min">The min.</param>
		private void EnsureCapacity(int min)
		{
			if (this._items.Length < min)
			{
				int num = (this._items.Length == 0) ? 4 : (this._items.Length * 2);
				if (num < min)
				{
					num = min;
				}
				this.Capacity = num;
			}
		}

		/// <summary>
		/// Enumerates over the <see cref="EventedList&lt;T&gt;"/>.
		/// </summary>
		[Serializable,
		System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
		public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
		{
			private EventedList<T> list;
			private int index;
			private int version;
			private T current;

			/// <summary>
			/// Initializes a new instance of the <see cref="EventedList&lt;T&gt;.Enumerator"/> struct.
			/// </summary>
			/// <param name="list">The list.</param>
			internal Enumerator(EventedList<T> list)
			{
				this.list = list;
				this.index = 0;
				this.version = list._version;
				this.current = default(T);
			}

			/// <summary>
			/// Gets the current.
			/// </summary>
			/// <value>The current.</value>
			public T Current
			{
				get
				{
					return this.current;
				}
			}

			/// <summary>
			/// Gets the current.
			/// </summary>
			/// <value>The current.</value>
			object IEnumerator.Current
			{
				get
				{
					if ((this.index == 0) || (this.index == (this.list._size + 1)))
					{
						throw new InvalidOperationException();
					}
					return this.Current;
				}
			}

			/// <summary>
			/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
			/// </summary>
			public void Dispose()
			{
			}

			/// <summary>
			/// Sets the enumerator to its initial position, which is before the first element in the collection.
			/// </summary>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
			void IEnumerator.Reset()
			{
				if (this.version != this.list._version)
				{
					throw new InvalidOperationException();
				}
				this.index = 0;
				this.current = default(T);
			}

			/// <summary>
			/// Advances the enumerator to the next element of the collection.
			/// </summary>
			/// <returns>
			/// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
			/// </returns>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
			public bool MoveNext()
			{
				if (this.version != this.list._version)
				{
					throw new InvalidOperationException();
				}
				if (this.index < this.list._size)
				{
					this.current = this.list._items[this.index];
					this.index++;
					return true;
				}
				this.index = this.list._size + 1;
				this.current = default(T);
				return false;
			}
		}

		/// <summary>
		/// An <see cref="EventArgs"/> structure passed to events generated by an <see cref="EventedList{T}"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
#pragma warning disable 693
		public class ListChangedEventArgs<T> : EventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="EventedList&lt;T&gt;.ListChangedEventArgs&lt;T&gt;"/> class.
			/// </summary>
			/// <param name="type">The type of change.</param>
			public ListChangedEventArgs(ListChangedType type)
			{
				this.ItemIndex = -1;
				this.ListChangedType = type;
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="EventedList&lt;T&gt;.ListChangedEventArgs&lt;T&gt;"/> class.
			/// </summary>
			/// <param name="type">The type of change.</param>
			/// <param name="item">The item that has changed.</param>
			/// <param name="itemIndex">Index of the changed item.</param>
			public ListChangedEventArgs(ListChangedType type, T item, int itemIndex)
			{
				this.Item = item;
				this.ItemIndex = itemIndex;
				this.ListChangedType = type;
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="EventedList&lt;T&gt;.ListChangedEventArgs&lt;T&gt;"/> class.
			/// </summary>
			/// <param name="type">The type of change.</param>
			/// <param name="item">The item that has changed.</param>
			/// <param name="itemIndex">Index of the changed item.</param>
			/// <param name="oldItem">The old item when an item has changed.</param>
			public ListChangedEventArgs(ListChangedType type, T item, int itemIndex, T oldItem)
				: this(type, item, itemIndex)
			{
				OldItem = oldItem;
			}

			/// <summary>
			/// Gets the item that has changed.
			/// </summary>
			/// <value>The item.</value>
			public T Item
			{
				get; private set;
			}

			/// <summary>
			/// Gets the index of the item.
			/// </summary>
			/// <value>The index of the item.</value>
			public int ItemIndex
			{
				get; private set;
			}

			/// <summary>
			/// Gets the type of change for the list.
			/// </summary>
			/// <value>The type of change for the list.</value>
			public ListChangedType ListChangedType
			{
				get; private set;
			}

			/// <summary>
			/// Gets the item's previous value.
			/// </summary>
			/// <value>The old item.</value>
			public T OldItem
			{
				get; private set;
			}
		}
#pragma warning restore 693
	}
}