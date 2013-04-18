using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
