using System;
using System.Collections.Generic;
using System.ComponentModel;
using VideoDecoder.Models;
using VideoDecoder.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VideoDecoder.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}