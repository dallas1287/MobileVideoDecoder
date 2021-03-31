using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoDecoder.Models;
using VideoDecoder.ViewModels;
using VideoDecoder.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static System.Net.Mime.MediaTypeNames;

namespace VideoDecoder.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            _ = PickAndShow(PickOptions.Default);
        }

        private async Task<FileResult> PickAndShow(PickOptions options)
        {
            try
            {
                var result = await FilePicker.PickAsync();
                if (result != null)
                {
                    var text = $"File Name: {result.FileName}";
                    if(result.ToString().Length > 0)
                    {
                        var stream = await result.OpenReadAsync();
                    }
                    //if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                    //    result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    var stream = await result.OpenReadAsync();
                    //    Image = ImageSource.FromStream(() => stream);
                    //}
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
                // The user canceled or something went wrong
            }
        }
    }
}