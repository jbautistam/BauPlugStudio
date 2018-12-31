using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.DatabaseStudio.Models.Deployment;
using Bau.Libraries.DatabaseStudio.Models;

namespace Bau.Libraries.DatabaseStudio.ViewModels.Projects.Deployments.Scripts
{
	/// <summary>
	///		ViewModel para el árbol que muestra los scripts asociados a una distribución
	/// </summary>
	public class TreeScriptsViewModel : BaseObservableObject
	{
		// Variables privadas
		private ObservableCollection<TreeScriptNodeViewModel> _children;
		private TreeScriptNodeViewModel _selectedNode;

		public TreeScriptsViewModel(string projectPath, DeploymentModel deployment)
		{
			ProjectPath = projectPath;
			Deployment = deployment;
			Children = new ObservableCollection<TreeScriptNodeViewModel>();
		}

		/// <summary>
		///		Carga los archivos
		/// </summary>
		public void LoadFiles()
		{
			LoadFiles(null, ProjectPath);
		}

		/// <summary>
		///		Carga los archivos de un proyecto
		/// </summary>
		private void LoadFiles(TreeScriptNodeViewModel root, string path)
		{
			if (!string.IsNullOrEmpty(path) && System.IO.Directory.Exists(path))
			{
				// Carga las carpetas
				foreach (string pathChild in System.IO.Directory.GetDirectories(path))
					LoadFiles(AddNode(root, pathChild, true, 
									  MainViewModel.Instance.ViewsController.GetIcon(ProjectExplorerViewModel.NodeType.Folder)),
								pathChild);
				// Carga los archivos
				foreach (string file in System.IO.Directory.GetFiles(path))
					if (!System.IO.Path.GetFileName(file).EqualsIgnoreCase(Application.Bussiness.ProjectBussiness.ProjectFileName) &&
						!System.IO.Path.GetExtension(file).EqualsIgnoreCase(ProjectModel.QueryExtension))
						AddNode(root, file, false, MainViewModel.Instance.ViewsController.GetIcon(GetFileType(file)),
								CheckAtDeployment(file));
			}
		}

		/// <summary>
		///		Comprueba si el script está en el modelo
		/// </summary>
		private bool CheckAtDeployment(string fileName)
		{
			// Obtiene el nombre de archivo relativo
			fileName = GetRelativeFileName(fileName);
			// Comprueba los scripts
			foreach (ScriptModel script in Deployment.Scripts)
				if (script.RelativeFileName.EqualsIgnoreCase(fileName))
					return true;
			// Si ha llegado hasta aquí es porque no estaba en el modelo
			return false;
		}

		/// <summary>
		///		Obtiene el tipo de archivos
		/// </summary>
		private ProjectExplorerViewModel.NodeType GetFileType(string file)
		{
			ProjectExplorerViewModel.NodeType type = ProjectExplorerViewModel.NodeType.Image;

				// Obtiene el tipo de nodo
				if (!string.IsNullOrEmpty(file))
				{
					string extension = System.IO.Path.GetExtension(file);

						if (extension.EqualsIgnoreCase(ProjectModel.ReportExtension))
							type = ProjectExplorerViewModel.NodeType.Report;
						else if (extension.EqualsIgnoreCase(ProjectModel.ImportScriptExtension))
							type = ProjectExplorerViewModel.NodeType.ImportScript;
						else if (extension.EqualsIgnoreCase(ProjectModel.QueryExtension))
							type = ProjectExplorerViewModel.NodeType.Query;
						else if (extension.EqualsIgnoreCase(ProjectModel.StyleExtension))
							type = ProjectExplorerViewModel.NodeType.Style;
				}
				// Devuelve el tipo de nodo
				return type;
		}

		/// <summary>
		///		Añade un nodo
		/// </summary>
		protected TreeScriptNodeViewModel AddNode(TreeScriptNodeViewModel parent, string fileName, bool isBold, string icon, bool isChecked = false)
		{
			TreeScriptNodeViewModel node = new TreeScriptNodeViewModel(parent, fileName);

				// Cambia la negrita
				node.IsBold = isBold;
				node.Icon = icon;
				node.IsExpanded = true;
				node.IsChecked = isChecked;
				// Añade el nodo a la lista
				if (parent == null)
					Children.Add(node);
				else
					parent.Children.Add(node);
				// Devuelve el nodo
				return node;
		}

		/// <summary>
		///		Comprueba los datos del árbol
		/// </summary>
		internal bool ValidateData(out string error)
		{
			// Inicializa los argumentos de salida
			error = string.Empty;
			// Comprueba si hay algún nodo seleccionado
			if (GetScripts().Count == 0)
				error = "Seleccione al menos un archivo para distribuir";
			// Devuelve el valor que indica si los datos son correctos
			return string.IsNullOrWhiteSpace(error);
		}

		/// <summary>
		///		Obtiene los scripts
		/// </summary>
		internal List<ScriptModel> GetScripts()
		{
			return GetScripts(Children);
		}

		/// <summary>
		///		Obtiene los scripts de los nodos
		/// </summary>
		internal List<ScriptModel> GetScripts(ObservableCollection<TreeScriptNodeViewModel> nodes)
		{
			List<ScriptModel> scripts = new List<ScriptModel>();

				// Obtiene los scripts seleccionados
				foreach (TreeScriptNodeViewModel scriptNode in nodes)
				{
					// Obtiene los datos del nodo
					if (!string.IsNullOrEmpty(scriptNode.FileName) && System.IO.File.Exists(scriptNode.FileName) && scriptNode.IsChecked)
						scripts.Add(GetScriptFromNode(scriptNode));
					// Obtiene los scripts de los nodos hijo
					scripts.AddRange(GetScripts(scriptNode.GetChildrensChecked()));
				}
				// Devuelve la colección de scripts
				return scripts;
		}

		/// <summary>
		///		Obtiene el objeto de script de un nodo
		/// </summary>
		private ScriptModel GetScriptFromNode(TreeScriptNodeViewModel scriptNode)
		{
			return new ScriptModel
							{
								RelativeFileName = GetRelativeFileName(scriptNode.FileName)
							};
		}

		/// <summary>
		///		Obtiene el nombre de archivo relativo a la carpeta de proyecto
		/// </summary>
		private string GetRelativeFileName(string fileName)
		{
			return fileName.Right(fileName.Length - ProjectPath.Length - 1);
		}

		/// <summary>
		///		Directorio de proyecto
		/// </summary>
		private string ProjectPath { get; }

		/// <summary>
		///		Modelo de distribución
		/// </summary>
		private DeploymentModel Deployment { get; }

		/// <summary>
		///		Nodos
		/// </summary>
		public ObservableCollection<TreeScriptNodeViewModel> Children 
		{ 
			get { return _children; }
			set { CheckObject(ref _children, value); }
		}

		/// <summary>
		///		Nodo seleccionado
		/// </summary>
		public TreeScriptNodeViewModel SelectedNode
		{	
			get { return _selectedNode; }
			set { CheckObject(ref _selectedNode, value); }
		}
	}
}
