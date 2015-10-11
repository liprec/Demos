using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Newtonsoft.Json;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;
using Windows.Foundation;
using System.Net;

namespace NetworkMonitor
{
    /// <summary>
    /// Event Hub token type
    /// </summary>
    public enum EventHubType
    {
        /// <summary>
        /// Default Azure Event Hub
        /// </summary>
        Default = 0,
        /// <summary>
        /// Azure IoT Hub
        /// </summary>
        IoTHub = 1
    }

    public sealed class EventHubClient
    {
        const string SASAuthorization = "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}";
        const string IoTHubApiVersion = "api-version=2015-08-15-preview";
        const string MessagePart = "messages";

        /// <summary>
        /// Constructor for the EventHubClient
        /// </summary>
        /// <param name="HostName">Full URI of host name and Namespace</param>
        /// <param name="EventHubType">Type of Event Hub</param>
        public EventHubClient(string HostName, EventHubType EventHubType)
        {

            this.BaseAddress = HostName;
            this.EventHubType = EventHubType;
        }

        /// <summary>
        /// Constructor for the EventHubClient
        /// </summary>
        /// <param name="ServiceBusNamespace">Namespace of the EventHub</param>
        /// <param name="ServiceBusHostName">Service name of the Event Hub</param>
        /// <param name="EventHubType">Type of Event Hub</param>
        public EventHubClient(string ServiceBusNamespace, string ServiceBusHostName, EventHubType EventHubType)
        {
            this.BaseAddress = string.Format("{0}.{1}", ServiceBusNamespace, ServiceBusHostName);
            this.EventHubType = EventHubType;
        }
        /// <summary>
        /// Request an access token based on the provided Shared Access Key information for the given
        /// Azure Event Hub
        /// </summary>
        /// <param name="SasKeyName">Sas key name of the Azure Event Hub</param>
        /// <param name="SasKey">Sas key of the Azure Event Hub</param>
        /// <param name="duration">Expiration time in seconds of the requested token</param>
        /// <param name="EventHubName">Name of the Azure Event hub</param>
        /// <returns></returns>
        public string GetSasToken(string SasKeyName, string SasKey, uint duration, string EventHubName)
        {
            if (!String.IsNullOrWhiteSpace(EventHubName))
            {
                this.EventHubName = EventHubName;
            }
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = DateTime.Now.ToUniversalTime() - origin;
            uint tokenExpirationTime = Convert.ToUInt32(diff.TotalSeconds) + duration * 60;

            string stringToSign = Uri.EscapeDataString(FullUri) + "\n" + tokenExpirationTime;

            string signature = HmacSha256(SasKey, stringToSign);
            string token = String.Format(CultureInfo.InvariantCulture, SASAuthorization,
                Uri.EscapeDataString(FullUri), Uri.EscapeDataString(signature), tokenExpirationTime, SasKeyName);
            return token;
        }

        /// <summary>
        /// Method to send a text message to the given Azure Event Hub
        /// </summary>
        /// <param name="Body">Body of the message</param>
        /// <param name="Token">Valid access token</param>
        /// <param name="EventHubName">Name of the Azure Event hub. Use null for default.</param>
        /// <returns>HttpResponseMessage message</returns>
        public IAsyncOperation<int> SendMessage(string Body, string Token, string EventHubName)
        {
            return SendMessageAsync(Body, Token, EventHubName).AsAsyncOperation();
        }

        /// <summary>
        /// Private async method of SendMessage
        /// </summary>
        private async Task<int> SendMessageAsync(string Body, string Token, string EventHubName)
        {
            if (!String.IsNullOrWhiteSpace(EventHubName))
            {
                this.EventHubName = EventHubName;
            }
            HttpClient webClient = new HttpClient();
            webClient.BaseAddress = new Uri(BaseAddress);
            webClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", Token);

            StringContent content = new StringContent(Body);

            HttpResponseMessage ResponseMsg = await webClient.PostAsync(RelativeUri, content);

            return (int)ResponseMsg.StatusCode;
        }

        /// <summary>
        /// Create a HmacSha256 encoded string. Windows IoT/WP is missing the default methods (System.Security.Cryptography.HMACSHA256)
        /// </summary>
        /// <param name="SecretKey">The secret key used to encrypt the provided string</param>
        /// <param name="Value">The value to be encrypted</param>
        /// <returns></returns>
        private string HmacSha256(string SecretKey, string Value)
        {
            // Move strings to buffers.
            // IoT SAS keys needs a different key convertion (??)
            IBuffer key = null;
            switch (EventHubType)
            {
                case EventHubType.IoTHub:
                    key = Convert.FromBase64String(SecretKey).AsBuffer();
                    break;
                default:
                    key = CryptographicBuffer.ConvertStringToBinary(SecretKey, BinaryStringEncoding.Utf8);
                    break;
            }            
            var msg = CryptographicBuffer.ConvertStringToBinary(Value, BinaryStringEncoding.Utf8);

            // Create HMAC.
            var objMacProv = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha256);
            var hash = objMacProv.CreateHash(key);
            hash.Append(msg);
            return CryptographicBuffer.EncodeToBase64String(hash.GetValueAndReset());
        }

        #region Properties
        //private string m_HostName;
        ///// <summary>
        ///// Event Hub Service Bus host name
        ///// </summary>
        //public string HostName
        //{
        //    get { return m_HostName; }
        //    set { m_HostName = value; }
        //}

        //private string m_Namespace;
        ///// <summary>
        ///// Event Hub Sevice Bus namespace
        ///// </summary>
        //public string Namespace
        //{
        //    get { return m_Namespace; }
        //    set { m_Namespace = value; }
        //}

        private EventHubType m_EventHubType = EventHubType.Default;
        /// <summary>
        /// Type of Event Hub
        /// </summary>
        public EventHubType EventHubType
        {
            get { return m_EventHubType; }
            set { m_EventHubType = value; }
        }

        private string m_EventHubName = "";
        /// <summary>
        /// Name of the Event Hub
        /// For IoT this is the DeviceID
        /// </summary>
        public string EventHubName
        {
            get { return m_EventHubName; }
            set { m_EventHubName = value; }
        }

        private string m_BaseAddess;
        /// <summary>
        /// Base address of the Azure endpoint
        /// </summary>
        public string BaseAddress
        {
            get { return string.Format("https://{0}", m_BaseAddess); }
            set { m_BaseAddess = value; }
        }

        /// <summary>
        /// Full URI of the web request
        /// </summary>
        public string FullUri
        {
            get {
                switch (EventHubType)
                {
                    case EventHubType.IoTHub:
                        // IoTHub requires a FullUri without https
                        return string.Format("{0}/devices/{1}", BaseAddress, EventHubName).Replace("https://", "");
                    default:
                        return string.Format("{0}/{1}/{2}", BaseAddress, EventHubName, MessagePart);
                }
            }
        }

        /// <summary>
        /// Reletive URI of the web request
        /// </summary>
        public string RelativeUri {
            get
            {
                switch (EventHubType)
                {
                    case EventHubType.IoTHub:
                        return string.Format("/devices/{0}/{1}/events?{2}", EventHubName, MessagePart, IoTHubApiVersion);
                    default:
                        return string.Format("/{0}/{1}", EventHubName, MessagePart);
                }
            }
        }
        #endregion
    }
}