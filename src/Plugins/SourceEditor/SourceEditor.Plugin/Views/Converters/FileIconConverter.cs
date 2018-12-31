using System;
using System.Windows.Data;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.SourceEditor.Model.Definitions;
using Bau.Libraries.SourceEditor.Model.Solutions;

namespace Bau.Libraries.SourceEditor.Plugin.Views.Converters
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
			string icon = "/BauControls;component/Themes/Images/File.png";

				// Dependiendo de si el objeto es un archivo o un nodo propietario
				if (value is FileModel)
				{
					FileModel file = value as FileModel;

						// Convierte la entrada según el estado en el icono asociado
						if (file != null)
						{
							AbstractDefinitionModel fileDefinition = file.FileDefinition;

								if (!fileDefinition.Icon.IsEmpty())
									icon = fileDefinition.Icon;
								else if (file is SolutionModel)
									icon = "/BauControls;component/Themes/Images/Template.png";
								else if (file is ProjectModel)
									icon = "/BauControls;component/Themes/Images/Section.png";
								else if (file.IsFolder)
									icon = "/BauControls;component/Themes/Images/Folder.png";
								else if (LibCommonHelper.Files.HelperFiles.CheckIsImage(file.FullFileName))
									icon = "/BauControls;component/Themes/Images/Image.png";
						}
				}
				else if (value is OwnerChildModel)
				{
					OwnerChildModel owner = value as OwnerChildModel;

						// Obtiene el icono
						if (owner != null && !owner.Definition.Icon.IsEmpty())
							icon = owner.Definition.Icon;
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
