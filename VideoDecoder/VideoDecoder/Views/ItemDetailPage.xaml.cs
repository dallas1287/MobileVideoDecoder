using System.ComponentModel;
using VideoDecoder.ViewModels;
using Xamarin.Forms;

namespace VideoDecoder.Views
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