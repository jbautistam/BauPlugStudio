using System;
using System.Windows.Data;

using Bau.Libraries.DatabaseStudio.ViewModels.Controllers;

namespace Bau.Libraries.FullDatabaseStudio.Plugin.Converters
{
	/// <summary>
	///		Conversor para los iconos asociados a los archivos del árbol de proyecto
	/// </summary>
	public class FileIconConverter : IValueConverter
	{
		/// <summary>
		///		Convierte un tipo en un icono
		/// </summary>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{ 
			if (value is DbScriptsViewModelEnums.NodeImage)
				return GetIcon((DbScriptsViewModelEnums.NodeImage) value);
			else
				return null;
		}

		/// <summary>
		///		Obtiene el icono asociado a un <see cref="DbScriptsViewModelEnums.NodeImage"/>
		/// </summary>
		private string GetIcon(DbScriptsViewModelEnums.NodeImage iconType)
		{	
			switch (iconType)
			{ 
				case DbScriptsViewModelEnums.NodeImage.Project:
					return "/FullDatabaseStudio.Plugin;component/Themes/Images/Project.png";
				case DbScriptsViewModelEnums.NodeImage.Folder:
					return "/FullDatabaseStudio.Plugin;component/Themes/Images/Folder.png";
				default:
					return "/FullDatabaseStudio.Plugin;component/Themes/Images/File.png";
			}
		}

		/// <summary>
		///		Convierte un valor de vuelta
		/// </summary>
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{ 
			throw new NotImplementedException();
		}
	}
}
