﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SummerPractice
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }


    private void выходToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }

    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
    {
      Application.Exit();
    }
  }
}
