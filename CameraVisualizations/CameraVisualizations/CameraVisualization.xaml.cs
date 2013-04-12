using System;
using System.Collections.Generic;
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
using CameraVisualizations.UserControls;

namespace CameraVisualizations
{
    /// <summary>
    /// Interaction logic for CameraVisualization.xaml
    /// </summary>
    public partial class CameraVisualization : TagVisualization
    {
        public ucPhone ucPhoneRep { get; set; }

        public CameraVisualization()
        {
            InitializeComponent();
            ucPhoneRep = new ucPhone();
            myGrid.Children.Add(ucPhoneRep);
        }

        private void CameraVisualization_Loaded(object sender, RoutedEventArgs e)
        {
            //TODO: customize CameraVisualization's UI based on this.VisualizedTag here
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
