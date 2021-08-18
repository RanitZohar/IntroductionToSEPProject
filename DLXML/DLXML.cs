﻿using DLAPI;
using DO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DL
{
    public class DLXML : IDL
    {
        #region singleton
        static readonly DLXML instance = new DLXML();
        public static IDL Instance { get => instance; }


        static DLXML()
        {
        }
        #endregion singleton


        static XElement LineStationRoot = new XElement("LineStations");
        static XElement BusRoot = new XElement("Busses");
        static XElement StationRoot = new XElement("Stations");
        static XElement RunningNumbersRoot = new XElement("IDS");
        static XElement LineTripRoot = new XElement("LineTrip");

        static string BusPath = @"Bus.xml";
        static string lineStationPath = @"LineStation.xml";
        static string LinePath = @"Line.xml";
        static string StationPath = @"Station.xml";
        static string RunningNumbersPath = @"RunningNumbers.xml";
        static string LineTripPath = @"LineTrip.xml";
   


        private DLXML() { }

        #region Bus
        public void addBus(Bus bus)   //the function get Bus variable and check if it`s already exist in the xml, if yes throw excepation, else add it
        {
            List<DO.Bus> buses = XMLTools.LoadListFromXMLSerializer<Bus>(BusPath);
            if (buses.FirstOrDefault(x => x.LicenseNum == bus.LicenseNum) != null)
                throw new AlreadyExistsException("This license num already exists!", bus.LicenseNum);

            buses.Add(bus);
            XMLTools.SaveListToXMLSerializer<Bus>(buses, BusPath);
        }
        DO.Bus ConvertBus(XElement element)  // convert xml object to Bus Type 
        {
            Bus bus = new Bus();
            var ser = new XmlSerializer(typeof(Bus));
            return (Bus)ser.Deserialize(element.CreateReader());
        }
        public IEnumerable<DO.Bus> getAllBusses()   //the funcation return all the list of all busses
        {
            List<DO.Bus> buses = XMLTools.LoadListFromXMLSerializer<Bus>(BusPath);

            return from bus in buses
                   select bus;
        }
        public void updateBus(Bus bus)  //the function get Bus variable and check if it`s  exist in then xml, if no throw excepation, else update it
        {
            List<Bus> buses = XMLTools.LoadListFromXMLSerializer<Bus>(BusPath);
            Bus busToUpdate = buses.FirstOrDefault(x => x.LicenseNum == bus.LicenseNum);
            if (busToUpdate == null)
                throw new NotExistException("This license num doenst exist", bus.LicenseNum);
            buses.Remove(busToUpdate);
            buses.Add(bus);
            XMLTools.SaveListToXMLSerializer<Bus>(buses, BusPath);
        }
        public void deleteBus(Bus bus)  //the function get Bus variable and check if it`s  exist in the xml, if no throw excepation, else delete it
        {
            List<Bus> buses = XMLTools.LoadListFromXMLSerializer<Bus>(BusPath);
            Bus busToDelete = buses.FirstOrDefault(x => x.LicenseNum == bus.LicenseNum);
            if (busToDelete == null)
                throw new NotExistException("This license num doenst exist", bus.LicenseNum);
            buses.Remove(busToDelete);
            XMLTools.SaveListToXMLSerializer<Bus>(buses, BusPath);
        }
        public Bus getBusByLicenseNum(int licenseNum)  //the function get licenseNum and return the bus that found
        {
            List<Bus> buses = XMLTools.LoadListFromXMLSerializer<Bus>(BusPath);
            Bus busToReturn = (from bus in buses
                               where bus.LicenseNum == licenseNum
                               select bus).FirstOrDefault();

            if (busToReturn == null)
                throw new NotExistException("License num doesnt exist!", licenseNum);
            return busToReturn;
        }
        #endregion Bus

        #region Line
        public IEnumerable<DO.Line> getAllLines()  //the funcation return all the list of all Lines
        {
            List<DO.Line> lines = XMLTools.LoadListFromXMLSerializer<Line>(LinePath);

            return from line in lines
                   select line;
        }
        public void addLine(Line line)  //the function get Line variable and check if it`s already exist in the xml, if yes throw excepation, else add it
        {
            List<DO.Line> lines = XMLTools.LoadListFromXMLSerializer<Line>(LinePath);
            XElement RunningNumbersRoot = XMLTools.LoadListFromXMLElement(RunningNumbersPath);
            int id = Convert.ToInt32(RunningNumbersRoot.Element("IDS").Element("LineId").Value);
            RunningNumbersRoot.Element("IDS").Element("LineId").Value = (++id).ToString();
            line.Id = id;
            lines.Add(line);
            XMLTools.SaveListToXMLSerializer<Line>(lines, LinePath);
            XMLTools.SaveListToXMLElement(RunningNumbersRoot, RunningNumbersPath);
        }

        public void updateLine(Line line)  ////the function get Line variable and check if it`s  exist in the xml, if no throw excepation, else update it
        {
            List<DO.Line> lines = XMLTools.LoadListFromXMLSerializer<Line>(LinePath);
            Line lineToUpdate = lines.FirstOrDefault(x => x.Id == line.Id);
            if (lineToUpdate == null)
                throw new NotExistException("The Id  number not exist", line.Id);
            lines.Remove(lineToUpdate);
            lines.Add(line);
            XMLTools.SaveListToXMLSerializer<Line>(lines, LinePath);
        }

        public void deleteLine(Line line)  //the function get Line variable and check if it`s  exist in the xml, if no throw excepation, else delete it
        {
            List<DO.Line> lines = XMLTools.LoadListFromXMLSerializer<Line>(LinePath);
            Line lineToDelete = lines.FirstOrDefault(x => x.Id == line.Id);
            if (lineToDelete == null)
                throw new NotExistException("The Id  number not exist", line.Id);
            lines.Remove(lineToDelete);
            XMLTools.SaveListToXMLSerializer<Line>(lines, LinePath);
        }

        public Line getLineById(int id)  //the function get id and return the line that found
        {
            List<DO.Line> lines = XMLTools.LoadListFromXMLSerializer<Line>(LinePath);
            Line lineToReturn = (from line in lines
                                 where line.Id == id
                                 select line).FirstOrDefault();
            return lineToReturn;
        }
        #endregion Line

        #region User
        public User getAdmin()   //create a admin user and return it
        {
            User admin = new User { UserName = "Ranit", Password = "Bar", Admin = true };
            return admin;
        }
        #endregion User

        #region Station
        public void addStation(Station station)  //the function get Station variable and check if it`s already exist in the xml, if yes throw excepation, else add it
        {
            List<DO.Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationPath);
            if (stations.FirstOrDefault(x => x.Code == station.Code) != null)
            {
                XElement stationXml = new XElement("Station",
                new XElement("Code", station.Code),
                new XElement("Latitude", station.Location.Latitude),
                new XElement("Longitude", station.Location.Longitude),
                new XElement("Name", station.Name),
                new XElement("Address", station.Address)
                );
                StationRoot.Add(stationXml);
                StationRoot.Save(StationPath);
            }
            throw new AlreadyExistsException("The code number already exist", station.Code);
        }
        
        public IEnumerable<DO.Station> getAllStations() //the funcation return all the Station of all Lines
        {
            List<DO.Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationPath);

            return from station in stations
                   select station;
        }

        public void updateStation(Station station)  //the function get Station variable and check if it`s exist in the xml, if no throw excepation, else update it
        {
            List<DO.Station> stations = XMLTools.LoadListFromXMLSerializer<DO.Station>(StationPath);
            Station stationToUpdate = stations.FirstOrDefault(x => x.Code == station.Code);
            if (stationToUpdate == null)
                throw new NotExistException("The code number not exist", station.Code);
            stations.Remove(stationToUpdate);
            stations.Add(station);
            XMLTools.SaveListToXMLSerializer<Station>(stations, StationPath);
        }

        public void deleteStation(Station station)  //the function get Station variable and check if it`s  exist in the xml, if no throw excepation, else delete it
        {
            List<DO.Station> stations = XMLTools.LoadListFromXMLSerializer<DO.Station>(StationPath);
            Station stationToDelete = stations.FirstOrDefault(x => x.Code == station.Code);
            if (stationToDelete == null)
                throw new NotExistException("The code number not exist", station.Code);
            stations.Remove(stationToDelete);
            XMLTools.SaveListToXMLSerializer<Station>(stations, StationPath);

        }
        #endregion Station

        #region LineStation
        public void addLineStation(LineStation lineStation)   //the function get LineStation variable and check if it`s already exist in the xml, if yes throw excepation, else add it
        {
            XElement lineStationRoot = XMLTools.LoadListFromXMLElement(lineStationPath);

            XElement line1 = (from p in lineStationRoot.Elements()
                              where int.Parse(p.Element("LineId").Value) == lineStation.LineId
                              select p).FirstOrDefault();
            if (line1 != null)
                throw new AlreadyExistsException("This is already exists!", lineStation.LineId);

            LineStationRoot.Add(lineStation.LineStationToXML());
            LineStationRoot.Save(lineStationPath);
        } 

        public void updateLineStation(LineStation lineStation) //the function get LineStation variable and check if it`s exist in the xml, if no throw excepation, else update it
        {
            XElement lineStationRootElem = XMLTools.LoadListFromXMLElement(lineStationPath);
            XElement line1 = (from p in lineStationRootElem.Elements()
                              where int.Parse(p.Element("LineId").Value) == lineStation.LineId
                              select p).FirstOrDefault();
            if (line1 == null)
                throw new NotExistException("This id doesnt  exist!", lineStation.LineId);

            line1.Element("LineId").Value = lineStation.LineId.ToString();
            line1.Element("Station").Value = lineStation.Station.ToString();
            line1.Element("LineStationIndex").Value = lineStation.LineStationIndex.ToString();
            line1.Element("NextStation").Value = lineStation.NextStation.ToString();
            line1.Element("PrevStation").Value = lineStation.PrevStation.ToString();
            line1.Element("DistanceFromTheLastStat").Value = lineStation.DistanceFromTheLastStat.ToString();
            line1.Element("TravelTimeFromTheLastStation").Value = lineStation.TravelTimeFromTheLastStation.ToString();

            XMLTools.SaveListToXMLElement(lineStationRootElem, lineStationPath);
        }

        public IEnumerable<DO.LineStation> getAllLineStations() //the funcation return all the LineStation of all Lines
        {
            XElement lineStationRootElem = XMLTools.LoadListFromXMLElement(lineStationPath);
            List<DO.LineStation> lineStations = (from p in lineStationRootElem.Elements()
                                                 select new LineStation()
                                                 {
                                                     LineId = Convert.ToInt32(p.Element("LineId").Value),
                                                     LineStationIndex = Convert.ToInt32(p.Element("LineStationIndex").Value),
                                                     Station = Convert.ToInt32(p.Element("Station").Value),
                                                     DistanceFromTheLastStat = Convert.ToDouble(p.Element("DistanceFromTheLastStat").Value),
                                                     NextStation = Convert.ToInt32(p.Element("NextStation").Value),
                                                     TravelTimeFromTheLastStation = TimeSpan.Parse(p.Element("TravelTimeFromTheLastStation").Value.ToString()),
                                                     PrevStation = Convert.ToInt32(p.Element("PrevStation").Value)
                                                 }).ToList();

            return lineStations;
        }
        #endregion LineStation

        #region LineTrip
        public IEnumerable<DO.LineTrip> getAllLineTrips()  //the funcation return all the LineTrip of all Lines
        {
            XElement lineTripRootElem = XMLTools.LoadListFromXMLElement(LineTripPath);
            List<DO.LineTrip> lineTrips = (from p in lineTripRootElem.Elements()
                                           select new LineTrip()
                                           {
                                               LineId = Convert.ToInt32(p.Element("LineId").Value),
                                               Id = Convert.ToInt32(p.Element("Id").Value),
                                               StartAt = TimeSpan.Parse(p.Element("StartAt").Value.ToString()),
                                               FinishAt = TimeSpan.Parse(p.Element("FinishAt").Value.ToString()),
                                               Frequency = TimeSpan.Parse(p.Element("Frequency").Value.ToString()),
                                           }).ToList();

            return lineTrips;
        }
        #endregion LineTrip
    }
}
