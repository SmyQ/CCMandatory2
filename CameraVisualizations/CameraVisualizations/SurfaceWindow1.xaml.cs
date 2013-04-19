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
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using CameraVisualizations.Utils;
using CameraVisualizations.UserControls;

namespace CameraVisualizations
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent(); // Kamil

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            InitializeDefinitions();

            scatterView.ItemsSource = Helper.Surface.UserControls;
            Helper.ScatterView = scatterView;
            
        }

        void scatterView_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            //throw new NotImplementedException();
            int a = 1;
        }

        void scatterView_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            int b = 2;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }

        private void InitializeDefinitions()
        {
            foreach (Phone p in Helper.Surface.Phones)
            {
                TagVisualizationDefinition tagDef =
                   new TagVisualizationDefinition();
                // The tag value that this definition will respond to.
                tagDef.Value = p.Id;
                // The .xaml file for the UI
                tagDef.Source =
                    new Uri("CameraVisualization.xaml", UriKind.Relative);
                // The maximum number for this tag value.
                tagDef.MaxCount = 2;
                // The visualization stays for 2 seconds.
                tagDef.LostTagTimeout = 0000.0;
                // Orientation offset (default).
                tagDef.OrientationOffsetFromTag = 0.0;
                // Physical offset (horizontal inches, vertical inches).
                tagDef.PhysicalCenterOffsetFromTag = new Vector(2.0, 2.0);
                // Tag removal behavior (default).
                tagDef.TagRemovedBehavior = TagRemovedBehavior.Disappear;
                // Orient UI to tag? (default).
                tagDef.UsesTagOrientation = true;
                // Add the definition to the collection.
                MyTagVisualizer.Definitions.Add(tagDef);
            }
        }

        private void OnVisualizationAdded(object sender, TagVisualizerEventArgs e)
        {
            CameraVisualization camera = (CameraVisualization)e.TagVisualization;
            ucPhone ucPhoneRep = camera.ucPhoneRep;

            int detectedId = Int32.Parse(camera.VisualizedTag.Value.ToString());
            Phone phone = Helper.Surface.Phones.Where(p => p.Id == detectedId).FirstOrDefault();

            if (phone != null)
            {      
                ucPhoneRep.CameraModel.Content = phone.Name;
                ucPhoneRep.myEllipse.Fill = phone.BrushColor;
                ucPhoneRep.imgPhoneModel.Source = new BitmapImage(new Uri(@"/CameraVisualizations;component/Resources/" + phone.ImgSrc, UriKind.Relative));
                ucPhoneRep.Phone = phone;
                
                if (phone.Pined == true)
                {
                    Helper.Surface.UserControls.Remove(Helper.Surface.UserControls.Where(u => u.Phone == phone && u is PhoneScatterViewItem).FirstOrDefault());
                    ucPhoneRep.btnPin.Visibility = Visibility.Hidden;
                    ucPhoneRep.btnUnpin.Visibility = Visibility.Visible;
                }
                
            }
            else
            {
                ucPhoneRep.CameraModel.Content = "UNKNOWN MODEL";
                ucPhoneRep.myEllipse.Fill = SurfaceColors.ControlAccentBrush;
            }
        }

        private void OnVisualizationRemoved(object sender, TagVisualizerEventArgs e)
        {
            CameraVisualization camera = (CameraVisualization)e.TagVisualization;
            int detectedId = Int32.Parse(camera.VisualizedTag.Value.ToString());
            Phone phone = Helper.Surface.Phones.Where(p => p.Id == detectedId).FirstOrDefault();

            if (phone != null && phone.Pined == true)
            {
                ucPhone x = camera.ucPhoneRep;
                camera.myGrid.Children.Remove(camera.ucPhoneRep);


                PhoneScatterViewItem item = new PhoneScatterViewItem();

                item.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(item_ManipulationStarted);
                item.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(item_ManipulationCompleted);
                item.Phone = phone;
                item.Content = x;

                item.Width = e.TagVisualization.Width;
                item.Height = e.TagVisualization.Height;
                item.Background = Brushes.White;
                item.Center = e.TagVisualization.Center;
                item.Orientation = 0;
                item.BorderThickness = new Thickness(0);
                item.ShowsActivationEffects = false;

                RoutedEventHandler loadedEventHandler = null;
                loadedEventHandler = new RoutedEventHandler(delegate
                {
                    item.Loaded -= loadedEventHandler;
                    Microsoft.Surface.Presentation.Generic.SurfaceShadowChrome ssc;
                    ssc = item.Template.FindName("shadow", item) as Microsoft.Surface.Presentation.Generic.SurfaceShadowChrome;
                    ssc.Visibility = Visibility.Hidden;
                });
                item.Loaded += loadedEventHandler;

                Helper.Surface.UserControls.Add(item);


                //scatterView.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(scatterView_ManipulationStarted);
                //scatterView.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(scatterView_ManipulationCompleted);
                //scatterView.Items.Add(item);
            }
        }

        void item_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            int i = 1;
            //throw new NotImplementedException();
        }

        void item_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            int b = 1;
            //throw new NotImplementedException();
        }

       

    }
}
