using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Sinergija21.Basic.iOS.Models;
using UIKit;

namespace Sinergija21.Basic.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            DI.Display = new iOSDisplayManager();
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, typeof(AppDelegate));
        }
    }
}
