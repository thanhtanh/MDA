using DatingUniversalApp.Models.Business;
using DatingUniversalApp.Models.PageViewModels;
using DatingUniversalApp.Models.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace DatingUniversalApp.Views.Dialog
{
    public sealed partial class NewStatusDialog : ContentDialog
    {

        public string StatusContent
        {
            get
            {
                return this.txtContent.Text;
            }

            set
            {
                this.txtContent.Text = value;
            }
        }

        public bool ShareLocation
        {
            get
            {
                return this.chkShareLocation.IsChecked.Value;
            }
            set
            {
                this.chkShareLocation.IsChecked = value;
            }
        }

        private NewStatusDialog()
        {
            this.InitializeComponent();
        }

        public NewStatusDialog(string content)
            : this()
        {
            this.txtContent.Text = content;
        }

    }
}
