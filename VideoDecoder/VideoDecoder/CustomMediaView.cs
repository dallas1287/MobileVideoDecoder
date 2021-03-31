using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace VideoDecoder
{
    public class CustomMediaView : View
    {
        public static readonly BindableProperty CustomMediaProperty = BindableProperty.Create(
            propertyName: "CustomMedia",
            returnType: typeof(int),
            declaringType: typeof(CustomMediaView),
            defaultValue: 0);


        public int DummyProp
        {
            get { return (int)GetValue(CustomMediaProperty); }
            set { SetValue(CustomMediaProperty, value); }
        }
    }
}
