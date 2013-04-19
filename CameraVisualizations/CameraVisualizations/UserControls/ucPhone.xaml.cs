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
using System.ComponentModel;
using System.Threading;

namespace CameraVisualizations.UserControls
{
    public delegate void CalledForImages(object sender);

    public partial class ucPhone : UserControl
    {
        public Phone Phone { get; set; }

        public static event CalledForImages CalledForImagesEvent;

        public ucPhone()
        {
            InitializeComponent();
        }

        private void pinClick(object sender, RoutedEventArgs e)
        {
            btnPin.Visibility = Visibility.Hidden;
            btnUnpin.Visibility = Visibility.Visible;

            Phone.Pined = true;

            

            if (CalledForImagesEvent != null)
                CalledForImagesEvent(this);


            //BackgroundWorker bw = new BackgroundWorker();
            
            //bw.SetApartmentState(ApartmentState.STA);
            //bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            //bw.RunWorkerAsync();

            Thread t = new Thread(new ThreadStart(ThreadProc));
            t.SetApartmentState(ApartmentState.STA);

            t.Start();

        }

        public void ThreadProc() 
        {     
            Dispatcher.Invoke((Action)delegate() 
            {
                //for (int i = 1; i <= 10; i++)
                //{
                    Helper.NetworkDevice.GetPhotosFromDevice(Phone.IpAddr, Phone.Port);

                    IPhotoProvider photoProvider = new AndroidPhotoProvider();
                    foreach (PhotoScatterViewItem item in photoProvider.GetPhotos(Phone))
                    {
                        Helper.Surface.UserControls.Add(item);
                    }
                //}

            });
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            Helper.NetworkDevice.GetPhotosFromDevice(Phone.IpAddr, Phone.Port);

            IPhotoProvider photoProvider = new AndroidPhotoProvider();
            foreach (PhotoScatterViewItem item in photoProvider.GetPhotos(Phone))
            {
                Helper.Surface.UserControls.Add(item);
            }

        }

        private void unpinClick(object sender, RoutedEventArgs e)
        {
            btnPin.Visibility = Visibility.Visible;
            btnUnpin.Visibility = Visibility.Hidden;

            Phone.Pined = false;
            List<CustomScatterViewItem> items = Helper.Surface.UserControls.Where(x => x.Phone == Phone).ToList();

            try
            {
                for (int i = 0; i < items.Count; i++)
                {
                    CustomScatterViewItem item = items[i];
                    Helper.Surface.UserControls.Remove(item);
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}
