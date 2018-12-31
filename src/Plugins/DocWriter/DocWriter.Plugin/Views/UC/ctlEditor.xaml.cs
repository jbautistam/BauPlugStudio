using System;
using System.Windows;
using System.Windows.Controls;

using ICSharpCode.AvalonEdit.Highlighting;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDocWriter.ViewModel.Solutions.TreeExplorer;
using Bau.Libraries.BauMvvm.Views.Forms.Trees;

namespace Bau.Plugins.DocWriter.Views.UC
{
	/// <summary>
	///		Control de usuario para el editor
	/// </summary>
	public partial class ctlEditor : UserControl
	{ 
		// Eventos públicos
		public event EventHandler TextChanged;
		public event EventHandler InsertInstructionRequired;
		// Variables privadas
		private string fileName;
		private DragDropTreeExplorerController dragDropController = new DragDropTreeExplorerController();

		public ctlEditor()
		{
			InitializeComponent();
		}

		/// <summary>
		///		Cambia el modo de resalte de un recurso
		/// </summary>
		public bool ChangeHighligtByResource(string resource)
		{
			bool changed = false;

				// Carga el recurso
				try
				{
					using (System.IO.Stream stm = Application.GetResourceStream(new Uri(resource)).Stream)
					{
						LoadHighLight(stm);
					}
				}
				catch (Exception exception)
				{
					System.Diagnostics.Debug.WriteLine($"Error al cambiar el modo de resalte. {exception.Message}");
				}
				// Devuelve el valor que indica que se ha cargado
				return changed;
		}

		/// <summary>
		///		Cambia el modo de resalte
		/// </summary>
		public void LoadHighLight(string fileName)
		{
			using (System.IO.Stream file = System.IO.File.OpenRead(fileName))
			{ 
				// Carga desde el stream
				LoadHighLight(file);
				// Cierra el stream
				file.Close();
			}
		}

		/// <summary>
		///		Cambia el modo de resalte desde un stream
		/// </summary>
		public void LoadHighLight(System.IO.Stream stmFile)
		{
			using (System.Xml.XmlTextReader rdrXml = new System.Xml.XmlTextReader(stmFile))
			{   
				// Cambia el modo de resalte
				txtEditor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(rdrXml, HighlightingManager.Instance);
				// Cierra el lector
				rdrXml.Close();
			}
		}

		/// <summary>
		///		Cambia el modo de resalte a partir del nombre de archivo
		/// </summary>
		private void ChangeHighLight(string fileName)
		{
			string extension = ".cs";

				// Obtiene la extensión del archivo
				if (!fileName.IsEmpty())
				{ 
					// Obtiene la extensión del archivo
					extension = System.IO.Path.GetExtension(fileName);
					// Normaliza la extensión
					if (extension.IsEmpty())
						extension = ".cs";
					else if (extension.EqualsIgnoreCase(".scss"))
						extension = ".css";
				}
				// Cambia el modo de resalte del archivo
				txtEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(extension);
		}

		/// <summary>
		///		Inserta un texto en la posición actual
		/// </summary>
		public void InsertText(string text, int intOffset = 0)
		{
			txtEditor.Document.Insert(txtEditor.CaretOffset, text);
			if (intOffset < 0)
				txtEditor.CaretOffset += intOffset;
			txtEditor.Focus();
		}

		/// <summary>
		///		Muestra los enlaces a las URL de archivos en el documento
		/// </summary>
		internal void ShowUrlFiles(string [] filesTarget, Libraries.LibDocWriter.ViewModel.Solutions.EventArguments.EndFileCopyEventArgs.CopyImageType idCopyMode)
		{
			//TODO --> Quitar esto
			switch (idCopyMode)
			{
				case Libraries.LibDocWriter.ViewModel.Solutions.EventArguments.EndFileCopyEventArgs.CopyImageType.Normal:
						for (int index = 0; index < filesTarget.Length; index++)
							if (!filesTarget [index].IsEmpty() && IsImage(filesTarget [index]))
							{ 
								// Inserta el párrafo
								InsertText(Environment.NewLine + "%p { style = \"text-align:center\"}");
								// Inserta el vínculo
								InsertLink(filesTarget [index], System.IO.Path.GetFileNameWithoutExtension(filesTarget [index]));
							}
					break;
				case Libraries.LibDocWriter.ViewModel.Solutions.EventArguments.EndFileCopyEventArgs.CopyImageType.Gallery:
					int intCell = 0;

						// Cabecera de tabla
						InsertText(Environment.NewLine + "\t%table");
						// Cuerpo de tabla
						for (int index = 0; index < filesTarget.Length; index++)
							if (!filesTarget [index].IsEmpty() && IsImage(filesTarget [index]))
							{ // Cabecera de fila
								if (intCell % 3 == 0)
									InsertText(Environment.NewLine + "\t\t%tr");
								// Celda
								InsertText(Environment.NewLine + "\t\t\t%td");
								InsertText(string.Format(" #a {{ href=\"{0}\" target=\"_blank\" }} #img {{ src = \"{1}\" alt = \"{2}\"}} # #",
														 filesTarget [index],
														 System.IO.Path.Combine(System.IO.Path.GetDirectoryName(filesTarget [index]),
																				"th_" + System.IO.Path.GetFileName(filesTarget [index])),
														 System.IO.Path.GetFileNameWithoutExtension(filesTarget [index])));
								// Incrementa la celda
								intCell++;
							}
					break;
			}
		}

