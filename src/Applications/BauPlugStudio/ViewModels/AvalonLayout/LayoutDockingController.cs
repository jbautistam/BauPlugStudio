using System;
using System.Linq;
using Xceed.Wpf.AvalonDock.Layout;

using Bau.Libraries.BauMvvm.Common.Controllers;
using Bau.Libraries.BauMvvm.ViewModels.Forms;
using Bau.Libraries.BauMvvm.Views.Forms;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;

namespace Bau.Applications.BauPlugStudio.ViewModels.AvalonLayout
{
	/// <summary>
	///		Controlador de AvalonDock
	/// </summary>
	public class LayoutDockingController
	{   
		// Eventos públicos
		public event EventHandler<LayoutDockingEventArgs> ActiveDocumentChanged;
		// Variables privadas
		private PaneViewModel _activeDocument;

		public LayoutDockingController(Xceed.Wpf.AvalonDock.DockingManager dockManager)
		{ 
			DockManager = dockManager;
			DockManager.ActiveContentChanged += (sender, evntArgs) =>
													{
														if (DockManager.ActiveContent != null && DockManager.Layout != null && DockManager.Layout.ActiveContent != null)
															ActiveDocument = GetPaneViewModel(DockManager.Layout.ActiveContent.ContentId);
														else
															ActiveDocument = null;
													};
		}

		/// <summary>
		///		Muestra un panel
		/// </summary>
		public void ShowPane(string windowID, string title, System.Windows.Controls.UserControl control, 
							 SystemControllerEnums.DockPosition position = SystemControllerEnums.DockPosition.Bottomm)
		{
			LayoutContent previous = GetLayoutPrevious(windowID);

				if (previous != null)
					previous.IsActive = true;
				else
				{
					LayoutAnchorGroup layoutGroup = GetGroupPane(DockManager.Layout, position);
					LayoutAnchorable layoutPane = new LayoutAnchorable { Title = title, ToolTip = title };

						// Añade el contenido
						layoutPane.Content = control;
						layoutPane.ContentId = windowID;
						// Añade el contenido al grupo
						layoutGroup.Children.Add(layoutPane);
						// Añade el panel a la lista de documentos del controlador
						AddDocument(new ToolViewModel(windowID, title, layoutPane, control, position));
				}
		}

		/// <summary>
		///		Muestra un documento
		/// </summary>
		public void ShowDocument(string windowID, string title, System.Windows.Controls.UserControl control)
		{
			LayoutContent previous = GetLayoutPrevious(windowID);

				if (previous != null && previous.Parent != null)
					previous.IsActive = true;
				else
				{
					LayoutDocumentPane documentPane = DockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
					LayoutDocument layoutDocument = new LayoutDocument { Title = title, ToolTip = title };

						// Crea un documento y le asigna el control de contenido
						if (documentPane != null)
						{ 
							// Asigna el control
							layoutDocument.Content = control;
							layoutDocument.ContentId = windowID;
							// Añade el nuevo LayoutDocument al array existente
							documentPane.Children.Add(layoutDocument);
							// Activa el documento
							layoutDocument.IsActive = true;
							layoutDocument.IsSelected = true;
							// Cambia el foco al control
							control.Focus();
							// Añade el documento al controlador
							AddDocument(new DocumentViewModel(windowID, title, layoutDocument, control));
						}
				}
		}

		/// <summary>
		///		Obtiene la ventana que se ha abierto anteriormente para el código pasado como parámetro
		/// </summary>
		private LayoutContent GetLayoutPrevious(string windowID)
		{
			// Busca el panel entre los documentos del diccionario
			if (Documents.TryGetValue(windowID, out PaneViewModel viewModel))
				return viewModel?.LayoutPane;
			// Si ha llegado hasta aquí es porque no existía
			return null;
		}

		/// <summary>
		///		Obtiene el <see cref="PaneViewModel"/> a partir de un ID de ventana
		/// </summary>
		private PaneViewModel GetPaneViewModel(string windowID)
		{
			// Busca el panel en el diccionario
			if (Documents.TryGetValue(windowID, out PaneViewModel viewModel))
				return viewModel;
			else
				return null;
		}

		/// <summary>
		///		Comprueba si existe un  documento
		/// </summary>
		private bool ExistsDocument(string windowID)
		{
			return Documents.ContainsKey(windowID);
		}

		/// <summary>
		///		Añade un documento a la lista de documentos tratados
		/// </summary>
		private void AddDocument(PaneViewModel paneViewModel)
		{ 
			// Añade el documento
			if (!ExistsDocument(paneViewModel.WindowID))
				Documents.Add(paneViewModel.WindowID, paneViewModel);
			// Asigna los manejadores de evento a la vista
			if (paneViewModel.ContentControl != null && paneViewModel.ContentControl is IFormView)
				paneViewModel.LayoutPane.Closing += (sender, evntArgs) => evntArgs.Cancel = !TreatEventCloseForm(paneViewModel);
			// Activa el documento
			ActiveDocument = paneViewModel;
		}

		/// <summary>
		///		Obtiene el grupo de ventanas del panel de la posición especificada
		/// </summary>
		private LayoutAnchorGroup GetGroupPane(LayoutRoot layoutRoot, SystemControllerEnums.DockPosition position)
		{
			LayoutAnchorSide layoutSide = GetAnchorSide(layoutRoot, position);

				// Crea el panel si no existía
				if (layoutSide == null)
					layoutSide = new LayoutAnchorSide();
				// Añade un grupo si no existía
				if (layoutSide.Children == null || layoutSide.Children.Count == 0)
					layoutSide.Children.Add(new LayoutAnchorGroup());
				// Devuelve el primer grupo
				return layoutSide.Children[0];
		}

