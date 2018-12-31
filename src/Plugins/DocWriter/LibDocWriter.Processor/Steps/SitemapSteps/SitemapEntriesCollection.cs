using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibDocWriter.Processor.Steps.SitemapSteps
{
	/// <summary>
	///		Colección de <see cref="SitemapEntry"/>
	/// </summary>
	internal class SitemapEntriesCollection : List<SitemapEntry>
	{
		/// <summary>
		///		Añade un archivo
		/// </summary>
		internal void Add(string fileName, DateTime createdAt)
		{
			Add(new SitemapEntry(fileName, createdAt));
		}

		/// <summary>
		///		Añade una colección de entradas
		/// </summary>
		internal void Add(SitemapEntriesCollection entries)
		{
			foreach (SitemapEntry entry in entries)
				Add(entry);
		}
	}
}
