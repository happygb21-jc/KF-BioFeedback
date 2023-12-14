﻿using NLog;

namespace Kingfar.BioFeedback.Shared.Abstractions.Common
{
    public static class AppLogs
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public static ILogger GetLogger() => logger;

        public static void Error(Exception ex) => logger.Error(ex);

        public static void Error(string message) => logger.Error(message);

        public static void Info(string message) => logger.Info(message);

        public static void Debug(string message) => logger.Debug(message);

        public static void Trace(Exception ex) => logger.Trace(ex);
    }
}