		/// <summary>
		///		Obtiene uno de los elementos laterales del gestor de ventanas
		/// </summary>
		private LayoutAnchorSide GetAnchorSide(LayoutRoot layoutRoot, SystemControllerEnums.DockPosition position)
		{
			switch (position)
			{
				case SystemControllerEnums.DockPosition.Right:
					return layoutRoot.RightSide;
				case SystemControllerEnums.DockPosition.Left:
					return layoutRoot.LeftSide;
				case SystemControllerEnums.DockPosition.Top:
					return layoutRoot.TopSide;
				case SystemControllerEnums.DockPosition.Bottomm:
					return layoutRoot.BottomSide;
				default:
					throw new NotImplementedException("Posición desconocida");
			}
		}

		/// <summary>
		///		Trata el evento de cierre de un documento
		/// </summary>
		private bool TreatEventCloseForm(PaneViewModel paneViewModel)
		{
			bool canClose = true;
			IFormView formView = paneViewModel?.GetFormView();

				// Comprueba el ViewModel del control para comprobar si se puede cerrar
				if (formView != null && formView.FormView.ViewModel.IsUpdated)
				{
					SystemControllerEnums.ResultType result;

						// Pregunta si se debe grabar el documento
						result = Globals.HostController.ControllerWindow.ShowQuestionCancel("Se han realizado modificaciones que aún no se han grabado. ¿Desea grabarlas ahora?");
						// Graba el documento si es necesario
						if (result == SystemControllerEnums.ResultType.Yes)
						{ 
							// Graba el documento
							(formView.FormView.ViewModel as BaseFormViewModel)?.SaveCommand.Execute(null);
							// Comprueba si se puede cerrar la ventana
							canClose = !formView.FormView.IsUpdated;
						}
						else
							canClose = result != SystemControllerEnums.ResultType.Cancel;
				}
				// Si se puede borrar, elimina la ventana de la colección
				if (canClose)
				{
					Documents.Remove(paneViewModel.WindowID);
					formView.FormView.CloseViewModel();
				}
				// Devuelve el valor que indica si se puede cerrar
				return canClose;
		}

		/// <summary>
		///		Graba los documentos
		/// </summary>
		internal void SaveAllDocuments()
		{
			foreach (System.Collections.Generic.KeyValuePair<string, PaneViewModel> keyDocument in Documents)
				if (keyDocument.Value != null && !(keyDocument.Value is ToolViewModel))
				{
					BaseFormViewModel form = keyDocument.Value.GetFormView().FormView.ViewModel as BaseFormViewModel;

						if (form.IsUpdated)
							form.SaveCommand.Execute(null);
				}
		}

		/// <summary>
		///		Cierra todas las ventanas
		/// </summary>
		internal void CloseAllDocuments()
		{
			System.Collections.Generic.List<PaneViewModel> documents = new System.Collections.Generic.List<PaneViewModel>();

			// Nota: Al cerrar un formulario se modifica la colección Documents, por tanto no se puede hacer un recorrido sobre Documents
			//			 porque da un error de colección modificada. Tampoco se puede hacer un recorrido for (int...) sobre Documents porque
			//			 es un diccionario y no tiene indizador. Por tanto, tenemos que copiar los elementos del diccionario sobre una lista
			//			 y después recorrer los elementos de esta lista copiada desde el final hasta el principio.
			// Añade el diccionario de documentos a la lista
			foreach (System.Collections.Generic.KeyValuePair<string, PaneViewModel> document in Documents)
				documents.Add(document.Value);
			// Recorre la lista cerrando todos los documentos aviertos
			for (int index = documents.Count - 1; index >= 0; index--)
				if (documents [index] != null && documents [index] is DocumentViewModel)
				{
					DocumentViewModel form = documents [index] as DocumentViewModel;

						if (form?.LayoutPane != null)
							try
							{
								form.LayoutPane.Close();
							}
							catch { }
				}
		}

		/// <summary>
		///		Lanza el evento de cambio de documento
		/// </summary>
		private void RaiseEventChangeDocument()
		{
			ActiveDocumentChanged?.Invoke(this, new LayoutDockingEventArgs(ActiveDocument));
		}

		/// <summary>
		///		Manager de la ventana principal
		/// </summary>
		public Xceed.Wpf.AvalonDock.DockingManager DockManager { get; }

		/// <summary>
		///		Documentos
		/// </summary>
		public System.Collections.Generic.Dictionary<string, PaneViewModel> Documents { get; } = new System.Collections.Generic.Dictionary<string, PaneViewModel>();

		/// <summary>
		///		Documento activo
		/// </summary>
		public PaneViewModel ActiveDocument
		{
			get { return _activeDocument; }
			private set
			{
				_activeDocument = value;
				RaiseEventChangeDocument();
			}
		}

		/// <summary>
		///		Tema de la ventana principal
		/// </summary>
		public string DockTheme
		{
			get { return Globals.HostController.HostViewModelController.Configuration.Parameters.GetValue(Globals.ApplicationName, "DockTheme"); }
			set { Globals.HostController.HostViewModelController.Configuration.Parameters.SetValue(Globals.ApplicationName, "DockTheme", value); }
		}
	}
}