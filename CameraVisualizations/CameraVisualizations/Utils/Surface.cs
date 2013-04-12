﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace CameraVisualizations.Utils
{
    public static class Surface
    {
        private static List<Phone> _phones = new List<Phone>();

        private static ObservableCollection<PhoneScatterViewItem> _userControls = new ObservableCollection<PhoneScatterViewItem>();

        static Surface() 
        { 
            Surface.Phones.Add(new Phone() {
                Id = 1,
                Name = "Samsung Galaxy S Plus",
                Pined = false,
                ImgSrc = "iconSamsung.png",
                BrushColor = SurfaceColors.Accent1Brush
            }); 

            Surface.Phones.Add(new Phone() {
                Id = 2,
                Name = "HTC Desire S",
                Pined = false,
                ImgSrc = "iconHTC.png",
                BrushColor = SurfaceColors.Accent2Brush
            });
        }

        public static List<Phone> Phones 
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

        public static ObservableCollection<PhoneScatterViewItem> UserControls
        {
            get
            {
                return _userControls;
            }

            set
            {
                _userControls = value;
            }
        }
    }
}