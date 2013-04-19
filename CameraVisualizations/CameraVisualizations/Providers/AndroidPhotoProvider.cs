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
using System.IO;

namespace CameraVisualizations.Providers
{
    public class AndroidPhotoProvider : IPhotoProvider
    {
        public List<PhotoScatterViewItem> GetPhotos(Phone phone)
        {
            List<PhotoScatterViewItem> items = new List<PhotoScatterViewItem>();
            
            foreach(String uri in GetPhotoNames(phone))
            {
                items.Add(new PhotoScatterViewItem(phone, uri));
            }

            return items;
        }

        private IEnumerable<String> GetPhotoNames(Phone phone)
        {
            List<String> filesNames = new List<String>();
            String uri = Environment.CurrentDirectory + "\\" + phone.IpAddr.Replace(".", "_") + "\\";

            DirectoryInfo di = new DirectoryInfo(uri);
            foreach (FileInfo file in di.GetFiles())
            {
                filesNames.Add(file.FullName);
            }

            return  filesNames;
        }
    }
}
