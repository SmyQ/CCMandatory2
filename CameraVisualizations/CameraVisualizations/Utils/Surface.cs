using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.ComponentModel;

namespace CameraVisualizations.Utils
{
    public class Surface : INotifyPropertyChanged
    {
        private  List<Phone> _phones = new List<Phone>();

        private  ObservableCollection<CustomScatterViewItem> _userControls = new ObservableCollection<CustomScatterViewItem>();

        public Surface() 
        { 
            Phones.Add(new Phone() {
                Id = 1,
                Name = "Samsung Galaxy S Plus",
                Pined = false,
                ImgSrc = "iconSamsung.png",
                BrushColor = SurfaceColors.Accent1Brush,
                IpAddr = "10.6.6.165",
                Port = 7000
            }); 

            Phones.Add(new Phone() {
                Id = 2,
                Name = "HTC Desire S",
                Pined = false,
                ImgSrc = "iconHTC.png",
                BrushColor = SurfaceColors.Accent2Brush,
                IpAddr = "10.6.6.124",
                Port = 8000
            });
        }

        public List<Phone> Phones 
        { 
            get 
            {
                return _phones;
            }

            set
            {
                _phones = value;
            }
        }

        public ObservableCollection<CustomScatterViewItem> UserControls
        {
            get
            {
                return _userControls;
            }

            set
            {
                _userControls = value;
                NotifyPropertyChanged("UserControls");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
