using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CameraVisualizations.Interfaces;
using CameraVisualizations.Utils;
using CameraVisualizations.UserControls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace CameraVisualizations.Providers
{
    public class AndroidPhotoProvider : IPhotoProvider
    {
        public List<PhotoScatterViewItem> GetPhotos(Phone phone)
        {
            List<PhotoScatterViewItem> items = new List<PhotoScatterViewItem>();

            ucPhoto ucp = new ucPhoto();
            ucp.imgPhoto.Source = new BitmapImage(new Uri(@"/CameraVisualizations;component/Resources/" + "pedo.jpg", UriKind.Relative));
            ucp.gridMain.Background = phone.BrushColor;

            items.Add(new PhotoScatterViewItem()
            {
                Phone = phone,
                Content = ucp
            });

            return items;
        }
    }
}
