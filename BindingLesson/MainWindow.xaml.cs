using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BindingLesson
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private ObservableCollection<Product> products = new ObservableCollection<Product>();

		public MainWindow()
		{
			InitializeComponent();

			using (var context = new Context())
			{
				products = new ObservableCollection<Product>(context.Products.ToList());
			}

			dataGrid.ItemsSource = products;

			products.CollectionChanged += ProductsCollectionChanged;

			//products.Add(new Product
			//{
			//	Name = "Socks",
			//	Price = 500,
			//	Quantity = 7
			//});

			//products.Remove(products.Last());
		}

		private void ProductsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add: // если добавление
					Product newProduct = e.NewItems[0] as Product;
					using (var context = new Context())
					{
						context.Products.Add(newProduct);
						context.SaveChanges();
					}
					break;
				case NotifyCollectionChangedAction.Remove: // если удаление
					Product oldProduct = e.OldItems[0] as Product;
					using (var context = new Context())
					{
						context.Products.Remove(oldProduct);
						context.SaveChanges();
					}
					break;
				case NotifyCollectionChangedAction.Replace: // если замена
					Product replacedProduct = e.OldItems[0] as Product;
					Product replacingProduct = e.NewItems[0] as Product;
					using (var context = new Context())
					{
						var result = context.Products.SingleOrDefault(p => p.Id == replacedProduct.Id);
						if (result != null)
						{
							result = replacingProduct;
							context.SaveChanges();
						}
					}
					break;
			}
		}

		//private void OnRowDeleted(object sender, KeyEventArgs e)
		//{
		//	var currentRow = (DataGridRow)dataGrid
		//		.ItemContainerGenerator
		//		.ContainerFromIndex(dataGrid.SelectedIndex);

		//	if (e.Key == Key.Delete && !currentRow.IsEditing)
		//	{
		//		products.RemoveAt(dataGrid.SelectedIndex);
		//	}
		//	else if (e.Key == Key.Enter && !currentRow.IsEditing)
		//	{

		//	}
		//}

		private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
		{
			Product changingProduct = (e.Column.GetCellContent(e.Row) as ContentPresenter).Content as Product;
			Product changedProduct = products.SingleOrDefault<Product>(p => p.Id == changingProduct.Id);
			changedProduct = changingProduct;
		}
	}
}
