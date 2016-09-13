using System.Windows;
using BloggersPoint.UI.ViewModel;
using BloggersPoint.Core.Services;

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
            IBloggersPointService bloggersPointService = new BloggersPointService();
            DataContext = PostListViewModel.Instance(bloggersPointService);
        }
    }
}
