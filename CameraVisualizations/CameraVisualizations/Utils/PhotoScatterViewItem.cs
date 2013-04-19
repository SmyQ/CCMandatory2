using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using CameraVisualizations.Interfaces;
using CameraVisualizations.UserControls;
using System.Windows.Media.Imaging;
using System.Windows;

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

        public PhotoScatterViewItem(Phone phone, String imgUrl) : this()
        {
            ucPhoto ucp = new ucPhoto();
            try
            {
                ucp.imgPhoto.Source = new BitmapImage(new Uri(imgUrl));
                ucp.gridMain.Background = phone.BrushColor;

                Phone = phone;
                Content = ucp;
                ImgUrl = imgUrl;

                ContainerManipulationStarted += new ContainerManipulationStartedEventHandler(photo_ContainerManipulationStarted);
                ContainerManipulationCompleted += new ContainerManipulationCompletedEventHandler(photo_ContainerManipulationCompleted);
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Img broken: " + imgUrl);
            }
        }

        void photo_ContainerManipulationCompleted(object sender, ContainerManipulationCompletedEventArgs e)
        {
            PhotoScatterViewItem image = sender as PhotoScatterViewItem;
            Console.Out.WriteLine("Finished");

            foreach (PhoneScatterViewItem item in Helper.Surface.UserControls.Where(u => u is PhoneScatterViewItem && image.Phone != u.Phone))
            {

                var itemc = item.TransformToAncestor(Helper.ScatterView).Transform(new Point(0, 0));
                var imgc = image.TransformToAncestor(Helper.ScatterView).Transform(new Point(0, 0));

                var dx = imgc.X - itemc.X;
                var dy = imgc.Y - itemc.Y;
                var distance = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
                if (distance < 300)
                {
                    //var phoneImage = image.Content as PhoneImage;
                    //var phoneThingy = item.Content as PhoneThingy;

                    //phoneThingy.uploadImage(phoneThingy.phoneUrl, phoneImage);

                    Console.Out.WriteLine("TRANSFER IMG");
                    Helper.NetworkDevice.SendImage(item.Phone.IpAddr, image.ImgUrl, item.Phone.Port);
                    //Helper.ScatterView.relaxItems();

                    break;
                }
                
            }
        }

        void photo_ContainerManipulationStarted(object sender, Microsoft.Surface.Presentation.Controls.ContainerManipulationStartedEventArgs e)
        {
            var image = sender as PhotoScatterViewItem;
            //throw new NotImplementedException();
            Console.Out.WriteLine("Started");
        }

        public String ImgUrl { get; set; }
    }
}
