using System;

using Bau.Libraries.MVVM.ViewModels.ComboItems;
using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.ViewModel.Helper
{
	/// <summary>
	///		Helper para asignación de combos
	/// </summary>
	internal class ComboViewHelper
	{
		internal ComboViewHelper(BauMvvm.ViewModels.BaseObservableObject viewModelParent)
		{
			ViewModelParent = viewModelParent;
		}

		/// <summary>
		///		Carga los elementos de un combo con los tipos de Webs
		/// </summary>
		internal ComboViewModel LoadComboWebTypes(string propertyName)
		{
			ComboViewModel cboWebType = new ComboViewModel(ViewModelParent, propertyName);

				// Añade los elementos con los tipos de Web
				cboWebType.AddItem((int) ProjectModel.WebDefinitionType.Web, "Web");
				cboWebType.AddItem((int) ProjectModel.WebDefinitionType.EBook, "eBook");
				cboWebType.AddItem((int) ProjectModel.WebDefinitionType.Template, "Template");
				// Selecciona el primer elemento
				cboWebType.SelectedID = (int) ProjectModel.WebDefinitionType.Web;
				// Devuelve el combo
				return cboWebType;
		}

		/// <summary>
		///		Carga el combo de tipos de documentos
		/// </summary>
		internal ComboViewModel GetComboDocumentTypes(string propertyName)
		{
			ComboViewModel cboCombo = new ComboViewModel(ViewModelParent, propertyName);

				// Añade los elementos
				cboCombo.AddItem(null, "<Seleccione un elemento>");
				cboCombo.AddItem((int) FileModel.DocumentType.Document, "Documento");
				cboCombo.AddItem((int) FileModel.DocumentType.Tag, "Etiqueta");
				cboCombo.AddItem((int) FileModel.DocumentType.Template, "Plantilla");
				cboCombo.AddItem((int) FileModel.DocumentType.Section, "Sección");
				cboCombo.AddItem((int) FileModel.DocumentType.SiteMap, "Mapa del sitio");
				cboCombo.AddItem((int) FileModel.DocumentType.File, "Otros");
				// Selecciona el elemento vacío
				cboCombo.SelectedID = (int) FileModel.DocumentType.Document;
				// Devuelve el combo
				return cboCombo;
		}

		/// <summary>
		///		ViewModel padre de los combos
		/// </summary>
		internal BauMvvm.ViewModels.BaseObservableObject ViewModelParent { get; }
	}
}