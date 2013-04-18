using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using CameraVisualizations.Interfaces;

namespace CameraVisualizations.Utils
{
    public class PhotoScatterViewItem : CustomScatterViewItem
    {
        public PhotoScatterViewItem()
        {
            CanScale = false;
            Orientation = 0;
            Width = 250;
            Height = 250;
        }
    }
}
