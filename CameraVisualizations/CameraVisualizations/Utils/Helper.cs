using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CameraVisualizations.Server;
using Microsoft.Surface.Presentation.Controls;

namespace CameraVisualizations.Utils
{
    public static class Helper
    {
        private static Surface _surface;

        public static Surface Surface 
        {
            get
            {
                return _surface ?? (_surface = new Surface());
            }
        }

        private static NetworkDevice _networkDevice;

        public static NetworkDevice NetworkDevice
        {
            get
            {
                return _networkDevice ?? (_networkDevice = new NetworkDevice());
            }
        }

        public static ScatterView ScatterView { get; set; }
    }
}
