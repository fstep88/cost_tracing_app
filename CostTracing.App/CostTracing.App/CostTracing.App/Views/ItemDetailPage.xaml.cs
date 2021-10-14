using CostTracing.App.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace CostTracing.App.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}