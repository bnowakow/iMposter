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
using System.ComponentModel;

namespace iMposter.View.Video
{
    /// <summary>
    /// Interaction logic for VideoPreview.xaml
    /// </summary>
    public abstract partial class VideoPreview : UserControl
    {
        public ImageSource imageSource { get; set; }
        protected BackgroundWorker worker;

        public abstract void UpdateCamera();
        public abstract void InitializeCamera();

        public VideoPreview()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
            imageSource = null;
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)delegate
            {
                UpdateCamera();
                canvasRoot.Width = previewImage.ActualWidth;
                canvasRoot.Height = previewImage.ActualHeight;
            });
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (!worker.IsBusy)
            {
                worker.RunWorkerAsync();
            }
        }

        private void startPreviewButton_Click(object sender, RoutedEventArgs e)
        {
            Initialize();
            InitializeCamera();
        }
    }
}
