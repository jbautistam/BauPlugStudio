using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.FtpManager.ViewModel.FtpExplorer.FtpConnectionItems;
using Bau.Libraries.MVVM.Views.Controllers;

namespace Bau.Plugins.FtpManager.Views.FtpExplorer.UC
{
	/// <summary>
	///		Ventana que muestra el árbol de proyectos de SourceEditor
	/// </summary>
	public partial class FtpTreeFilesView : UserControl
	{ // Propiedades
			public static readonly DependencyProperty SourcePathProperty = 
								DependencyProperty.Register(nameof(SourcePath), typeof(string), typeof(FtpTreeFilesView),
																						new FrameworkPropertyMetadata("C:\\", 
																																					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
			public static readonly DependencyProperty SelectedPathProperty = 
								DependencyProperty.Register(nameof(SelectedPath), typeof(string), typeof(FtpTreeFilesView),
																						new FrameworkPropertyMetadata("C:\\", 
																																					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public static readonly DependencyProperty ShowFilesProperty = 
								DependencyProperty.Register(nameof(ShowFiles), typeof(bool), typeof(FtpTreeFilesView),
																						new FrameworkPropertyMetadata(true, 
																																					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		// Evenbtos
			public static readonly RoutedEvent SelectedPathChangedEvent = EventManager.RegisterRoutedEvent(nameof(SelectedPathChanged), RoutingStrategy.Bubble, 
																																																		 typeof(RoutedPropertyChangedEventHandler<string>), 
																																																		 typeof(FtpTreeFilesView));
		// Eventos públicos
			public event EventHandler<Libraries.FtpManager.ViewModel.FtpExplorer.EventArguments.FileEventArgs> OpenFile;
		// Variables privadas
			private Point pntStartDrag;
			private DragDropTreeExplorerController objDragDropController = new DragDropTreeExplorerController();

		public FtpTreeFilesView()
		{ InitializeComponent();
		}

		/// <summary>
		///		Inicializa el control
		/// </summary>
		internal void InitControl(FtpTreeExplorerViewModel objViewModel)
		{ ViewModelData = objViewModel;
			trvExplorer.DataContext = ViewModelData;
			trvExplorer.ItemsSource = ViewModelData.Nodes;
			ViewModelData.ChangedFile += (objSender, objEventArgs) => RaiseSelectedPathEvent();
		}

		/// <summary>
		///		Obtiene el nombre de archivo seleccionado
		/// </summary>
		public string GetSelectedFile()
		{ return (trvExplorer.SelectedItem as FtpFileNodeViewModel)?.File;
		}

		/// <summary>
		///		Actualiza el árbol
		/// </summary>
		public void Refresh()
		{ ViewModelData?.Refresh();	
		}

		/// <summary>
		///		Lanza el evento <see cref="SelectedPathChangedEvent"/>
		/// </summary>
    private void RaiseSelectedPathEvent()
    {	FtpFileNodeViewModel objFile = ViewModelData?.GetSelectedFile();

				if (objFile?.IsFolder ?? false)
					{ SelectedPath = objFile.File;
						RaiseEvent(new RoutedEventArgs(SelectedPathChangedEvent));
					}
    }

		/// <summary>
		///		ViewModel del formulario
		/// </summary>
		public FtpTreeExplorerViewModel ViewModelData { get; private set; }

		/// <summary>
		///		Directorio origen
		/// </summary>
		public string SourcePath
		{	get { return (string) GetValue(SourcePathProperty); }
			set 
				{ SetValue(SourcePathProperty, value); 
					ViewModelData.SourcePath = value;
				}
		}

		/// <summary>
		///		Directorio seleccionado
		/// </summary>
		public string SelectedPath
		{ get { return (string) GetValue(SelectedPathProperty); }
			set 
				{ SetValue(SelectedPathProperty, value);
					ViewModelData.SelectedPath = value;
				}
		}

		/// <summary>
		///		Indica si se deben mostrar los archivos
		/// </summary>
		public bool ShowFiles
		{	get { return (bool) GetValue(ShowFilesProperty); }
			set 
				{ SetValue(ShowFilesProperty, value); 
					ViewModelData.ShowFiles = value;
				}
		}

		/// <summary>
		///		Controlador del evento <see cref="SelectedPathChangedEvent"/>
		/// </summary>
		public event RoutedEventHandler SelectedPathChanged
		{	add { AddHandler(SelectedPathChangedEvent, value); } 
			remove { RemoveHandler(SelectedPathChangedEvent, value); }
		}

		private void trvExplorer_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{ if (trvExplorer.DataContext is FtpTreeExplorerViewModel &&  (sender as TreeView).SelectedItem is Libraries.MVVM.ViewModels.TreeItems.ITreeViewItemViewModel)
				(trvExplorer.DataContext as FtpTreeExplorerViewModel).SelectedItem = (Libraries.MVVM.ViewModels.TreeItems.ITreeViewItemViewModel) (sender as TreeView).SelectedItem;
		}

		private void trvExplorer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{ ViewModelData.PropertiesCommand.Execute(null);
		}

		private void trvExplorer_MouseDown(object sender, MouseButtonEventArgs e)
		{ if (e.ChangedButton == MouseButton.Left)
				ViewModelData.SelectedItem = null;
		}

		private void trvExplorer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{ pntStartDrag = e.GetPosition(null);
		}

		private void trvExplorer_PreviewMouseMove(object sender, MouseEventArgs e)
		{ if (e.LeftButton == MouseButtonState.Pressed)
				{	Point pntMouse = e.GetPosition(null);
					Vector vctDifference = pntStartDrag - pntMouse;

						if (pntMouse.X < trvExplorer.ActualWidth - 50 &&
								(Math.Abs(vctDifference.X) > SystemParameters.MinimumHorizontalDragDistance || 
								 Math.Abs(vctDifference.Y) > SystemParameters.MinimumVerticalDragDistance))
							objDragDropController.InitDragOperation(trvExplorer, trvExplorer.SelectedItem as FtpFileNodeViewModel);
				}
		}
		
		private void trvExplorer_DragEnter(object sender, DragEventArgs e)
		{ objDragDropController.TreatDragEnter(e);
		}

		private void trvExplorer_Drop(object sender, DragEventArgs e)
		{ FtpFileNodeViewModel objNodeSource = objDragDropController.GetDragDropFileNode(e.Data) as FtpFileNodeViewModel;

				if (objNodeSource != null)
					{ TreeViewItem trvNode = new Libraries.MVVM.Tools.ToolsWpf().FindAncestor<TreeViewItem>((DependencyObject) e.OriginalSource);

							if (trvNode != null)
								{ FtpFileNodeViewModel objNodeTarget = trvNode.Header as FtpFileNodeViewModel;

										if (objNodeSource != null && objNodeTarget != null)
											ViewModelData.Copy(objNodeSource, objNodeTarget, 
																				 (e.KeyStates & DragDropKeyStates.ControlKey) == DragDropKeyStates.ControlKey);
								}
				}
		}
	}
}