		/// <summary>
		///		Inserta un vínculo
		/// </summary>
		private void InsertLink(string link, string title)
		{
			if (!link.IsEmpty())
			{   
				// Crea el vínculo adecuado para una imagen o una referencia
				if (link.EndsWith(".js", StringComparison.CurrentCultureIgnoreCase))
					link = "%script { src=\"" + link + "\" }";
				else if (link.EndsWith(".css", StringComparison.CurrentCultureIgnoreCase) ||
								 link.EndsWith(".scss", StringComparison.CurrentCultureIgnoreCase))
					link = "%link { href=\"" + link + "\" rel=\"stylesheet\" }";
				else if (IsImage(link))
					link = " #img { src = \"" + link + "\" alt = \"" + title + "\" } # ";
				else
					link = " #a { href = \"" + link + "\" } " + title + " # ";
				// Añade el vínculo
				InsertText(link);
			}
		}

		/// <summary>
		///		Comprueba si un vínculo representa una imagen
		/// </summary>
		private bool IsImage(string link)
		{
			return link.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase) ||
				   link.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) ||
				   link.EndsWith(".gif", StringComparison.CurrentCultureIgnoreCase) ||
				   link.EndsWith(".bmp", StringComparison.CurrentCultureIgnoreCase) ||
				   link.EndsWith(".tif", StringComparison.CurrentCultureIgnoreCase);
		}

		/// <summary>
		///		Copia / corta en el portapales el texto seleccionado
		/// </summary>
		private void CopyText(bool cut)
		{
			if (!txtEditor.SelectedText.IsEmpty())
			{ 
				// Pasa el texto al portapapeles
				Clipboard.SetText(txtEditor.SelectedText);
				// Si lo tiene que cortar, lo quita del texto
				if (cut)
					txtEditor.SelectedText = "";
			}
		}

		/// <summary>
		///		Pega el texto del portapapeles
		/// </summary>
		private void PasteText()
		{
			string text = Clipboard.GetText();

				if (!text.IsEmpty())
					InsertText(text);
		}

		/// <summary>
		///		Pega el texto del portapapeles como HTML
		/// </summary>
		private void PasteTextAsHtml()
		{
			string text = Clipboard.GetText();

				if (!text.IsEmpty())
				{
					string html = "";

						// Codifica el HTML
						foreach (char chrChar in text)
							if (chrChar == '<')
								html += "&lt;";
							else if (chrChar == '>')
								html += "&gt;";
							else if (chrChar == '&')
								html += "&amp;";
							else
								html += chrChar;
						// Pega el texto
						InsertText(html);
				}
		}

		/// <summary>
		///		Elimina el texto seleccionado
		/// </summary>
		private void DeleteText()
		{
			if (!txtEditor.SelectedText.IsEmpty())
				txtEditor.SelectedText = "";
		}

		/// <summary>
		///		Texto
		/// </summary>
		public string Text
		{
			get { return txtEditor.Text; }
			set { txtEditor.Text = value; }
		}

		/// <summary>
		///		Nombre del archivo
		/// </summary>
		public string FileName
		{
			get { return fileName; }
			set
			{
				fileName = value;
				ChangeHighLight(fileName);
			}
		}

		/// <summary>
		///		ViewModel
		/// </summary>
		public Libraries.BauMvvm.ViewModels.BaseObservableObject ViewModel { get; set; }

		private void txtEditor_TextChanged(object sender, EventArgs e)
		{
			TextChanged?.Invoke(this, EventArgs.Empty);
			if (ViewModel != null)
				ViewModel.IsUpdated = true;
		}

		private void txtEditor_DragEnter(object sender, DragEventArgs e)
		{
			dragDropController.TreatDragEnter(e);
		}

		private void txtEditor_Drop(object sender, DragEventArgs e)
		{
			FileNodeViewModel fileNode = dragDropController.GetDragDropFileNode(e.Data) as FileNodeViewModel;

			// Inserta el vínculo
			if (fileNode != null)
				InsertLink(fileNode.File.IDFileName, fileNode.File.Title);
			// Selecciona este control
			this.Focus();
			txtEditor.Focus();
		}

		private void mnuCopy_Click(object sender, RoutedEventArgs e)
		{
			CopyText(false);
		}

		private void mnuCut_Click(object sender, RoutedEventArgs e)
		{
			CopyText(true);
		}

		private void mnuPaste_Click(object sender, RoutedEventArgs e)
		{
			PasteText();
		}

		private void mnuPasteHtml_Click(object sender, RoutedEventArgs e)
		{
			PasteTextAsHtml();
		}

		private void mnuDelete_Click(object sender, RoutedEventArgs e)
		{
			DeleteText();
		}

		private void mnuInsertInstruction_Click(object sender, RoutedEventArgs e)
		{
			InsertInstructionRequired?.Invoke(this, EventArgs.Empty);
		}
	}
}
