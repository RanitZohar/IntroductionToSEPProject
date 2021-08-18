﻿using BLAPI;
using System;
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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        IBl bl;
        public ManagerWindow()
        {
            InitializeComponent();
            bl = this.bl = BLAPI.BLFactory.getBL(); 
        }

        private void ShowBuses_Click(object sender, RoutedEventArgs e)  //the function open the ShowALLBuses window
        {
            ShowALLBuses window = new ShowALLBuses(bl);
            window.Show();
        }

        private void ShowLines_Click(object sender, RoutedEventArgs e)   //the function open the ShowAllLines window
        {
            ShowAllLines window = new ShowAllLines(bl);
            window.Show();
        }

        private void ShowStations_Click(object sender, RoutedEventArgs e)   //the function open the ShowAllStations window
        {
            ShowAllStations window = new ShowAllStations(bl);
            window.ShowDialog();
        }
    }
}
