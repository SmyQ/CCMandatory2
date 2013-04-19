using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CameraVisualizations.Utils
{
    public class Phone
    {
        public int Id { get; set; }
        public bool Pined { get; set; }
        public string Name { get; set; }
        public Brush BrushColor { get; set; }
        public String ImgSrc { get; set; }
        public String IpAddr { get; set; }
        public int Port { get; set; }
    }
}
