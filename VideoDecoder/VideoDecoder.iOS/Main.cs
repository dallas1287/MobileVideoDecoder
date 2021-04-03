using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Foundation;
using UIKit;

namespace VideoDecoder.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var names = assembly.GetManifestResourceNames();
            foreach(var n in names)
                Console.WriteLine(n);
            var path = NSBundle.MainBundle.PathForResource("robot", ".mp4");
            Console.WriteLine(path);
            var docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Console.WriteLine(docs);

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
