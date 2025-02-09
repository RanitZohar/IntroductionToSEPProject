﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_1853_9327
{
    public class BusStation
    {
        private const int MAXDIGITS = 1000000;
        private const int MIN_LAT = -90;
        private const int MAX_LAT = 90;
        private const int MIN_LON = -180;
        private const int MAX_LON = 180;

      

        private int busStationKey;
        private double latitude; 
        private double longitude;
        public String Address { get; set; } = "";


        public BusStation(int key, double lat, double longi)
        {
            busStationKey = key;
            latitude = lat;
            longitude = longi;



        }



     
       
        public int BusStationKey
        {
            get { return busStationKey; }

            set
            {
                
                if (value <= 0 && value >= MAXDIGITS)
                {
                    throw new ArgumentException(
                       String.Format("{0} is not a valid key number", value));
                }
                busStationKey = value;
               
            }
        }

        public double Latitude
        {
            get { return latitude; }
            set
            {
                if (value >= MIN_LAT && value <= MAX_LAT)
                {
                    latitude = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Latitude",
                        String.Format("{0} should be between {1} and {2}", value, MIN_LAT, MAX_LAT));
                }
            }
        }

        public double Longitude
        {
            get { return longitude; }
            set
            {
                if (value >= MIN_LON && value <= MAX_LON)
                {
                    longitude = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Longitude",
                        String.Format("{0} should be between {1} and {2}", value, MIN_LON, MAX_LON));
                }
            }

        }



        public override string ToString()
        {
            String result = "BusStation Code: " + busStationKey;
            result += String.Format(", {0}°{1} {2}°{3}",
                Math.Abs(Latitude), (Latitude > 0) ? "N" : "S",
                Math.Abs(Longitude), (Longitude > 0) ? "E" : "W");
            return result;
        }
    }

}
