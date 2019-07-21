using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.IO;

namespace archives.common
{
    public class ApplicationLog
    {
        public static ILoggerRepository Repository = LogManager.CreateRepository("AppLoggerRepository");
        private static ILog AppLog = LogManager.GetLogger(Repository.Name, "AppLogger");

        #region 公共方法

        #region Init(初始化)

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            if (Repository == null)
                Repository = LogManager.CreateRepository("AppLoggerRepository");

            if (AppLog == null)
                AppLog = LogManager.GetLogger(Repository.Name, "AppLogger");

            var file = new FileInfo("log4net.config");

            if (!file.Exists)
            {
                throw new FileNotFoundException("日志组件初始化失败，未找到 log4net.config 文件。");
            }

            XmlConfigurator.Configure(Repository, file);

            Info("************ Logging Start ************");
        }

        #endregion

        #region Info(记录运行信息)

        /// <summary>
        /// 记录运行信息
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            AppLog.Info(message);
        }

        #endregion

        #region Debug(记录调试信息)

        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            AppLog.Debug(message);
        }

        #endregion

        #region Error(记录错误信息)

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="message">错误信息</param>
        public static void Error(string message)
        {
            Error(message, null);
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="ex">异常堆栈</param>
        public static void Error(string message, Exception ex)
        {
            AppLog.Error(message, ex);
        }

        #endregion

        #region Fatal(记录致命错误信息)

        /// <summary>
        /// 记录致命错误信息
        /// </summary>
        /// <param name="message">错误信息</param>
        public static void Fatal(string message)
        {
            Fatal(message, null);
        }

        /// <summary>
        /// 记录致命错误信息
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="ex">异常堆栈</param>
        public static void Fatal(string message, Exception ex)
        {
            AppLog.Fatal(message, ex);
        }

        #endregion

        #region Warn(记录警告信息)

        /// <summary>
        /// 记录致命错误信息
        /// </summary>
        /// <param name="message">错误信息</param>
        public static void Warn(string message)
        {
            Warn(message, null);
        }

        /// <summary>
        /// 记录致命错误信息
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="ex">异常堆栈</param>
        public static void Warn(string message, Exception ex)
        {
            AppLog.Warn(message, ex);
        }

        #endregion

        #endregion
    }
}
