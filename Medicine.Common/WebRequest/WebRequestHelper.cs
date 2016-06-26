using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Medicine.Common
{
    /// <summary>
    /// This class is used for web request helper
    /// </summary>
    public class WebRequestHelper
    {
        private readonly WebRequest request;
        private Stream dataStream;

        /// <summary>
        /// The request status
        /// </summary>
        public String Status
        {
            get; set;
        }

        /// <summary>
        /// Overloaded Constructor
        /// </summary>
        /// <param name="url"></param>
        public WebRequestHelper(string url)
        {
            // Create a request using a URL that can receive a post.
            request = WebRequest.Create(url);
        }

        /// <summary>
        /// Overloaded Constructor
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        public WebRequestHelper(string url, string method)
            : this(url)
        {

            if (method.Equals("GET") || method.Equals("POST"))
            {
                // Set the Method property of the request to POST.
                request.Method = method;
            }
            else
            {
                throw new Exception("Invalid Method Type");
            }
        }

        /// <summary>
        /// Overloaded Constructor
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        public WebRequestHelper(string url, string method, string data)
            : this(url, method)
        {

            // Create POST data and convert it to a byte array.
            string postData = data;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";

            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;

            // Get the request stream.
            dataStream = request.GetRequestStream();

            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);

            // Close the Stream object.
            dataStream.Close();

        }

        /// <summary>
        /// Get response
        /// </summary>
        /// <returns></returns>
        public string GetResponse()
        {
            string responseFromServer = string.Empty;
            try
            {
                // Get the original response.
                WebResponse response = request.GetResponse();

                this.Status = ((HttpWebResponse)response).StatusDescription;

                // Get the stream containing all content returned by the requested server.
                dataStream = response.GetResponseStream();

                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);

                // Read the content fully up to the end.
                responseFromServer = reader.ReadToEnd();

                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("WebRequestHelper", "GetResponse", ex, request.RequestUri.ToString());
            }

            return responseFromServer;
        }
    }
}