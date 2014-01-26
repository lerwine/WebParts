using System;
using System.Text;
using System.Collections;

namespace HtmlBulletedList
{
	public class HtmlListItemCollection : ICollection, IEnumerable
	{
		private HtmlBulletedList htmlBulletedList;
		private ArrayList listItems;

		public HtmlListItemCollection(HtmlBulletedList owner)
		{
			this.htmlBulletedList = owner;
			this.listItems = new ArrayList();
		}

		public HtmlListItem this[int index]
		{
			get
			{
				return (HtmlListItem)(this.listItems[index]);
			}
		}

		protected HtmlBulletedList Owner
		{
			get
			{
				return this.htmlBulletedList;
			}
		}

		public void Add(HtmlListItem item)
		{
			this.listItems.Add((object)item);
			this.htmlBulletedList.Controls.Add(item);
		}

		public void AddAt(int index, HtmlListItem item)
		{
			int i;

			this.listItems.Insert(index, (object)item);
			if (index == this.listItems.Count - 1 || (i = this.htmlBulletedList.Controls.IndexOf(item)) < 0)
			{
				this.htmlBulletedList.Controls.Add(item);
				return;
			}
			this.htmlBulletedList.Controls.AddAt(i, item);
		}

		public void Clear()
		{
			foreach (HtmlListItem item in this.listItems)
			{
				if (this.htmlBulletedList.Controls.Contains(item))
					this.htmlBulletedList.Controls.Remove(item);
			}
			this.listItems.Clear();
		}

		public bool Contains(HtmlListItem item)
		{
			return this.listItems.Contains((object)item);
		}

		public int IndexOf(HtmlListItem item)
		{
			return this.listItems.IndexOf((object)item);
		}

		public void Remove(HtmlListItem item)
		{
			this.listItems.Remove((object)item);
		}

		public void RemoveAt(int index)
		{
			if (this.htmlBulletedList.Controls.Contains((System.Web.UI.Control)(this.listItems[index])))
				this.htmlBulletedList.Controls.Remove((System.Web.UI.Control)(this.listItems[index]));

			this.listItems.RemoveAt(index);
		}

		#region ICollection Members

		public void CopyTo(Array array, int index)
		{
			IEnumerator ie;

			ie = this.listItems.GetEnumerator();
			while (ie.MoveNext())
			{
				array.SetValue(ie.Current, index);
				index++;
			}
		}

		public int Count
		{
			get
			{
				return this.listItems.Count;
			}
		}

		public bool IsSynchronized
		{
			get
			{
				return this.listItems.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return this.listItems.SyncRoot;
			}
		}

		#endregion

		#region IEnumerable Members

		public System.Collections.IEnumerator GetEnumerator()
		{
			return new HtmlListEnumerator(this.listItems);
		}

		#endregion
	}

	public class HtmlListEnumerator : IEnumerator
	{
		int index;
		ArrayList currentList;

		public HtmlListEnumerator(ArrayList items)
		{
			this.index = -1;
			this.currentList = items;
		}

		#region IEnumerator Members

		public object Current
		{
			get
			{
				if (this.index < 0 || this.currentList == null || this.index >= this.currentList.Count)
					throw new Exception("Index out of range.");
				return this.currentList[this.index];
			}
		}

		public bool MoveNext()
		{
			if (this.currentList == null || this.index == this.currentList.Count)
				return false;

			this.index++;

			return (this.index != this.currentList.Count);
		}

		public void Reset()
		{
			this.index = -1;
		}

		#endregion
	}
}
