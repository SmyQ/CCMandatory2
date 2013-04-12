using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CameraVisualizations.Utils;
using CameraVisualizations.Interfaces;
using CameraVisualizations.Providers;

namespace CameraVisualizations.UserControls
{
    /// <summary>
    /// Interaction logic for ucPhone.xaml
    /// </summary>
    public partial class ucPhone : UserControl
    {
        public Phone Phone { get; set; }

        public ucPhone()
        {
            InitializeComponent();
        }

        private void pinClick(object sender, RoutedEventArgs e)
        {
            btnPin.Visibility = Visibility.Hidden;
            btnUnpin.Visibility = Visibility.Visible;

            Phone.Pined = true;

            IPhotoProvider photoProvider = new AndroidPhotoProvider();
            foreach(PhotoScatterViewItem item in photoProvider.GetPhotos(Phone))
            {
                Surface.UserControls.Add(item);
            }
        }

        private void unpinClick(object sender, RoutedEventArgs e)
        {
            btnPin.Visibility = Visibility.Visible;
            btnUnpin.Visibility = Visibility.Hidden;

            Phone.Pined = false;
            //var ucToDel = Surface.UserControls.Where(u => u.Phone == Phone).FirstOrDefault();
            //Surface.UserControls.Remove(ucToDel);

            IEnumerable<CustomScatterViewItem> items = Surface.UserControls.Where(x => x.Phone == Phone && x is PhotoScatterViewItem);

            //foreach (CustomScatterViewItem item in items)
            //{
            //    Surface.UserControls.Remove(item);
            //}
        }
    }
}
