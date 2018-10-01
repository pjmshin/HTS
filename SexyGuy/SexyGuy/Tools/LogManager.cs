using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SexyGuy.Tools
{
    public enum LogType { Daily, Monthly}

    public class LogManager
    {
        private string _Path;

        #region Constructors
        public LogManager()
            :this(Path.Combine(Application.Root, "Log"),LogType.Daily,null,null)
        {
        }

        public LogManager(string path,LogType logType, string prefix, string postfix) 
        {
            _Path = path;
            _SetLogPath(logType, prefix,postfix);

        }
        public LogManager(string prefix, string postfix)
           : this(Path.Combine(Application.Root, "Log"), LogType.Daily, prefix, postfix)
        {

        }
        #endregion

        #region Methods
        private void _SetLogPath(LogType logType,string prefix, string postfix)
        {
            string path = string.Empty;
            string name = string.Empty;

            switch (logType)
            {
                case LogType.Daily:

                    path =string.Format(@"{0}\{1}\", DateTime.Now.Year, DateTime.Now.ToString("MM"));
                    name = DateTime.Now.ToString("yyyyMMdd") ;
                    break;
                case LogType.Monthly:
                    path = string.Format(@"{0}\", DateTime.Now.Year);
                    name = DateTime.Now.ToString("yyyyMM") ;
                    break;

            }

            if (!string.IsNullOrEmpty(prefix))
            {
                name= prefix + name;
            }
            if (!string.IsNullOrEmpty(postfix))
            {
                name = name + postfix;
            }

            name += ".txt";

            _Path = Path.Combine(_Path, path);

            if (!Directory.Exists(_Path))
                Directory.CreateDirectory(_Path);

            _Path = Path.Combine(_Path, name);
            
        }


        public void Write(string data)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_Path, true))
                {
                    writer.Write(data);
                }
            }
            
            catch(Exception ex)
            { }
        }

        public void WriteLine(string data)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_Path, true))
                {
                    writer.WriteLine(DateTime.Now.ToString("yyyyMMdd HH:mm:ss\t") + data);

                }
            }
                       
            catch(Exception ex)
            { }
        }

        #endregion

    }
}
