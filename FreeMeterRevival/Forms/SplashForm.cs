using ComponentFactory.Krypton.Toolkit;
using System.Threading;

namespace FreeMeterRevival.Forms
{
    public partial class SplashForm : KryptonForm
    {
        public SplashForm()
        {
            this.Refresh();
            InitializeComponent();
            this.Refresh();
        }
        public void ShowState(string state)
        {
            lblState.Text = state;

            this.Refresh();
        }
        public void CloseWithSleep()
        {
            bgwClose.RunWorkerAsync();
        }

        private void bgwClose_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Thread.Sleep(1000);
           
        }

        private void bgwClose_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }
    }
}
