using System;
using System.Windows.Data;

using Bau.Libraries.LibBlogReader.Model;

namespace Bau.Plugins.BlogReader.Views.Converters
{
	/// <summary>
	///		Conversor para los iconos asociados a las entradas de un blog (leído, no leído, interesante
	/// </summary>
	public class BlogEntryIconConverter : IValueConverter
	{
		/// <summary>
		///		Convierte un elemento <see cref="Libraries.LibBlogReader.ViewModel.Blogs.BlogEntryViewModel"/> en el icono asociado
		/// </summary>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			string icon = null;

				// Convierte la entrada según el estado en el icono asociado
				if (value is EntryModel.StatusEntry)
				{
					EntryModel.StatusEntry status = (EntryModel.StatusEntry) ((int) value);

						// Obtiene la dirección del icono
						switch (status)
						{
							case EntryModel.StatusEntry.Interesting:
									icon = "/BauControls;component/Themes/Images/EntryInteresting.png";
								break;
							case EntryModel.StatusEntry.NotRead:
									icon = "/BauControls;component/Themes/Images/EntryNotRead.png";
								break;
							case EntryModel.StatusEntry.Read:
									icon = "/BauControls;component/Themes/Images/EntryRead.png";
								break;
						}
				}
				// Devuelve la dirección del icono
				return icon;
		}

		/// <summary>
		///		Convierte el valor devuelto (no hace nada, simplemente implementa la interface)
		/// </summary>
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value;
		}
	}
}
