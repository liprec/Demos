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
using Newtonsoft.Json;
using System.Configuration;
using Microsoft.Azure.EventHubs;

namespace SensorEventGenerator
{
    class EventHubObserver : IObserver<Sensor>
    {
        private EventHubConfig _config;
        private EventHubClient _eventHubClient;
        private Logger _logger;
                
        public EventHubObserver(EventHubConfig config)
        {
            try
            {
                _config = config;
                var _connectionString = new EventHubsConnectionStringBuilder(_config.ConnectionString)
                {
                    EntityPath = config.EventHubName
                };
                _eventHubClient = EventHubClient.CreateFromConnectionString(_connectionString.ToString());

                this._logger = new Logger(ConfigurationManager.AppSettings["logger_path"]);
            }
            catch (Exception ex)
            {
                _logger.Write(ex);
                throw ex;
            }

        }
        public void OnNext(Sensor sensorData)
        {
            try
            {
                var serialisedString = JsonConvert.SerializeObject(sensorData); 
                EventData data = new EventData(Encoding.UTF8.GetBytes(serialisedString));

                _eventHubClient.SendAsync(data);

#if DEBUG
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Sending " + serialisedString + " at: " + sensorData.time);
#endif
            }
            catch (Exception ex)
            {
                _logger.Write(ex);
                throw ex;
            }

        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            _logger.Write(error);
            throw error;
        }

    }
}
