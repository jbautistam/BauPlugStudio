using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibXmppClient.Core.Forms;

namespace Bau.Plugins.BauMessenger.Views.Forms
{
	/// <summary>
	///		Formulario para rellenar los datos de un formulario de XMPP
	/// </summary>
	public partial class XmppDataFormView : Window
	{ 
		// Constantes privadas
		private const string IdGrid = "grdProperties";

		public XmppDataFormView(JabberForm xmppform, string title)
		{   
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa las propiedades
			Title = title;
			XmppForm = xmppform;
		}

		/// <summary>
		///		Inicializa el formulario
		/// </summary>
		private void InitForm()
		{
			string xaml = GetXamlForm(IdGrid, XmppForm);

				// Asigna las instrucciones
				lblInstructions.Text = XmppForm.Instructions;
				// Asigna el contenido al control principal
				pnlViewer.Content = ConvertXaml(xaml);
		}

		/// <summary>
		///		Obtiene el Xaml de un formulario de Xmpp
		/// </summary>
		private string GetXamlForm(string gridName, JabberForm xmppform)
		{
			int rows = 0;
			string xaml = "";

				// Crea el Xaml de los controles
				foreach (KeyValuePair<string, JabberFormItem> formItem in xmppform.Items)
					if (MustShow(formItem.Value))
					{ 
						// Crea el control
						xaml += GetXamlControl(formItem.Key, formItem.Value, rows);
						if (formItem.Key.EqualsIgnoreCase("url"))
							xaml += GetXamlImage(formItem.Value, ++rows);
						// Incrementa el número de línea
						rows++;
					}
				// Compila los controles en el grid
				return GetXamlGrid(gridName, rows, xaml);
		}

		/// <summary>
		///		Indica si se debe mostrar una línea de control
		/// </summary>
		private bool MustShow(JabberFormItem formItem)
		{
			return formItem.Type != JabberFormItem.FormItemType.Hidden;
		}

		/// <summary>
		///		Obtiene el XAML de un control
		/// </summary>
		private string GetXamlControl(string controlID, JabberFormItem formItem, int row)
		{
			bool hasInputControl = CheckHasInputControl(formItem);
			string xaml;

				if (formItem.Type == JabberFormItem.FormItemType.Fixed)
					xaml = GetXamlLabel(formItem.FirstValue, false, row, 2);
				else
					xaml = GetXamlLabel(formItem.Title, formItem.IsRequired, row, hasInputControl ? 1 : 2);
				// Si tiene un control de entrada de datos, obtiene el valor
				if (hasInputControl)
					xaml += GetXamlControlFormItem(controlID, formItem, row);
				// Devuelve el Xaml
				return xaml;
		}

		/// <summary>
		///		Comprueba si tiene un control de entrada de datos
		/// </summary>
		private bool CheckHasInputControl(JabberFormItem formItem)
		{
			return formItem.Type != JabberFormItem.FormItemType.Hidden && formItem.Type != JabberFormItem.FormItemType.Fixed;
		}

		/// <summary>
		///		Obtiene el Xaml de una etiqueta
		/// </summary>
		private string GetXamlLabel(string text, bool isRequired, int row, int colSpan)
		{
			string required = "";

				// Asigna el asterico a la etiqueta cuando es obligatorio
				if (isRequired)
					required = "*";
				// Devuelve el Xaml de la etiqueta
				return $"<Label Grid.Row='{row}' Grid.Column='0' Grid.ColumnSpan='{colSpan}' Content='{text}{required}:' />";
		}

		/// <summary>
		///		Obtiene el Xaml de una imagen
		/// </summary>
		private string GetXamlImage(JabberFormItem formItem, int row)
		{
			return $"<Image Grid.Row='{row}' Grid.Column='1' Source='{formItem.FirstValue}' />";
		}

		/// <summary>
		///		Obtiene el control asociado a un elemento de formulario
		/// </summary>
		private string GetXamlControlFormItem(string controlID, JabberFormItem formItem, int row)
		{
			switch (formItem.Type)
			{
				case JabberFormItem.FormItemType.Boolean:
					return GetXamlCheckBox(controlID, row, formItem.FirstValue.GetBool());
				case JabberFormItem.FormItemType.TextMultiple:
				case JabberFormItem.FormItemType.TextPrivate:
				case JabberFormItem.FormItemType.TextSingle:
					return GetXamlTextBox(controlID, row, formItem.FirstValue);
				default:
					return GetXamlTextBox(controlID, row, formItem.FirstValue + " Esto no está controlado");
			}
		}

		/// <summary>
		///		Obtiene el Xaml de un checkbox
		/// </summary>
		private string GetXamlCheckBox(string controlID, int row, bool isChecked)
		{
			return $"<CheckBox x:Name='{controlID}' Tag='{controlID}' Grid.Row='{row}' Grid.Column='1' IsChecked='{isChecked}' />";
		}

