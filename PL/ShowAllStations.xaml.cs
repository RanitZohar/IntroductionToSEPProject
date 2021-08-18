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
    /// Interaction logic for ShowAllStations.xaml
    /// </summary>
    public partial class ShowAllStations : Window
    {
        IBl bl;
        public ShowAllStations(IBl bl)  // the window get IBL
        {
            InitializeComponent();
            this.bl = bl;
            ShowAllStationsDataGrid.ItemsSource = bl.getAllStations();
        }
        private void RefreshStations()// בגלל שעשינו שינויים בכפתורי התידלוק והטיפול נקרא לריפרש שיחפש שוב את האוטובוס ע"י מספר רישוי ויעדכן את הנתונים לתןך 
        {  // המיין גריד נקודה דאטה קונטקס ויציג את הנתונים בחלון
            ShowAllStationsDataGrid.ItemsSource = bl.getAllStations();
            ShowAllStationsDataGrid.Items.Refresh();
        }
        private void ShowAllStationsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e) // the function send the line that the user press on and open new window that will show the station that line go through
        {
            BO.Station row = (BO.Station)ShowAllStationsDataGrid.SelectedItems[0];
            int code = row.Code;
            BO.Station station = bl.getStationByCode(code);
            List<BO.LineStation> BOLineStations =(List<BO.LineStation>)bl.getLineStationByCode(code);
            StationDetails StationDetail = new StationDetails(bl,station,BOLineStations);
            StationDetail.ShowDialog();
        }

        private void AddStationButton_Click(object sender, RoutedEventArgs e)  // the function open the AddStationWindow window 
        {
            AddStationWindow addstation = new AddStationWindow(bl);
            addstation.ShowDialog();
            RefreshStations();
        }
        private void UpdateBtn_Click(object sender, RoutedEventArgs e) // press on the one of the tidluk button  in one of the bus in the table start processor
        {
            BO.Station row = (BO.Station)ShowAllStationsDataGrid.SelectedItems[0];
            int code = row.Code;
            BO.Station BOStation = bl.getStationByCode(code);
            UpdateStationWindow updateStation = new UpdateStationWindow(bl, BOStation);
            updateStation.ShowDialog();
            RefreshStations();
        }
    }
}
