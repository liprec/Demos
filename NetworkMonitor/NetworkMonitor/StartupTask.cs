using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Microsoft.Azure.Devices.Client;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using Windows.ApplicationModel.Resources;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace NetworkMonitor
{
    public sealed class StartupTask : IBackgroundTask
    {
        private bool IsRunning = true;
        private CancellationTokenSource CancelToken;
        private string[] WebSites = { "http://www.microsoft.com", "http://www.google.com", "http://www.twitter.com", "http://www.facebook.com" };

        #region Secure
        private string ConnectionString = "<YourIotDeviceConnectiontring>";
        #endregion

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            taskInstance.Canceled += TaskInstance_Canceled;
            CancelToken = new CancellationTokenSource();

            while (IsRunning)
            {
                DateTimeOffset StartTime = DateTimeOffset.Now;
                List<WebSites> WebSiteResponses = new List<NetworkMonitor.WebSites>();

                foreach (string webUrl in WebSites)
                {
                    // Setup HttpClient for the website call
                    HttpClient WebCall = new HttpClient();
                    WebCall.BaseAddress = new Uri(webUrl);

                    DateTimeOffset startResuestTime = DateTimeOffset.Now;
                    HttpResponseMessage response = WebCall.GetAsync(WebCall.BaseAddress).Result;

                    //Construct WebSite response class
                    WebSites WebSiteResponse = new WebSites();
                    WebSiteResponse.Url = WebCall.BaseAddress.ToString();
                    WebSiteResponse.Response = ((int)response.StatusCode).ToString();
                    WebSiteResponse.RequestDateTime = StartTime.DateTime;
                    WebSiteResponse.ResponseTime = (int)(DateTimeOffset.Now.Subtract(startResuestTime).Ticks / 1000);

                    WebSiteResponses.Add(WebSiteResponse);
                }

                SendMessage(WebSiteResponses);
                // Wait 1 minute (= 60.000ms)
                Task.Delay(60000).Wait(CancelToken.Token);
            }
        }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            //BackgroundTask is canceled, so cancel all running tasks/loops
            CancelToken.Cancel();
            IsRunning = false;
        }

        private async Task SendMessage(List<WebSites> WebSiteResponses)
        {
            string JsonMessage = JsonConvert.SerializeObject(WebSiteResponses);

            IotHubConnectionStringBuilder IotConString = IotHubConnectionStringBuilder.Create(ConnectionString);

            EventHubClient IoTClient = new EventHubClient(IotConString.HostName, EventHubType.IoTHub);
            string Token = IoTClient.GetSasToken(IotConString.SharedAccessKeyName, IotConString.SharedAccessKey, 60, IotConString.DeviceId);
            var Response = await IoTClient.SendMessage(JsonMessage, Token, IotConString.DeviceId);
         }
    }
}