		/// <summary>
		///		Obtiene el Xaml de un cuadro de texto
		/// </summary>
		private string GetXamlTextBox(string controlID, int row, string value)
		{
			return $"<TextBox x:Name='{controlID}' Tag='{controlID}' Grid.Row='{row}' Grid.Column='1' Text='{value}' />";
		}

		/// <summary>
		///		Obtiene el Xaml de un grid
		/// </summary>
		private string GetXamlGrid(string gridName, int rows, string xamlContent)
		{
			string xaml = $"<Grid xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' x:Name='{gridName}' Tag='{gridName}'>";

				// Asigna las definiciones de filas
				xaml += "<Grid.RowDefinitions>";
				for (int index = 0; index < rows; index++)
					xaml += "<RowDefinition Height='Auto' />";
				xaml += "</Grid.RowDefinitions>";
				// Asigna las definiciones de columnas
				xaml += "<Grid.ColumnDefinitions>";
				xaml += "<ColumnDefinition Width='Auto'/>";
				xaml += "<ColumnDefinition Width='*'/>";
				xaml += "</Grid.ColumnDefinitions>";
				// Asigna el contenido
				xaml += xamlContent;
				// Cierra el grid
				xaml += "</Grid>";
				// Devuelve el Xaml
				return xaml;
		}

		/// <summary>
		///		Asigna el XAML al control de propiedades
		/// </summary>
		private FrameworkElement ConvertXaml(string xaml)
		{
			byte[] xamlBytes = new System.Text.UTF8Encoding().GetBytes(xaml);

				// Crea un elemento cargando el XAML
				return XamlReader.Load(new System.IO.MemoryStream(xamlBytes)) as FrameworkElement;
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		private void Save()
		{
			string error = "";

				// Rellena el formulario con los valores
				FillForm(XmppForm, IdGrid);
				// Comprueba si todos los datos obligatorios tienen al menos un valor
				foreach (KeyValuePair<string, JabberFormItem> keyFormItem in XmppForm.Items)
					if (keyFormItem.Value.IsRequired &&
							(keyFormItem.Value.Results == null || keyFormItem.Value.Results.Count == 0 ||
							 keyFormItem.Value.FirstResult.IsEmpty()))
						error += $"No se ha introducido ningún valor en el control etiquetado como '{keyFormItem.Value.Title}' {Environment.NewLine}";
				// Si hay algún error lo muestra, si no, cierra el formulario
				if (!error.IsEmpty())
					BauMessengerPlugin.MainInstance.HostPluginsController.ControllerWindow.ShowMessage(error);
				else
				{
					DialogResult = true;
					Close();
				}
		}

		/// <summary>
		///		Rellena el formulario con los valores de los controles
		/// </summary>
		private void FillForm(JabberForm xmppform, string idControl)
		{
			List<FrameworkElement> controls = new List<FrameworkElement>();

			// Obtiene la colección de controles hijo del panel
			GetChildControls(pnlViewer, controls);
			// Rellena los elementos del formulario
			foreach (KeyValuePair<string, JabberFormItem> keyFormItem in xmppform.Items)
				if (MustShow(keyFormItem.Value) && keyFormItem.Value.Type != JabberFormItem.FormItemType.Fixed)
				{
					FrameworkElement control;

						// Limpia los valores
						keyFormItem.Value.Results.Clear();
						// Obtiene el control adecuado
						control = SearchControl(controls, keyFormItem.Key);
						// Obtiene el valor del control
						if (control != null)
						{
							if (control is TextBox)
								keyFormItem.Value.Results.Add((control as TextBox).Text);
							else if (control is CheckBox)
								keyFormItem.Value.Results.Add((control as CheckBox).IsChecked.ToString());
						}
				}
		}

		/// <summary>
		///		Obtiene el control
		/// </summary>
		private FrameworkElement SearchControl(List<FrameworkElement> controls, string key)
		{ // Recorre la lista de controles
			foreach (FrameworkElement control in controls)
				if (control.Tag != null && (control.Tag as string).EqualsIgnoreCase(key))
					return control;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return null;
		}

		/// <summary>
		///		Obtiene una colección de controles hijo
		/// </summary>
		private void GetChildControls(Visual parent, List<FrameworkElement> controls)
		{
			int children = VisualTreeHelper.GetChildrenCount(parent);

				// Recorre los elementos hijo del control
				for (int index = 0; index < children; index++)
				{
					Visual childVisual = (Visual) VisualTreeHelper.GetChild(parent, index);

						// Si es un control con un valor en el tag lo añade a la colección
						if (childVisual is FrameworkElement && (childVisual as FrameworkElement)?.Tag != null)
							controls.Add(childVisual as FrameworkElement);
						// Añade los controles hijo
						GetChildControls(childVisual, controls);
				}
		}

		/// <summary>
		///		Formulario de XMPP que se tiene que mostrar / rellenar
		/// </summary>
		private JabberForm XmppForm { get; }

		private void cmdSave_Click(object sender, RoutedEventArgs e)
		{
			Save();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			InitForm();
		}
	}
}
