using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CameraVisualizations.Interfaces;
using CameraVisualizations.Utils;
using CameraVisualizations.UserControls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Microsoft.Surface.Presentation.Controls;

namespace CameraVisualizations.Providers
{
    public class MockPhotoProvider : IPhotoProvider
    {
        public List<PhotoScatterViewItem> GetPhotos(Phone phone)
        {
            List<PhotoScatterViewItem> items = new List<PhotoScatterViewItem>();

            ucPhoto ucp = new ucPhoto();
            ucp.imgPhoto.Source = new BitmapImage(new Uri(@"/CameraVisualizations;component/Resources/" + "pedo.jpg", UriKind.Relative));
            ucp.gridMain.Background = phone.BrushColor;

            PhotoScatterViewItem photo = new PhotoScatterViewItem()
            {
                Phone = phone,
                Content = ucp,
                
            };

            photo.ContainerManipulationStarted += new ContainerManipulationStartedEventHandler(photo_ContainerManipulationStarted);
            photo.ContainerManipulationCompleted += new ContainerManipulationCompletedEventHandler(photo_ContainerManipulationCompleted);

            items.Add(photo);

            ucp = new ucPhoto();
            ucp.imgPhoto.Source = new BitmapImage(new Uri(@"/CameraVisualizations;component/Resources/" + "cycle.png", UriKind.Relative));
            ucp.gridMain.Background = phone.BrushColor;

            photo = new PhotoScatterViewItem()
            {
                Phone = phone,
                Content = ucp,
                
            };

            photo.ContainerManipulationStarted += new ContainerManipulationStartedEventHandler(photo_ContainerManipulationStarted);
            photo.ContainerManipulationCompleted += new ContainerManipulationCompletedEventHandler(photo_ContainerManipulationCompleted);

            items.Add(photo);

            return items;
        }

        void photo_ContainerManipulationCompleted(object sender, ContainerManipulationCompletedEventArgs e)
        {
            var image = sender as PhotoScatterViewItem;
            Console.Out.WriteLine("Finished");

            //foreach (ScatterViewItem item in Helper.Surface.)
            //{
            //    if (item.Content is PhoneThingy)
            //    {
            //        var itemc = item.TransformToAncestor(scatterThingy.scatter).Transform(new Point(0, 0));
            //        var imgc = image.TransformToAncestor(scatterThingy.scatter).Transform(new Point(0, 0));

            //        var dx = imgc.X - itemc.X;
            //        var dy = imgc.Y - itemc.Y;
            //        var distance = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
            //        if (distance < 300)
            //        {
            //            var phoneImage = image.Content as PhoneImage;
            //            var phoneThingy = item.Content as PhoneThingy;

            //            phoneThingy.uploadImage(phoneThingy.phoneUrl, phoneImage);

            //            scatterThingy.relaxItems();

            //            break;
            //        }
            //    }
            //}
        }

        void photo_ContainerManipulationStarted(object sender, Microsoft.Surface.Presentation.Controls.ContainerManipulationStartedEventArgs e)
        {
            var image = sender as PhotoScatterViewItem;
            //throw new NotImplementedException();
            Console.Out.WriteLine("Started");
        }
    }
}
