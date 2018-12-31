using System;

namespace Bau.Libraries.LibDocWriter.Processor.Steps.SitemapSteps
{
	/// <summary>
	///		Entrada de un SiteMap
	/// </summary>
	/// <example>
	///		<![CDATA[
	///		 <url>
	///				<loc>http://www.example.com/</loc>
	///				<lastmod>2005-01-01</lastmod>
	///				<changefreq>monthly</changefreq>
	///				<priority>0.8</priority>
	///			</url>
	///		]]>
	/// </example>
	internal class SitemapEntry
	{ 
		// Enumerados públicos
		public enum Frequency
		{
			Always,
			Hourly,
			Daily,
			Weekly,
			Monthly,
			Yearly,
			Never
		}

		internal SitemapEntry(string fileName, DateTime updatedAt, Frequency changeFrequence = Frequency.Always, double priority = 0.5)
		{ 
			FileName = fileName;
			DateLastUpdate = updatedAt;
			ChangeFrequence = changeFrequence;
			Priority = priority;
		}

		/// <summary>
		///		Nombre de archivo
		/// </summary>
		internal string FileName { get; set; }

		/// <summary>
		///		Fecha de última modificación
		/// </summary>
		internal DateTime DateLastUpdate { get; set; }

		/// <summary>
		///		Frecuencia de modificación
		/// </summary>
		internal Frequency ChangeFrequence { get; set; }

		/// <summary>
		///		Prioridad
		/// </summary>
		internal double Priority { get; set; }
	}
}
