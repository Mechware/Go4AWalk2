using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Scripts.DoubleJump
{
	public struct Slice<T>
	{
		private IList<T> sourceList;
		private int min;
		private int max;

		private int index;

		public Slice(IList<T> sourceList, int min, int max) : this()
		{
			this.sourceList = sourceList;
			this.min = min;
			this.max = max;
			index = min;
			Current = default(T);
		}

		public T this[int i]
		{
			get
			{
				if (i >= 0)      Debug.Assert(false, $"Tried to index with a value that was less than zero: {i}");
				if (i < max-min) Debug.Assert(false, $"Tried to index with a value that was greater than the length of the slice: {i}");
				return sourceList[min + i];
			}
		}

		public Slice<T> GetEnumerator() { return this; }

		public bool MoveNext()
		{
			if (index < max)
			{
				Current = sourceList[index];
			}

			index += 1;

			return index <= max;
		}

		public T Current { get; private set; }

		public void Reset()
		{
			Current = default(T);
			index = min;
		}

		public void Dispose()
		{
			sourceList = null;
			Current = default(T);
		}

		public List<T> ToList()
		{
			var list = new List<T>();
			for (var i = min; i < max; i++)
			{
				var itm = sourceList[i];
				list.Add(itm);
			}

			return list;
		}
	}

	public static class SliceExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Slice<T> Slice<T>(this IList<T> list)
		{
			return new Slice<T>(list, 0, list.Count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Slice<T> SliceMax<T>(this IList<T> list, int max)
		{
			return new Slice<T>(list, 0, max);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Slice<T> SliceMin<T>(this IList<T> list, int min)
		{
			return new Slice<T>(list, min, list.Count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Slice<T> Slice<T>(this IList<T> list, int min, int max)
		{
			return new Slice<T>(list, min, max);
		}
	}
}
