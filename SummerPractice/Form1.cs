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
    private string default_base_cinema = "default_base_cinema.cinema", default_base_movie = "default_base_movie.movie";


    public Form1()
    {
      InitializeComponent();
      Load(default_base_cinema, default_base_movie);
    }

    private void exit(object sender, EventArgs e)
    {
      Application.Exit();
    }

    private void close(object sender, FormClosedEventArgs e)
    {
      Application.Exit();
    }

    //Tab 1

    private void searchMovie(object sender, EventArgs e)
    {
      string field_name;
      try
      {
        switch (comboBox1.SelectedIndex)
        {
          case 0:
            field_name = "Title";
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
          if (textBox9.Text == "")
          {
            result.Add(movie);
            continue;
          }
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
      listView1.Items.Clear();
      foreach (var cinema in movie.Cinemas)
      {
        if (cinema.Dates.ContainsKey(movie))
        {
          ListViewItem item = new ListViewItem(cinema.Name);
          item.SubItems.Add(cinema.Dates[movie].Item1.ToShortDateString());
          item.SubItems.Add(cinema.Dates[movie].Item2.ToShortDateString());
          listView1.Items.Add(item);
        }
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

    private void deleteCinemaDates(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
      {
        if (listView1.SelectedIndices.Count != 0)
          listView1.Items.RemoveAt(listView1.SelectedIndices[0]);
      }
    }

    private void addMovie(object sender, EventArgs e)
    {
      try
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
                DateTime.ParseExact(listView1.Items[i].SubItems[1].Text, "dd.MM.yyyy", null),
                DateTime.ParseExact(listView1.Items[i].SubItems[2].Text, "dd.MM.yyyy", null)));
              cinema.Movies.Add(movie);
            }
            lst.Add(cinema);
          }
        }
        movie.Cinemas = lst;
        if (control.Movies.Contains(movie))
        {
          if (
            MessageBox.Show("Данный фильм уже имеется, все равно добавить ?", "Предупреждение",
              MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
            return;
        }
        foreach (var movie1 in control.Movies)
        {
          if (movie1.Title == movie.Title)
            if (
              MessageBox.Show("Фильм схож с имеющимся, все равно добавить ?", "Предупреждение",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
              return;
        }
        control.Movies.Add(movie);
        checkBox1.Checked = false;
        movieChange(sender, e);
        textBox1.Text = "";
        textBox2.Text = "";
        textBox3.Text = "";
        textBox4.Text = "";
        textBox5.Text = "";
        textBox6.Text = "";
        textBox7.Text = "";
        textBox8.Text = "";
        listView1.Items.Clear();
      }
      catch (Exception exception)
      {
        Program.log.Error(exception);
        MessageBox.Show(exception.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void removeMovie(object sender, EventArgs e)
    {
      for (int i = 0; i < listView1.Items.Count; i++)
      {
        foreach (var cinema in control.Cinemas)
        {
          if (cinema.Name == listView1.Items[i].Text)
          {
            if (cinema.Dates.ContainsKey(LoadedMovie))
              for (DateTime day = cinema.Dates[LoadedMovie].Item1.AddDays(-10);
                day != cinema.Dates[LoadedMovie].Item2.AddDays(1);)
              {
                cinema.Attendance.Remove(new Tuple<Movie, DateTime>(LoadedMovie, day));
                day = day.AddDays(1);
              }
            cinema.Dates.Remove(LoadedMovie);
            cinema.Movies.Remove(LoadedMovie);
            break;
          }
        }
      }
      control.Movies.Remove(LoadedMovie);
    }

    //Tab 2
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
          if (textBox10.Text == "")
          {
            result.Add(cinema);
            continue;
          }
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

    public void showCinema(Cinema cinema)
    {
      textBox13.Text = cinema.Capacity.ToString();
      textBox14.Text = cinema.getType();
      textBox15.Text = cinema.CEO;
      textBox16.Text = cinema.Adress;
      textBox17.Text = cinema.Name;
      listView2.Items.Clear();
      listView3.Items.Clear();
      foreach (var movie in cinema.Movies)
      {
        if (cinema.Dates.ContainsKey(movie))
        {
          ListViewItem it = new ListViewItem(movie.Title);
          it.SubItems.Add(cinema.Dates[movie].Item1.ToShortDateString());
          it.SubItems.Add(cinema.Dates[movie].Item2.ToShortDateString());
          listView2.Items.Add(it);
        }
      }
      foreach (var obj in cinema.Attendance)
      {
        ListViewItem item = new ListViewItem(obj.Key.Item1.Title);
        item.SubItems.Add(obj.Key.Item2.ToString());
        item.SubItems.Add(obj.Value.ToString());
        listView3.Items.Add(item);
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
        dateTimePicker3.Visible = true;
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
        dateTimePicker3.Visible = false;
        comboBox4.Visible = false;
        button6.Visible = false;
        button7.Visible = false;
        button8.Visible = false;
      }
    }

    private void AddCinema(object sender, EventArgs e)
    {
      try
      {
        Cinema cinema = new Cinema(textBox17.Text, textBox14.Text, Int32.Parse(textBox13.Text),
          textBox16.Text, textBox15.Text, new List<Movie>(), new SortedDictionary<Movie, Tuple<DateTime, DateTime>>(),
          new SortedDictionary<Tuple<Movie, DateTime>, int>(new MovieDateComparer()));
        foreach (var movie in control.Movies)
        {
          for (int i = 0; i < listView2.Items.Count; i++)
          {
            if (listView2.Items[i].Text == movie.Title)
            {
              cinema.Dates.Add(movie,
                new Tuple<DateTime, DateTime>(
                  DateTime.ParseExact(listView2.Items[i].SubItems[1].Text, "dd.MM.yyyy", null),
                  DateTime.ParseExact(listView2.Items[i].SubItems[2].Text, "dd.MM.yyyy", null)));
              cinema.Movies.Add(movie);
            }
          }
          for (int i = 0; i < listView3.Items.Count; i++)
          {
            if (listView3.Items[i].Text == movie.Title)
            {
              cinema.Attendance.Add(
                new Tuple<Movie, DateTime>(movie,
                  DateTime.ParseExact(listView3.Items[i].SubItems[2].Text, "dd.MM.yyyy", null)),
                Int32.Parse(listView3.Items[i].SubItems[1].Text));
            }
          }
        }
        control.Cinemas.Add(cinema);
        textBox13.Text = "";
        textBox14.Text = "";
        textBox15.Text = "";
        textBox16.Text = "";
        textBox17.Text = "";
        listView2.Items.Clear();
        listView3.Items.Clear();
      }
      catch (Exception exception)
      {
        Program.log.Error(exception);
        MessageBox.Show(exception.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    private void addMovieDates(object sender, EventArgs e)
    {
      try
      {
        if (comboBox4.SelectedItem == null)
          throw new Exception("Ничего не выбрано");
        ListViewItem item = new ListViewItem(comboBox4.SelectedItem.ToString());
        if (item.Text == "")
          throw new Exception("Пустое имя фильма");
        if (dateTimePicker5.Value > dateTimePicker4.Value)
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

    private void deleteMovieDates(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
      {
        if (listView2.SelectedIndices.Count != 0)
          listView2.Items.RemoveAt(listView2.SelectedIndices[0]);
      }
    }

    private void Update(string filename, string filename2)
    {
      try
      {
        using (var file = File.Open(filename, FileMode.Create))
        {
          Serializer.Serialize(file, control.Cinemas);
        }
        using (var file = File.Open(filename2, FileMode.Create))
        {
          Serializer.Serialize(file, control.Movies);
        }
      }
      catch (Exception exception)
      {
        Program.log.Info(exception);
        MessageBox.Show(exception.Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    private void Load(string filename, string filename2)
    {
      try
      {
        using (var file = File.Open(filename, FileMode.OpenOrCreate))
        {
          control.Cinemas = Serializer.Deserialize<List<Cinema>>(file);
        }
        using (var file = File.Open(filename2, FileMode.OpenOrCreate))
        {
          control.Movies = Serializer.Deserialize<List<Movie>>(file);
        }
        foreach (var movie in control.Movies)
        {
          List<Cinema> lst = new List<Cinema>();
          foreach (var cinema in movie.Cinemas)
          {
            if (!lst.Contains(cinema))
              lst.Add(cinema);
          }
          movie.Cinemas = lst;
        }
      }
      catch (Exception exception)
      {
        Program.log.Info(exception);
        MessageBox.Show(exception.Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    private void loadBase(object sender, EventArgs e)
    {
      openFileDialog1.FileName = default_base_cinema;
      openFileDialog1.Filter = "База кинотеатров|*.cinema";
      openFileDialog1.Title = "Откройте файл с кинотеатрами";
      if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        string filename = openFileDialog1.FileName;
        openFileDialog1.FileName = default_base_movie;
        openFileDialog1.Filter = "База фильмов|*.movie";
        openFileDialog1.Title = "Откройте файл с фильмами";
        if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
          Load(filename, openFileDialog1.FileName);
          MessageBox.Show("Файл загружен", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      }
    }

    private void saveBase(object sender, EventArgs e)
    {
      saveFileDialog1.FileName = default_base_cinema;
      saveFileDialog1.Filter = "База кинотеатров|*.cinema";
      saveFileDialog1.Title = "Сохраните файл с кинотеатрами";
      saveFileDialog1.ShowDialog();
      if (saveFileDialog1.FileName != "")
      {
        string filename = saveFileDialog1.FileName;
        saveFileDialog1.FileName = default_base_movie;
        saveFileDialog1.Filter = "База фильмов|*.movie";
        saveFileDialog1.Title = "Сохраните файл с фильмами";
        saveFileDialog1.ShowDialog();
        if (saveFileDialog1.FileName != "")
        {
          Update(filename, saveFileDialog1.FileName);
          MessageBox.Show("Файл сохранен", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      }
    }

    private void addAttendance(object sender, EventArgs e)
    {
      try
      {
        if (listView2.SelectedIndices.Count == 0)
          throw new Exception("Не выбран фильм для добавления посещений");
        foreach (var movie in LoadedCinema.Movies)
        {
          if (movie.Title == listView2.SelectedItems[0].Text)
          {
            ListViewItem item = new ListViewItem(movie.Title);
            item.SubItems.Add(numericUpDown1.Value.ToString());
            if (LoadedCinema.Dates.ContainsKey(movie))
            {
              if (dateTimePicker3.Value < LoadedCinema.Dates[movie].Item1 ||
                  dateTimePicker3.Value > LoadedCinema.Dates[movie].Item2)
                throw new Exception("Неверная дата для посещаемости");
            }
            item.SubItems.Add(dateTimePicker3.Value.ToShortDateString());
            if (listView3.Items.Contains(item))
              throw new Exception("Данный сеанс уже отмечен");
            listView3.Items.Add(item);
          }
        }
      }
      catch (Exception exception)
      {
        Program.log.Info(exception);
        MessageBox.Show(exception.Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    //Tab3
    private void longestMovieReport(object sender, EventArgs e)
    {
      Tuple<List<Tuple<MovieReport, List<Cinema>>>, int> result = control.getLongestPeriodMovieInfo();
      listView5.Items.Clear();
      foreach (var elem in result.Item1)
      {
        ListViewItem item = new ListViewItem(elem.Item1.Title);
        item.SubItems.Add(elem.Item1.Genre);
        item.SubItems.Add(elem.Item1.Director);
        item.SubItems.Add(elem.Item1.ManStar);
        item.SubItems.Add(elem.Item1.WomanStar);
        item.SubItems.Add(elem.Item1.Company);
        item.SubItems.Add(elem.Item1.Year.ToString());
        item.SubItems.Add(elem.Item1.Cost.ToString());
        item.SubItems.Add(result.Item2.ToString());
        listView5.Items.Add(item);
      }
      tabControl2.SelectTab(0);
    }

    private void ShowMovieFromLongest(object sender, EventArgs e)
    {
      try
      {
        if (listView5.SelectedItems.Count == 0)
          throw new Exception("Фильм не выбран");
        foreach (var movie in control.Movies)
        {
          if (movie.Title == listView5.SelectedItems[0].Text)
          {
            showMovie(movie);
            tabControl1.SelectTab(0);
            Program.MainForm.LoadedMovie = movie;
            return;
          }
        }
      }
      catch (Exception exception)
      {
        Program.log.Info(exception);
        MessageBox.Show(exception.Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    private void profitReport(object sender, EventArgs e)
    {
      try
      {
        if (dateTimePicker6.Value > dateTimePicker7.Value)
          throw new Exception("Неправильно выставлен период");
        PeriodReport[] report =
          control.getPeriodReport(new Tuple<DateTime, DateTime>(dateTimePicker6.Value, dateTimePicker7.Value));
        int i = 0;
        foreach (var elem in report)
        {
          i++;
          chart1.Series[0].Points.AddXY(i, elem.TotalProfit);
          chart1.Series[1].Points.AddXY(i, elem.AverageProfit);

          ListViewItem item = new ListViewItem(i.ToString());
          item.SubItems.Add(elem.cinemaName);
          listView8.Items.Add(item);
        }
        tabControl2.SelectTab(1);
      }
      catch (Exception exception)
      {
        Program.log.Info(exception);
        MessageBox.Show(exception.Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    private void weekMovies(object sender, EventArgs e)
    {
      DateTime[] period = {dateTimePicker8.Value, dateTimePicker8.Value};
      while (period[0].DayOfWeek != DayOfWeek.Monday)
        period[0] = period[0].AddDays(-1);
      while (period[1].DayOfWeek != DayOfWeek.Sunday)
        period[1] = period[1].AddDays(1);
      WeekReport report = control.getWeekReport(new Tuple<DateTime, DateTime>(period[0], period[1]));
      Array.Sort(report.MovieReports.ToArray(), new MovieReportComparer());
      foreach (var movieReport in report.MovieReports)
      {
        ListViewItem item = new ListViewItem(movieReport.Title);
        item.SubItems.Add(movieReport.Genre);
        item.SubItems.Add(movieReport.Director);
        item.SubItems.Add(movieReport.ManStar);
        item.SubItems.Add(movieReport.WomanStar);
        item.SubItems.Add(movieReport.Company);
        item.SubItems.Add(movieReport.Year.ToString());
        item.SubItems.Add(movieReport.Cost.ToString());
        listView4.Items.Add(item);
      }
      tabControl2.SelectTab(2);
    }

    private void showMovieFromWeek(object sender, EventArgs e)
    {
      try
      {
        if (listView4.SelectedItems.Count == 0)
          throw new Exception("Фильм не выбран");
        foreach (var movie in control.Movies)
        {
          if (movie.Title == listView4.SelectedItems[0].Text)
          {
            showMovie(movie);
            tabControl1.SelectTab(0);
            Program.MainForm.LoadedMovie = movie;
            return;
          }
        }
      }
      catch (Exception exception)
      {
        Program.log.Info(exception);
        MessageBox.Show(exception.Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    private void showMostProfit(object sender, EventArgs e)
    {
      try
      {
        if (listView6.SelectedItems.Count == 0)
          throw new Exception("Фильм не выбран");
        foreach (var movie in control.Movies)
        {
          if (movie.Title == listView6.SelectedItems[0].Text)
          {
            showMovie(movie);
            tabControl1.SelectTab(0);
            Program.MainForm.LoadedMovie = movie;
            return;
          }
        }
      }
      catch (Exception exception)
      {
        Program.log.Info(exception);
        MessageBox.Show(exception.Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    private void profitFromCinemas(object sender, EventArgs e)
    {
      try
      {
        if (comboBox5.SelectedItem == null)
          throw new Exception("Фильм не выбран");
        List<Tuple<Cinema, double>> result = new List<Tuple<Cinema, double>>();
        foreach (var movie in control.Movies)
        {
          if (movie.Title != comboBox5.SelectedItem.ToString())
            continue;

          foreach (var cinema in control.Cinemas)
          {
            int attendance = 0;
            if (cinema.Dates.ContainsKey(movie))
              for (DateTime day = cinema.Dates[movie].Item1; day != cinema.Dates[movie].Item2;)
              {
                if (cinema.Attendance.ContainsKey(new Tuple<Movie, DateTime>(movie, day)))
                  attendance += cinema.Attendance[new Tuple<Movie, DateTime>(movie, day)];
                day = day.AddDays(1);
              }
            result.Add(new Tuple<Cinema, double>(cinema, attendance * movie.Cost));
          }
          break;
        }

        foreach (var elem in result)
        {
          ListViewItem item = new ListViewItem(elem.Item1.Name);
          item.SubItems.Add(elem.Item2.ToString());
          listView7.Items.Add(item);
        }
        tabControl2.SelectTab(4);
      }
      catch (Exception exception)
      {
        Program.log.Info(exception);
        MessageBox.Show(exception.Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    private void tabControl1_Selected(object sender, TabControlEventArgs e)
    {
      comboBox5.Items.Clear();
      foreach (var movie in control.Movies)
      {
        comboBox5.Items.Add(movie.Title);
      }
    }

    private void bestCass(object sender, EventArgs e)
    {
      Tuple<MovieReport[], double> results = control.getMostProfitMovies();
      foreach (var movieReport in results.Item1)
      {
        ListViewItem item = new ListViewItem(movieReport.Title);
        item.SubItems.Add(movieReport.Genre);
        item.SubItems.Add(movieReport.Director);
        item.SubItems.Add(movieReport.ManStar);
        item.SubItems.Add(movieReport.WomanStar);
        item.SubItems.Add(movieReport.Company);
        item.SubItems.Add(movieReport.Year.ToString());
        item.SubItems.Add(movieReport.Cost.ToString());
        item.SubItems.Add(results.Item2.ToString());
        listView6.Items.Add(item);
      }
      tabControl2.SelectTab(3);
    }
  }
}