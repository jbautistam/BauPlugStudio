using System;
using System.Collections.Generic;

using Bau.Libraries.LibDataBaseStudio.Model.Connections;
using Bau.Libraries.SourceEditor.Model.Solutions;
using Bau.Libraries.SourceEditor.Model.Definitions;
using Bau.Libraries.LibDbProviders.Base.Schema;

namespace Bau.Libraries.LibDataBaseStudio.ViewModel.Controllers
{
	/// <summary>
	///		Manager para la creación de los nodos de esquema
	/// </summary>
	internal class TreeNodesSchemaManager
	{
		/// <summary>
		///		Carga los datos de una conexión
		/// </summary>
		private SchemaConnectionModel LoadConnection(FileModel file)
		{
			return new Application.Bussiness.SchemaConnectionBussiness().Load(file.FullFileName);
		}

		/// <summary>
		///		Obtiene los nodos de tablas
		/// </summary>
		internal OwnerChildModelCollection GetNodesTables(FileModel file)
		{
			OwnerChildModelCollection childs = new OwnerChildModelCollection();
			SchemaConnectionModel connection = LoadConnection(file);

				// Carga las tablas del esquema
				foreach (TableDbModel table in new Application.Services.SchemaReader().GetTables(connection))
				{
					OwnerChildModel childTable = childs.Add($"{table.Schema}.{table.Name}", file, table.Name,
															SearchDefinition(file, SourceEditorPluginManager.TreeType.Table), 
															true);
						// Añade las columnas
						foreach (FieldDbModel column in table.Fields)
							childTable.ObjectChilds.Add(column.Name, file, column.Name,
														SearchDefinition(file, SourceEditorPluginManager.TreeType.Column), 
														false);
				}
				// Devuelve la colección de nodos
				return childs;
		}

		/// <summary>
		///		Obtiene los nodos de tablas
		/// </summary>
		internal OwnerChildModelCollection GetNodesViews(FileModel file)
		{
			OwnerChildModelCollection childs = new OwnerChildModelCollection();
			SchemaConnectionModel connection = LoadConnection(file);

				// Carga las vistas del esquema
				foreach (TableDbModel view in new Application.Services.SchemaReader().GetViews(connection))
				{
					OwnerChildModel childView;

						// Añade la vista
						childView = childs.Add($"{view.Schema}.{view.Name}", file, view.Name,
											   SearchDefinition(file, SourceEditorPluginManager.TreeType.View), true);
						// Añade las columnas
						foreach (FieldDbModel column in view.Fields)
							childView.ObjectChilds.Add(column.Name, file, column.Name,
													   SearchDefinition(file, SourceEditorPluginManager.TreeType.Column), false);
				}
				// Devuelve la colección de nodos
				return childs;
		}

		/// <summary>
		///		Obtiene los nodos de procedimientos almacenados
		/// </summary>
		internal OwnerChildModelCollection GetNodesStoredProcedures(FileModel file)
		{
			OwnerChildModelCollection childs = new OwnerChildModelCollection();
			OwnerChildModel root;
			SchemaConnectionModel connection = LoadConnection(file);
			List<RoutineDbModel> routines = new Application.Services.SchemaReader().GetStoredProcedures(connection);

				// Nodo raíz de procedimientos almacenados
				root = childs.Add(SourceEditorPluginManager.TreeType.StoredProceduresRoot.ToString(),
								  file, "Rutinas",
								  SearchDefinition(file, SourceEditorPluginManager.TreeType.StoredProceduresRoot),
								  true);

				// Carga las rutinas del esquema
				foreach (RoutineDbModel routine in routines)
					root.ObjectChilds.Add(routine.Name, file, routine.Name, SearchDefinition(file, SourceEditorPluginManager.TreeType.StoredProcedure), false);
				// Devuelve la colección
				return childs;
		}

		/// <summary>
		///		Obtiene la definición de un tipo de objeto
		/// </summary>
		private OwnerObjectDefinitionModel SearchDefinition(FileModel file, SourceEditorPluginManager.TreeType treeType)
		{
			return file.FileDefinition.OwnerChilds.SearchRecursive(treeType.ToString());
		}
	}
}
