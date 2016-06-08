using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog.Fluent;
using ProtoBuf;

namespace SummerPractice
{
  public partial class Form1 : Form
  {
    public ControlCinema control = new ControlCinema();
    public Movie LoadedMovie = null;
    public Cinema LoadedCinema = null;

    public Form1()
    {
      InitializeComponent();
      Load("default_base.txt");
    }

    private void exit(object sender, EventArgs e)
    {
      Application.Exit();
    }

    private void close(object sender, FormClosedEventArgs e)
    {
      Application.Exit();
    }

    private void searchMovie(object sender, EventArgs e)
    {
      string field_name;
      try
      {
        switch (comboBox1.SelectedIndex)
        {
          case 0:
            field_name = "Name";
            break;
          case 1:
            field_name = "Genre";
            break;
          case 2:
            field_name = "Director";
            break;
          case 3:
            field_name = "ManStar";
            break;
          case 4:
            field_name = "WomanStar";
            break;
          case 5:
            field_name = "Company";
            break;
          case 6:
            field_name = "Year";
            break;
          case 7:
            field_name = "Cost";
            break;
          default:
            throw new Exception("Невозможно распознать выбранный критерий поиска");
        }
        List<Movie> result = new List<Movie>();
        foreach (var movie in control.Movies)
        {
          foreach (var field in movie.GetType().GetFields())
          {
            if (field.Name == field_name)
              if (field.GetValue(movie).ToString() == textBox9.Text)
                result.Add(movie);
          }
        }

        (new SearchResults(result)).ShowDialog();
      }
      catch (Exception exception)
      {
        Program.log.Info(exception);
        MessageBox.Show(exception.Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    public void showMovie(Movie movie)
    {
      textBox1.Text = movie.Title;
      textBox2.Text = movie.Genre;
      textBox3.Text = movie.Director;
      textBox4.Text = movie.ManStar;
      textBox5.Text = movie.WomanStar;
      textBox6.Text = movie.Company;
      textBox7.Text = movie.Year.ToString();
      textBox8.Text = movie.Cost.ToString();

      foreach (var cinema in movie.Cinemas)
      {
        ListViewItem item = new ListViewItem(cinema.Name);
        item.SubItems.Add(cinema.Dates[movie].Item1.ToShortDateString());
        item.SubItems.Add(cinema.Dates[movie].Item2.ToShortDateString());
        listView1.Items.Add(item);
      }
    }

    public void showCinema(Cinema cinema)
    {
      textBox13.Text = cinema.Capacity.ToString();
      textBox14.Text = cinema.getType();
      textBox15.Text = cinema.CEO;
      textBox16.Text = cinema.Adress;
      textBox17.Text = cinema.Name;
      foreach (var movie in cinema.Movies)
      {
        ListViewItem item = new ListViewItem(movie.Title);
        item.SubItems.Add(dateTimePicker3.Value.ToShortDateString());
        item.SubItems.Add(cinema.Attendance[new Tuple<Movie, DateTime>(movie, dateTimePicker3.Value)].ToString());
        listView3.Items.Add(item);

        ListViewItem it = new ListViewItem(movie.Title);
        it.SubItems.Add(cinema.Dates[movie].Item1.ToShortDateString());
        it.SubItems.Add(cinema.Dates[movie].Item2.ToShortDateString());
        listView2.Items.Add(it);
      }
    }

    private void movieChange(object sender, EventArgs e)
    {
      if (checkBox1.Checked)
      {
        dateTimePicker1.Visible = true;
        dateTimePicker2.Visible = true;
        textBox1.ReadOnly = false;
        textBox2.ReadOnly = false;
        textBox3.ReadOnly = false;
        textBox4.ReadOnly = false;
        textBox5.ReadOnly = false;
        textBox6.ReadOnly = false;
        textBox7.ReadOnly = false;
        textBox8.ReadOnly = false;
        button2.Visible = true;
        button3.Visible = true;
        addCinema.Visible = true;
        comboBox2.Visible = true;
        comboBox2.Items.Clear();
        foreach (var cinema in control.Cinemas)
        {
          comboBox2.Items.Add(cinema.Name);
        }
      }
      else
      {
        textBox1.ReadOnly = true;
        textBox2.ReadOnly = true;
        textBox3.ReadOnly = true;
        textBox4.ReadOnly = true;
        textBox5.ReadOnly = true;
        textBox6.ReadOnly = true;
        textBox7.ReadOnly = true;
        textBox8.ReadOnly = true;
        addCinema.Visible = false;
        button2.Visible = false;
        button3.Visible = false;
        dateTimePicker1.Visible = false;
        dateTimePicker2.Visible = false;
        comboBox2.Visible = false;
      }
    }

    private void addCinemaDates(object sender, EventArgs e)
    {
      try
      {
        ListViewItem item = new ListViewItem(comboBox2.SelectedItem.ToString());
        if (item.Text == "")
          throw new Exception("Пустое имя кинотеатра");
        if (dateTimePicker1.Value > dateTimePicker2.Value)
          throw new Exception("Дата начала проката больше даты окончания");
        item.SubItems.Add(dateTimePicker1.Value.ToShortDateString());
        item.SubItems.Add(dateTimePicker2.Value.ToShortDateString());
        listView1.Items.Add(item);
      }
      catch (Exception exception)
      {
        Program.log.Info(exception);
        MessageBox.Show(exception.Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    private void deleteCinemaFromList(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
      {
        if (listView1.SelectedIndices.Count != 0)
          listView1.Items.RemoveAt(listView1.SelectedIndices[0]);
      }
    }

    private void AddMovie(object sender, EventArgs e)
    {
      String[] strVals = new[]
      {textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text};
      Movie movie = new Movie(strVals, Int32.Parse(textBox7.Text), Double.Parse(textBox8.Text), null);
      List<Cinema> lst = new List<Cinema>();
      for (int i = 0; i < listView1.Items.Count; i++)
      {
        foreach (var cinema in control.Cinemas)
        {
          if (cinema.Name == listView1.Items[i].Text)
          {
            cinema.Dates.Add(movie, new Tuple<DateTime, DateTime>(
              DateTime.ParseExact(listView1.Items[i].SubItems[0].ToString(), "dd.MM.yyyy", null),
              DateTime.ParseExact(listView1.Items[i].SubItems[0].ToString(), "dd.MM.yyyy", null)));
          }
          lst.Add(cinema);
        }
      }
      movie.Cinemas = lst;
      control.Movies.Add(movie);
      Update("default_base.txt");
    }

    private void removeMovie(object sender, EventArgs e)
    {
      for (int i = 0; i < listView1.Items.Count; i++)
      {
        foreach (var cinema in control.Cinemas)
        {
          if (cinema.Name == listView1.Items[i].Text)
          {
            foreach (var key in cinema.Attendance.Keys)
            {
              if (key.Item1 == LoadedMovie)
                cinema.Attendance.Remove(key);
            }
            cinema.Dates.Remove(LoadedMovie);
            cinema.Movies.Remove(LoadedMovie);
          }
        }
      }
      control.Movies.Remove(LoadedMovie);
    }

    private void cinemaSearch(object sender, EventArgs e)
    {
      string field_name;
      try
      {
        switch (comboBox3.SelectedIndex)
        {
          case 0:
            field_name = "Name";
            break;
          case 1:
            field_name = "Adress";
            break;
          case 2:
            field_name = "CEO";
            break;
          case 3:
            field_name = "Type";
            break;
          case 4:
            field_name = "Capacity";
            break;
          default:
            throw new Exception("Невозможно распознать выбранный критерий поиска");
        }
        List<Cinema> result = new List<Cinema>();
        foreach (var cinema in control.Cinemas)
        {
          foreach (var field in cinema.GetType().GetFields())
          {
            if (field.Name == field_name)
              if (field.GetValue(cinema).ToString() == textBox10.Text)
                result.Add(cinema);
          }
        }

        (new SearchResults2(result)).ShowDialog();
      }
      catch (Exception exception)
      {
        Program.log.Info(exception);
        MessageBox.Show(exception.Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    private void cinemaChange(object sender, EventArgs e)
    {
      if (checkBox2.Checked)
      {
        textBox13.ReadOnly = false;
        textBox14.ReadOnly = false;
        textBox15.ReadOnly = false;
        textBox16.ReadOnly = false;
        textBox17.ReadOnly = false;
        numericUpDown1.Visible = true;
        button9.Visible = true;
        dateTimePicker4.Visible = true;
        dateTimePicker5.Visible = true;
        comboBox4.Visible = true;
        comboBox4.Items.Clear();
        foreach (var movie in control.Movies)
        {
          comboBox4.Items.Add(movie.Title);
        }
        button6.Visible = true;
        button7.Visible = true;
        button8.Visible = true;
      }
      else
      {
        textBox13.ReadOnly = true;
        textBox14.ReadOnly = true;
        textBox15.ReadOnly = true;
        textBox16.ReadOnly = true;
        textBox17.ReadOnly = true;
        numericUpDown1.Visible = false;
        button9.Visible = false;
        dateTimePicker4.Visible = false;
        dateTimePicker5.Visible = false;
        comboBox4.Visible = false;
        button6.Visible = false;
        button7.Visible = false;
        button8.Visible = false;
      }
    }

    private void AddCinema(object sender, EventArgs e)
    {
      Cinema cinema = new Cinema(textBox17.Text, textBox14.Text, Int32.Parse(textBox13.Text),
        textBox16.Text, textBox15.Text, new List<Movie>(), new SortedDictionary<Movie, Tuple<DateTime, DateTime>>(),
        new SortedDictionary<Tuple<Movie, DateTime>, int>());
      foreach (var movie in control.Movies)
      {
        for (int i = 0; i < listView2.Items.Count; i++)
        {
          if (listView2.Items[i].Text == movie.Title)
          {
            cinema.Dates.Add(movie,
              new Tuple<DateTime, DateTime>(
                DateTime.ParseExact(listView2.Items[i].SubItems[0].Text, "dd.MM.yyyy", null),
                DateTime.ParseExact(listView2.Items[i].SubItems[1].Text, "dd.MM.yyyy", null)));
            cinema.Movies.Add(movie);
          }
        }
        for (int i = 0; i < listView3.Items.Count; i++)
        {
          if (listView3.Items[i].Text == movie.Title)
          {
            cinema.Attendance.Add(
              new Tuple<Movie, DateTime>(movie,
                DateTime.ParseExact(listView3.Items[0].SubItems[1].Text, "dd.MM.yyyy", null)),
              Int32.Parse(listView3.Items[0].SubItems[0].Text));
          }
        }
      }
      control.Cinemas.Add(cinema);
      Update("default_base.txt");
    }

    private void Update(string filename)
    {
      using (var file = File.Create(filename))
      {
        Serializer.Serialize(file, control.Cinemas);
        Serializer.Serialize(file, control.Movies);
      }
    }

    private void Load(string filename)
    {
      using (var file = File.Open(filename, FileMode.OpenOrCreate))
      {
        control.Cinemas = Serializer.Deserialize<List<Cinema>>(file);
        control.Movies = Serializer.Deserialize<List<Movie>>(file);
      }
    }

    private void removeCinema(object sender, EventArgs e)
    {
      foreach (var movie in control.Movies)
      {
        movie.Cinemas.Remove(LoadedCinema);
      }
      control.Cinemas.Remove(LoadedCinema);
    }

    private void AddMovieDates(object sender, EventArgs e)
    {
      try
      {
        if (comboBox4.SelectedItem == null)
          throw new Exception("Ничего не выбрано");
        ListViewItem item = new ListViewItem(comboBox4.SelectedItem.ToString());
        if (item.Text == "")
          throw new Exception("Пустое имя фильма");
        if (dateTimePicker1.Value > dateTimePicker2.Value)
          throw new Exception("Дата начала проката больше даты окончания");
        item.SubItems.Add(dateTimePicker5.Value.ToShortDateString());
        item.SubItems.Add(dateTimePicker4.Value.ToShortDateString());
        listView2.Items.Add(item);
      }
      catch (Exception exception)
      {
        Program.log.Info(exception);
        MessageBox.Show(exception.Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    private void listView2_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
      {
        if (listView2.SelectedIndices.Count != 0)
          listView2.Items.RemoveAt(listView2.SelectedIndices[0]);
      }
    }
  }
}