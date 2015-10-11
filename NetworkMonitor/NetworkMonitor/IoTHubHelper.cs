using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace NetworkMonitor
{
    static class SharedAccessSignatureConstants
    {
        public const int MaxKeyNameLength = 256;
        public const int MaxKeyLength = 256;
        public const string SharedAccessSignature = "SharedAccessSignature";
        public const string AudienceFieldName = "sr";
        public const string SignatureFieldName = "sig";
        public const string KeyNameFieldName = "skn";
        public const string ExpiryFieldName = "se";
        public const string SignedResourceFullFieldName = SharedAccessSignature + " " + AudienceFieldName;
        public const string KeyValueSeparator = "=";
        public const string PairSeparator = "&";
        public static readonly DateTime EpochTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public static readonly TimeSpan MaxClockSkew = TimeSpan.FromMinutes(5);
    }

    class IoTHubHelper
    {
        public static string BuildSignature(string keyName, string key, string target, TimeSpan timeToLive)
         { 
             string expiresOn = BuildExpiresOn(timeToLive);
             string audience = WebUtility.UrlEncode(target); 
             List<string> fields = new List<string>(); 
             fields.Add(audience); 
             fields.Add(expiresOn); 
 
             string signature = Sign(string.Join("\n", fields), key); 
 
 
             var buffer = new StringBuilder(); 
             buffer.AppendFormat(CultureInfo.InvariantCulture, "{0} {1}={2}&{3}={4}&{5}={6}", 
                 SharedAccessSignatureConstants.SharedAccessSignature, 
                 SharedAccessSignatureConstants.AudienceFieldName, audience, 
                 SharedAccessSignatureConstants.SignatureFieldName, WebUtility.UrlEncode(signature), 
                 SharedAccessSignatureConstants.ExpiryFieldName, WebUtility.UrlEncode(expiresOn)); 
              
             if (!string.IsNullOrEmpty(keyName)) 
             { 
                 buffer.AppendFormat(CultureInfo.InvariantCulture, "&{0}={1}", 
                     SharedAccessSignatureConstants.KeyNameFieldName, WebUtility.UrlEncode(keyName)); 
             }

            return buffer.ToString(); 
         } 
 
 
         static string BuildExpiresOn(TimeSpan timeToLive)
         { 
             DateTime expiresOn = DateTime.UtcNow.Add(timeToLive); 
             TimeSpan secondsFromBaseTime = expiresOn.Subtract(SharedAccessSignatureConstants.EpochTime); 
             long seconds = Convert.ToInt64(secondsFromBaseTime.TotalSeconds, CultureInfo.InvariantCulture); 
             return Convert.ToString(seconds, CultureInfo.InvariantCulture); 
         }

        public static string Sign(string requestString, string key)
        {
            var algo = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha256);
            var keyMaterial = Convert.FromBase64String(key).AsBuffer();
            var hash = algo.CreateHash(keyMaterial);
            hash.Append(CryptographicBuffer.ConvertStringToBinary(requestString, BinaryStringEncoding.Utf8));


            var sign = CryptographicBuffer.EncodeToBase64String(hash.GetValueAndReset());
            return sign;
        }


}
}
