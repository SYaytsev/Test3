using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp3
{
    /// <summary>
    /// Logger message level
    /// </summary>
    enum LogLevel { Info, Debug, Warning, Error }
    /// <summary>
    /// Logger record strategy
    /// </summary>
    enum Strategy { RecordToConsole, RecordToFile }
    /// <summary>
    /// Logger - класс, который имеет методы Info, Debug, Warning, Error. 
    /// Logger иметь возможность писать в разные источники (Console, File, etc). 
    /// Источник зависит от выбраной стратегии записи (recordStrategy).
    /// </summary>
    class Logger : IDisposable
    {
        private static Logger uniqueInstance;
        private static IStrategy recordStrategy;

        private Logger() { }
        public static Logger Instance(Strategy typeStrategy)
        {
            if (uniqueInstance == null)
            {
                uniqueInstance = new Logger();
                switch (typeStrategy)
                {
                    case Strategy.RecordToConsole:
                        recordStrategy = new RecordToConsole();
                        break;
                    case Strategy.RecordToFile:
                        recordStrategy = new RecordToFile();
                        break;
                }
            }
            return uniqueInstance;
        }

        public void Dispose()
        {
            recordStrategy.Dispose();
        }

        public void Info(string in_text)
        {
            recordStrategy.Write(DateTime.Now.ToString() + " " + LogLevel.Info + ": " + in_text);
        }

        public void Debug(string in_text)
        {
            recordStrategy.Write(DateTime.Now.ToString() + " " + LogLevel.Debug + ": " + in_text);
        }

        public void Warning(string in_text)
        {
            recordStrategy.Write(DateTime.Now.ToString() + " " + LogLevel.Warning + ": " + in_text);
        }

        public void Error(string in_text)
        {
            recordStrategy.Write(DateTime.Now.ToString() + " " + LogLevel.Error + ": " + in_text);
        }
    }


    interface IStrategy : IDisposable
    {
        void Write(string logText);
    }

    class RecordToConsole : IStrategy
    {
        public void Write(string logText)
        {
            Console.WriteLine(logText);
        }
        public void Dispose()
        {
            Console.Clear();
        }
    }

    class RecordToFile : IStrategy
    {
        private StreamWriter sw;

        public RecordToFile()
        {
            string currentDate = DateTime.Now.Date.ToShortDateString();
            string path = "LogFile_" + currentDate + ".txt";

            if (!File.Exists(path))
            {
                sw = File.CreateText(path);
                sw.WriteLine("Log File " + Environment.NewLine);
            }
            else
            {
                sw = File.AppendText(path);
            }
        }
        public void Write(string logText)
        {
            sw.WriteLine(logText);
        }
        public void Dispose()
        {
            sw.WriteLine("End log session");
            sw.Flush();
            sw.Close();
        }
    }
}
