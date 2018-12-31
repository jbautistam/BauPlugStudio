using System;
using System.Windows.Controls;
using Bau.Libraries.BauMvvm.Views.Forms;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDocWriter.Model.Solutions;
using Bau.Libraries.LibDocWriter.ViewModel.Solutions;

namespace Bau.Plugins.DocWriter.Views
{
	/// <summary>
	///		Ventana para mostrar los datos de un archivo de DocWriter
	/// </summary>
	public partial class DocumentView : UserControl, IFormView
	{
		public DocumentView(SolutionModel solution, FileModel file)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa la vista de datos
			grdData.DataContext = ViewModel = new DocumentViewModel(solution, file);
			udtEditor.ViewModel = ViewModel;
			udtEditor.Text = ViewModel.Content;
			// Asigna la clase del documento
			FormView = new BaseFormView(ViewModel);
			// Cambia el modo de resalte a Nhtml
			//#if DEBUG // ... cuando queramos hacer pruebas con un archivo de definición
			//	try
			//		{ udtEditor.LoadHighLight(@"C:\Users\jbautistam\Downloads\NhtmlSyntaxHighLight.xml");
			//		}
			//	catch (Exception exception)
			//		{ DocWriterPlugin.MainInstance.HostController.ControllerWindow.ShowMessage("Error al cambiar el modo de resalte. " + exception.Message);
			//		}
			//#else
			try
			{
				using (System.IO.Stream stm = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/DocWriter.Plugin;Resources/NhtmlSyntaxHighLight.xml")).Stream)
				{
					udtEditor.LoadHighLight(stm);
				}
			}
			catch (Exception exception)
			{
				System.Diagnostics.Debug.WriteLine("Error al cambiar el modo de resalte. " + exception.Message);
			}
			//#endif
			// Inicializa las plantillas
			udtTemplates.Project = file.Project;
			udtImage.Project = file.Project;
			udtImage.FileType = FileModel.DocumentType.File;
			// Asigna el manejador de eventos
			ViewModel.EndFileCopy += new EventHandler<Libraries.LibDocWriter.ViewModel.Solutions.EventArguments.EndFileCopyEventArgs>(ViewModel_EndFileCopy);
			ViewModel.InsertTextEditor += (sender, evntArgs) => udtEditor.InsertText(evntArgs.Text, evntArgs.Offset);
		}

		/// <summary>
		///		Muestra las URL de las imágenes en el documento
		/// </summary>
		private void ShowImagesDocument(string [] files, Libraries.LibDocWriter.ViewModel.Solutions.EventArguments.EndFileCopyEventArgs.CopyImageType idCopyMode)
		{ 
			// Copia el primer nombre de archivo en el resumen de imágenes
			if (ViewModel.UrlImageSummary.IsEmpty())
			{
				ViewModel.UrlImageSummary = files [0];
				files [0] = null;
			}
			// Muestra los vínculos de imagen
			udtEditor.ShowUrlFiles(files, idCopyMode);
		}

		/// <summary>
		///		ViewModel asociado al documento
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		ViewModel asociado al documento
		/// </summary>
		public DocumentViewModel ViewModel { get; }

		private void udtEditor_Changed(object sender, EventArgs evntArgs)
		{
			ViewModel.Content = udtEditor.Text;
		}

		private void udtEditor_InsertInstructionRequired(object sender, EventArgs e)
		{
			ViewModel.ExecuteInsertInstruction();
		}

		/// <summary>
		///		Rutina de tratamiento del evento de copia de archivos
		/// </summary>
		private void ViewModel_EndFileCopy(object sender, Libraries.LibDocWriter.ViewModel.Solutions.EventArguments.EndFileCopyEventArgs e)
		{
			if (e.FilesTarget != null && e.FilesTarget.Length > 0)
				ShowImagesDocument(e.FilesTarget, e.CopyImageMode);
		}
	}
}
