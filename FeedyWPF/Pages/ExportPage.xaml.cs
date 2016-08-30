﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FeedyWPF.Models;
namespace FeedyWPF.Pages
{
    /// <summary>
    /// Interaction logic for ExportPage.xaml
    /// </summary>
    public partial class ExportPage : Page
    {
        public ExportPage()
        {
            InitializeComponent();
        }
        public ExportPage(Evaluation evaluation)
        {
            InitializeComponent();
            Evaluation = evaluation;

            DataContext = Evaluation;
        }

        private Evaluation Evaluation { get; set; }
    }
}