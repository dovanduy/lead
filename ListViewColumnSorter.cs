using System;
using System.Collections;
using System.Windows.Forms;

public class ListViewColumnSorter : IComparer
{
	private int ColumnToSort;

	private SortOrder OrderOfSort;

	private CaseInsensitiveComparer ObjectCompare;

	public SortOrder Order
	{
		get
		{
			return this.OrderOfSort;
		}
		set
		{
			this.OrderOfSort = value;
		}
	}

	public int SortColumn
	{
		get
		{
			return this.ColumnToSort;
		}
		set
		{
			this.ColumnToSort = value;
		}
	}

	public ListViewColumnSorter()
	{
		this.ColumnToSort = 0;
		this.OrderOfSort = SortOrder.None;
		this.ObjectCompare = new CaseInsensitiveComparer();
	}

	public int Compare(object x, object y)
	{
		int num;
		ListViewItem listViewItem = (ListViewItem)x;
		ListViewItem listViewItem1 = (ListViewItem)y;
		int num1 = this.ObjectCompare.Compare(listViewItem.SubItems[this.ColumnToSort].Text, listViewItem1.SubItems[this.ColumnToSort].Text);
		if (this.OrderOfSort != SortOrder.Ascending)
		{
			num = (this.OrderOfSort != SortOrder.Descending ? 0 : -num1);
		}
		else
		{
			num = num1;
		}
		return num;
	}
}