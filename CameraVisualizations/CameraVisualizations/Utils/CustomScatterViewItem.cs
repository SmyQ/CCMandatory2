using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using CameraVisualizations.Interfaces;

namespace CameraVisualizations.Utils
{
    public class CustomScatterViewItem : ScatterViewItem, ICustomScatterViewItem
    {
        public Phone Phone { get; set; }
    }
}
