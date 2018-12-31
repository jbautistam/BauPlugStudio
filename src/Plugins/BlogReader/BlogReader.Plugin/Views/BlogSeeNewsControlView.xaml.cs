using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibBlogReader.Model;
using Bau.Libraries.LibBlogReader.ViewModel.Blogs;
using Bau.Libraries.LibBlogReader.ViewModel.Blogs.TreeBlogs;
using Bau.Libraries.BauMvvm.Views.Forms;

namespace Bau.Plugins.BlogReader.Views
{
	/// <summary>
	///		Ventana para ver las noticias del blog
	/// </summary>
	public partial class BlogSeeNewsControlView : UserControl, IFormView
	{ 
		// Variables privadas
		private readonly DispatcherTimer _tmrRead;
		private DateTime _lastRead = DateTime.Now;

		public BlogSeeNewsControlView(BaseNodeViewModel nodeViewModel)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Crea el temporizador de lectura
			_tmrRead = new DispatcherTimer();
			_tmrRead.Interval = new TimeSpan(0, 0, 30);
			_tmrRead.Tick += tmrRead_Tick;
			// Inicializa el ViewModel
			ViewModel = CreateViewModel(nodeViewModel);
			grdData.DataContext = ViewModel;
			lswEntries.ItemsSource = ViewModel.Entries;
			FormView = new BaseFormView(ViewModel);
			// Observa el evento PropertyChanged para ver cuándo se cambia el Html
			ViewModel.PropertyChanged += (sender, evntArgs) =>
												{
													if (evntArgs.PropertyName.EqualsIgnoreCase("HtmlNews"))
														ShowHtmlNews();
												};
			// Añade el viewModel al explorador
			wbExplorer.DataContext = ViewModel;
			wbExplorer.FunctionExecute += (sender, evntArgs) => ViewModel.ExecuteFromExplorer(evntArgs.Parameters);
			// Agrupa los elementos
			if (lswEntries.ItemsSource != null)
			{
				CollectionView views = (CollectionView) CollectionViewSource.GetDefaultView(lswEntries.ItemsSource);
				PropertyGroupDescription groupDescription = new PropertyGroupDescription("BlogName");

					views.GroupDescriptions.Add(groupDescription);
			}
			// Muestra el HTML
			ShowHtmlNews();
		}

		/// <summary>
		///		Crea el ViewModel
		/// </summary>
		private BlogSeeNewsViewModel CreateViewModel(BaseNodeViewModel nodeViewModel)
		{
			BlogsModelCollection blogs = new BlogsModelCollection();

				// Obtiene la colección de blogs
				if (nodeViewModel != null)
					blogs = nodeViewModel.GetBlogs();
				// Devuelve el ViewModel
				return new BlogSeeNewsViewModel(blogs);
		}

		/// <summary>
		///		Muestra el HTML de las noticias
		/// </summary>
		private void ShowHtmlNews()
		{
			// Inicializa el temporizador de lectura
			_tmrRead.Start();
			// Muestra el HTML en el navegador
			wbExplorer.HtmlContent = ViewModel.HtmlNews;
		}

		/// <summary>
		///		Marca los elementos seleccionados como leídos
		/// </summary>
		private void MarkRead()
		{
			ViewModel.MarkRead();
		}

		/// <summary>
		///		ViewModel
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		Wrapper para el layout de documentos
		/// </summary>
		public BlogSeeNewsViewModel ViewModel { get; }

		/// <summary>
		///		Tratamiento del evento del temporizador
		/// </summary>
		private void tmrRead_Tick(object sender, EventArgs e)
		{
			if ((DateTime.Now - _lastRead).TotalSeconds > 30 && IsVisible)
			{ 
				// Detiene el temporizador y guarda la hora
				_lastRead = DateTime.Now;
				_tmrRead.Stop();
				// Marca los elementos como leídos
				MarkRead();
			}
		}

		private void lswEntries_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ViewModel.MarkAsDirty();
		}
	}
}