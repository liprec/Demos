using Microsoft.Analytics.Interfaces;
using Microsoft.Analytics.Types.Sql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Functions
{
    public class Location
    {
        public static string GetBuilding(string location)
        {
            return location.Split(',')[0];
        }
        public static string GetFloor(string location)
        {
            return location.Split(',')[1];
        }
    }
}
