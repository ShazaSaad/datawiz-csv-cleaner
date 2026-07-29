using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DataWiz
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var frame = new Frame();
            Content = frame;
        frame.Navigate(typeof(DataWiz.Views.MainPage));

        // After navigation, the Page instance will be created as the frame content.
        // Assign the HostWindow on the page's ViewModel here so it gets a valid Window instance
        // (avoids race condition where App.MainAppWindow may not be set yet).
        if (frame.Content is DataWiz.Views.MainPage page)
        {
            page.ViewModel.HostWindow = this;
        }
        }
    }
}
