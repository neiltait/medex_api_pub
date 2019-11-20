using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    /// <summary>
    ///     Log message for before controller action has executed
    /// </summary>
    public class LogMessageActionDefault
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <param name="userAuthenticationType">user authentication type</param>
        /// <param name="userIsAuthenticated">user authentication status</param>
        /// <param name="controllerName">name of controller called</param>
        /// <param name="controllerMethod">method of caller called</param>
        /// <param name="parameters">list of parameters passed to method</param>
        /// <param name="remoteIP">IP address of client</param>
        /// <param name="timestamp">timestamp when method called</param>
        public LogMessageActionDefault(
            string userId,
            string userAuthenticationType,
            bool userIsAuthenticated,
            string controllerName,
            string controllerMethod,
            IDictionary<string, object> parameters,
            string remoteIP,
            DateTime timestamp,
            double totalRus)
        {
            UserId = userId;
            UserAuthenticationType = userAuthenticationType;
            UserIsAuthenticated = userIsAuthenticated;
            ControllerName = controllerName;
            ControllerMethod = controllerMethod;
            Parameters = parameters;
            RemoteIP = remoteIP;
            TimeStamp = timestamp;
            TotalRus = totalRus;
        }

        /// <summary>
        ///     name of user
        /// </summary>
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; }

        /// <summary>
        ///     authentication type of user
        /// </summary>
        [JsonProperty(PropertyName = "authentication_type")]
        public string UserAuthenticationType { get; }

        /// <summary>
        ///     user authentication status
        /// </summary>
        [JsonProperty(PropertyName = "is_authenticated")]
        public bool UserIsAuthenticated { get; }

        /// <summary>
        ///     name of controller
        /// </summary>
        [JsonProperty(PropertyName = "controller")]
        public string ControllerName { get; }

        /// <summary>
        ///     method called
        /// </summary>
        [JsonProperty(PropertyName = "action")]
        public string ControllerMethod { get; }

        /// <summary>
        ///     List of all parameters (as strings)
        /// </summary>
        [JsonProperty(PropertyName = "parameters")]
        public IDictionary<string, object> Parameters { get; }

        /// <summary>
        ///     IP of user's machine
        /// </summary>
        [JsonProperty(PropertyName = "remote_ip")]
        public string RemoteIP { get; }

        /// <summary>
        ///     Time of call
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
        [DataType(DataType.DateTime)]
        public DateTime TimeStamp { get; }

        /// <summary>
        /// Total RUs
        /// </summary>
        [JsonProperty(PropertyName = "total_rus")]
        public double TotalRus { get; }

        /// <summary>
        ///     ToString()
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            var contents = new StringBuilder();
            contents.Append(UserId + " " + UserAuthenticationType + " " + UserIsAuthenticated + " " + ControllerName +
                            " " + ControllerMethod + " ");

            foreach (var p in Parameters)
            {
                contents.Append(p + " ");
            }

            contents.Append(RemoteIP + " ");
            contents.Append(TimeStamp.ToLongDateString() + "_" + TimeStamp.ToLongTimeString());

            return contents.ToString();
        }
    }
}