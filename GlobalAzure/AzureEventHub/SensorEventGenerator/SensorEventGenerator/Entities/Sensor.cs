//********************************************************* 
// 
//    Copyright (c) Microsoft. All rights reserved. 
//    This code is licensed under the Microsoft Public License. 
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF 
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY 
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR 
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT. 
// 
//********************************************************* 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorEventGenerator
{
    public class Sensor
    {
        public string time;
        public int dsplid;
        public string dspl;
        public int temp;
        public int hmdt;
        public string status;
        public string location;

        static Random R = new Random();
        static string sensorNamesPrefix = "sensor";
        static int sensorTotalNumber = 1000;
        
        public static Sensor Generate()
        {
            int sensorNumber = R.Next(sensorTotalNumber);
            double error = (sensorNumber % 100.0) == 0 ? 1.9 : 1;

            return new Sensor { time = DateTime.UtcNow.AddHours(1).ToString("o"),
                dsplid = sensorNumber,
                dspl = sensorNamesPrefix + sensorNumber.ToString(),
                temp = (int)Math.Round(R.Next(20, 25) * error, 0),
                hmdt = (int)Math.Round(R.Next(30, 70) * error, 0),
                status = R.Next(250) == 1 ? " O." : "OK",
                location = 
                    "Building " + 
                    Math.Ceiling(sensorNumber / 24.0).ToString() + 
                    ", " + 
                    "Floor " + 
                    (R.Next(10) == 1 ? "" : Math.Ceiling(sensorNumber / 4.0).ToString())
            };
        }
    }
}
