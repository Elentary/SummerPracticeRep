using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog.LayoutRenderers;

namespace SummerPractice
{
  public partial class SearchResults : Form
  {
    private Movie[] Movies;

    public SearchResults()
    {
      InitializeComponent();
    }

    public SearchResults(List<Movie> movies)
    {
      InitializeComponent();
      Movies = movies.ToArray();
      foreach (var movie in movies)
      {
        ListViewItem item = new ListViewItem(movie.Title);
        item.SubItems.Add(movie.Genre);
        item.SubItems.Add(movie.Director);
        item.SubItems.Add(movie.ManStar);
        item.SubItems.Add(movie.WomanStar);
        item.SubItems.Add(movie.Company);
        item.SubItems.Add(movie.Year.ToString());
        item.SubItems.Add(movie.Cost.ToString());
        listView1.Items.Add(item);
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (listView1.SelectedItems.Count != 0)
      {
        Program.MainForm.showMovie(Movies[listView1.SelectedIndices[0]]);
        Program.MainForm.LoadedMovie = Movies[listView1.SelectedIndices[0]];
      }
      this.Close();
    }

    private void button2_Click(object sender, EventArgs e)
    {
      this.Close();
    }
  }
}