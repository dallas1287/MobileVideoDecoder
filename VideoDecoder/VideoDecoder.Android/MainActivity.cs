using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using System.Reflection;
using System.IO;
using Xamarin.Essentials;

namespace VideoDecoder.Droid
{
    [Activity(Label = "VideoDecoder", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            string resourceFile = "";
            var resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            foreach (var n in resourceNames)
            {
                Console.WriteLine(n);
                if(n.EndsWith(".mp4"))
                {
                    resourceFile = n;
                }
            }

            Decoder dec = new Decoder();
            _ = dec.CopyVideoIfNotExists(Assembly.GetExecutingAssembly(), resourceFile);
            //System.Threading.Tasks.Task task = dec.SaveCountAsync(1234567890);
            //string filename = "/storage/emulated/0/Documents/eevee_master.mp4";
            //AndroidNativeDecoder nd = new AndroidNativeDecoder();
            //nd.InitDecoder(filename);

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}