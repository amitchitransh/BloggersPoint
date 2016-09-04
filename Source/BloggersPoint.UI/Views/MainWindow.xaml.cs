using System.Windows;
using BloggersPoint.UI.ViewModel;

namespace BloggersPoint.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = PostListViewModel.Instance();
        }
    }
}
