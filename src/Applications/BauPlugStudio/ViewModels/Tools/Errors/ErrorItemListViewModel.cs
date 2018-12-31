using System;
using System.Collections.ObjectModel;

using Bau.Libraries.BauMvvm.Common.Controllers.Messengers.Common;

namespace Bau.Applications.BauPlugStudio.ViewModels.Tools.Errors
{
	/// <summary>
	///		ViewModel para la lista de errores
	/// </summary>
	public class ErrorItemListViewModel : Libraries.BauMvvm.ViewModels.BaseObservableObject
	{ 
		// Variables privadas
		private MessageError _selectedErrorItem;
		private ObservableCollection<MessageError> _errorItems = new ObservableCollection<MessageError>();

		public ErrorItemListViewModel()
		{
		}

		/// <summary>
		///		Añade un elemento de error
		/// </summary>
		internal void AddError(MessageError error)
		{ 
			// Añade el elemento al error
			ErrorItems.Add(error);
			// Si tiene demasiados elementos de log quita el primero
			if (ErrorItems.Count > 1000)
				ErrorItems.RemoveAt(0);
		}

		/// <summary>
		///		Actualiza la lista
		/// </summary>
		public void RefreshList()
		{ 
			// ErrorItems = new LogItemBussiness().LoadLast(50).ToObservableCollection();
		}

		/// <summary>
		///		Error seleccionado
		/// </summary>
		public MessageError SelectedErrorItem
		{
			get { return _selectedErrorItem; }
			set { CheckObject(ref _selectedErrorItem, value); }
		}

		/// <summary>
		///		Registros de error
		/// </summary>
		public ObservableCollection<MessageError> ErrorItems
		{
			get {  return _errorItems; }
			set { CheckObject(ref _errorItems, value); }
		}
	}
}
