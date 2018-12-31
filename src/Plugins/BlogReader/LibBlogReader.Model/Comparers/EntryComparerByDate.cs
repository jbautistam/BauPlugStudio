using System;

namespace Bau.Libraries.LibBlogReader.Model.Comparers
{
	/// <summary>
	///		Comparador de <see cref="EntryModel"/> por fecha
	/// </summary>
	internal class EntryComparerByDate : LibDataStructures.Tools.Comparers.AbstractBaseComparer<EntryModel>
	{
		internal EntryComparerByDate(bool ascending) : base(ascending) { }

		/// <summary>
		///		Compara dos <see cref="EntryModel"/> por fecha
		/// </summary>
		protected override int CompareData(EntryModel first, EntryModel second)
		{
			return first.DatePublish.CompareTo(second.DatePublish);
		}
	}
}
