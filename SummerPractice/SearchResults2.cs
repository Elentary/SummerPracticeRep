using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SummerPractice
{
  public partial class SearchResults2 : Form
  {
    private Cinema[] Cinemas;

    public SearchResults2()
    {
      InitializeComponent();
    }

    public SearchResults2(List<Cinema> cinemas)
    {
      InitializeComponent();
      Cinemas = cinemas.ToArray();
      foreach (var cinema in cinemas)
      {
        ListViewItem item = new ListViewItem(cinema.Name);
        item.SubItems.Add(cinema.Adress);
        item.SubItems.Add(cinema.CEO);
        item.SubItems.Add(cinema.getType());
        item.SubItems.Add(cinema.Capacity.ToString());
        listView1.Items.Add(item);
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (listView1.SelectedItems.Count != 0)
      {
        Program.MainForm.showCinema(Cinemas[listView1.SelectedIndices[0]]);
        Program.MainForm.LoadedCinema = Cinemas[listView1.SelectedIndices[0]];
      }
      this.Close();
    }

    private void button2_Click(object sender, EventArgs e)
    {
      this.Close();
    }
  }
}