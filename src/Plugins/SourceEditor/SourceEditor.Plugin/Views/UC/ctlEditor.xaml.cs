using System;
using System.Windows.Controls;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.SourceEditor.Plugin.Views.UC
{
	/// <summary>
	///		Control de usuario para el editor
	/// </summary>
	public partial class ctlEditor : UserControl
	{ 
		// Eventos públicos
		public event EventHandler TextChanged;
		// Variables privadas
		private string fileName;

		public ctlEditor()
		{
			InitializeComponent();
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
				}
				// Cambia el modo de resalte del archivo
				ChangeHighLightByExtension(extension);
		}

		/// <summary>
		///		Cambia el modo de resalta a partir de una extensión
		/// </summary>
		internal void ChangeHighLightByExtension(string extension)
		{
			txtEditor.ChangeHighLightByExtension(extension);
		}

		/// <summary>
		///		Inserta un texto en el editor
		/// </summary>
		internal void InsertText(string code)
		{
			txtEditor.InsertText(code);
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
		public BauMvvm.ViewModels.BaseObservableObject ViewModel { get; set; }

		private void txtEditor_TextChanged(object sender, EventArgs e)
		{
			TextChanged?.Invoke(this, EventArgs.Empty);
			if (ViewModel != null)
				ViewModel.IsUpdated = true;
		}
	}
}
