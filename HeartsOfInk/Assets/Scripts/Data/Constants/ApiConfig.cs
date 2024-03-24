﻿namespace Assets.Scripts.Data.Constants
{
    class ApiConfig
    {
        // Local
        public const string LobbyHOIServerUrl = "http://localhost:5000/";
        public const string IngameServerUrl = "http://localhost:7001/";
        public const string LoggingServerUrl = "https://localhost:44356/";

        public const string SignalRHUBName = "signalrhoi";

        /// <summary>
        /// Delay in miliseconds between calls to server for update states.
        /// </summary>
        public const int DelayBetweenStateUpdates = 50;
    }
}