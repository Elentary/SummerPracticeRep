using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SummerPractice
{
  public partial class LoadingScreen : Form
  {
    public LoadingScreen()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      if (progressBar1.Value >= 100)
      {
        timer1.Stop();
        this.Hide();
        (new Form1()).Show();
        return;
      }
      progressBar1.Value++;
    }
  }
}
