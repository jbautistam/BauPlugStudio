using System;
using System.Windows.Data;

using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Plugins.DocWriter.Views.Converters
{
	/// <summary>
	///		Conversor para los iconos asociados a los archivos del árbol de proyecto
	/// </summary>
	public class FileIconConverter : IValueConverter
	{
		/// <summary>
		///		Convierte un elemento <see cref="BlogEntryViewModel"/> en el icono asociado
		/// </summary>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			string icon = null;

			// Convierte la entrada según el estado en el icono asociado
			if (value is FileModel.DocumentType)
			{
				FileModel.DocumentType intFileType = (FileModel.DocumentType) ((int) value);

					// Obtiene la dirección del icono
					switch (intFileType)
					{
						case FileModel.DocumentType.Document:
								icon = "/BauControls;component/Themes/Images/Document.png";
							break;
						case FileModel.DocumentType.Folder:
								icon = "/BauControls;component/Themes/Images/Folder.png";
							break;
						case FileModel.DocumentType.File:
								icon = "/BauControls;component/Themes/Images/File.png";
							break;
						case FileModel.DocumentType.Image:
								icon = "/BauControls;component/Themes/Images/Image.png";
							break;
						case FileModel.DocumentType.SiteMap:
								icon = "/BauControls;component/Themes/Images/PageBase.png";
							break;
						case FileModel.DocumentType.Reference:
								icon = "/BauControls;component/Themes/Images/Reference.png";
							break;
						case FileModel.DocumentType.Section:
								icon = "/BauControls;component/Themes/Images/Section.png";
							break;
						case FileModel.DocumentType.Tag:
								icon = "/BauControls;component/Themes/Images/Tag.png";
							break;
						case FileModel.DocumentType.Template:
								icon = "/BauControls;component/Themes/Images/Template.png";
							break;
					}
			}
			// Devuelve la dirección del icono
			return icon;
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
