using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMediaPlayer.Model;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;

namespace Bau.Libraries.LibMediaPlayer.Repository
{
	/// <summary>
	///		Repository para la estructura de carpetas / archivos
	/// </summary>
	internal class MediaRepository
	{   
		// Constantes privadas
		private const string TagRoot = "MediaRoot";
		private const string TagFolder = "Folder";
		private const string TagAlbum = "Album";
		private const string TagId = "Id";
		private const string TagName = "Name";
		private const string TagDescription = "Description";
		private const string TagPath = "Path";
		private const string TagFile = "File";
		private const string TagUrl = "Url";
		private const string TagStatus = "Status";
		private const string TagFileName = "FileName";
		private const string TagCreatedAt = "CreatedAt";

		/// <summary>
		///		Carga una estructura de carpetas
		/// </summary>
		internal MediaFolderModel Load(string fileName)
		{
			var folder = new MediaFolderModel();
			MLFile fileML = new XMLParser().Load(fileName);

				// Si existe el archivo, lo carga
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name == TagRoot)
							foreach (MLNode childML in nodeML.Nodes)
								if (childML.Name == TagFolder)
									folder.Folders.Add(LoadFolder(folder, childML));
								else if (childML.Name == TagAlbum)
									folder.Albums.Add(LoadAlbums(folder, childML));
				// Devuelve la carpeta cargada
				return folder;
		}

		/// <summary>
		///		Carga los datos de una carpeta de un nodo
		/// </summary>
		private MediaFolderModel LoadFolder(MediaFolderModel parent, MLNode nodeML)
		{
			MediaFolderModel folder = new MediaFolderModel();

				// Carga los datos de la carpeta
				folder.Name = nodeML.Nodes[TagName].Value;
				folder.Parent = parent;
				// Carga los hijos de la carpetas
				foreach (MLNode childML in nodeML.Nodes)
					if (childML.Name == TagFolder)
						folder.Folders.Add(LoadFolder(folder, childML));
					else if (childML.Name == TagAlbum)
						folder.Albums.Add(LoadAlbums(folder, childML));
				// Devuelve la carpeta
				return folder;
		}

		/// <summary>
		///		Carga los datos de un blog
		/// </summary>
		private MediaAlbumModel LoadAlbums(MediaFolderModel parent, MLNode rootML)
		{
			var album = new MediaAlbumModel();

				// Carga los datos
				album.Folder = parent;
				album.GlobalId = rootML.Attributes[TagId].Value;
				album.Name = rootML.Nodes[TagName].Value;
				album.Description = rootML.Nodes[TagDescription].Value;
				album.Path = rootML.Nodes[TagPath].Value;
				// Asigna los archivos
				album.Files.AddRange(LoadFiles(album, rootML));
				// Devuelve los datos del blog
				return album;
		}

		/// <summary>
		///		Carga los archivos de un álbum
		/// </summary>
		private MediaFileModelCollection LoadFiles(MediaAlbumModel album, MLNode rootML)
		{
			var files = new	MediaFileModelCollection();

				// Carga los archivos
				foreach (MLNode nodeML in rootML.Nodes)
					if (nodeML.Name == TagFile)
					{
						var file = new MediaFileModel();

							// Carga los datos
							file.Album = album;
							file.Name = nodeML.Nodes[TagName].Value;
							file.Description = nodeML.Nodes[TagDescription].Value;
							file.CreatedAt = nodeML.Attributes[TagCreatedAt].Value.GetDateTime(DateTime.Now);
							file.Status = nodeML.Attributes[TagStatus].Value.GetEnum(MediaFileModel.StatusFile.NotPlayed);
							file.FileName = nodeML.Nodes[TagFileName].Value;
							file.Url = nodeML.Nodes[TagUrl].Value;
							// Añade el archivo
							if (!string.IsNullOrEmpty(file.Url) || !string.IsNullOrEmpty(file.FileName))
								files.Add(file);
					}
				// Devuelve los archivos
				return files;
		}

		/// <summary>
		///		Graba una estructura de carpetas
		/// </summary>
		internal void Save(MediaFolderModel folder, string fileName)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add(TagRoot);

				// Añade los nodos de las carpetas
				foreach (MediaFolderModel child in folder.Folders)
					nodeML.Nodes.Add(GetNode(child));
				// Añade los nodos de los albums
				foreach (MediaAlbumModel album in folder.Albums)
					nodeML.Nodes.Add(GetNode(album));
				// Graba el archivo
				new XMLWriter().Save(fileName, fileML);
		}

		/// <summary>
		///		Obtiene el nodo XML de una carpeta
		/// </summary>
		private MLNode GetNode(MediaFolderModel folder)
		{
			MLNode nodeML = new MLNode(TagFolder);

				// Añade los datos de la carpeta
				nodeML.Nodes.Add(TagName, folder.Name);
				// Añade los nodos de las carpetas
				foreach (MediaFolderModel child in folder.Folders)
					nodeML.Nodes.Add(GetNode(child));
				// Añade los nodos de los albums
				foreach (MediaAlbumModel album in folder.Albums)
					nodeML.Nodes.Add(GetNode(album));
				// Devuelve el nodo
				return nodeML;
		}

		/// <summary>
		///		Obtiene el nodo XML de un álbum
		/// </summary>
		private MLNode GetNode(MediaAlbumModel album)
		{
			MLNode nodeML = new MLNode(TagAlbum);

				// Añade los datos del nodo
				nodeML.Attributes.Add(TagId, album.GlobalId);
				nodeML.Nodes.Add(TagName, album.Name);
				nodeML.Nodes.Add(TagDescription, album.Description);
				nodeML.Nodes.Add(TagPath, album.Path);
				// Añade los nodos de los archivos
				nodeML.Nodes.AddRange(GetNodes(album.Files));
				// Devuelve el nodo
				return nodeML;
		}

		/// <summary>
		///		Obtiene los nodos de los archivos
		/// </summary>
		private MLNodesCollection GetNodes(MediaFileModelCollection files)
		{
			var nodesML = new MLNodesCollection();

				// Añade los nodos de los archivos
				foreach (MediaFileModel file in files)
				{
					var nodeML = nodesML.Add(TagFile);

						// Carga los datos
						nodeML.Nodes.Add(TagName, file.Name);
						nodeML.Nodes.Add(TagDescription, file.Description);
						nodeML.Attributes.Add(TagCreatedAt, file.CreatedAt);
						nodeML.Attributes.Add(TagStatus, file.Status.ToString());
						nodeML.Nodes.Add(TagFileName, file.FileName);
						nodeML.Nodes.Add(TagUrl, file.Url);
				}
				// Devuelve la colección de nodos
				return nodesML;
		}
	}
}
