using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    public class LoggerHelper
    {
        // NOTE: this is specific to dotnet 4.6 and above. 

        /// <summary>
        /// this filename is an optional parameter and if we dont add something to the filename then by
        /// default, the CallerFilePath attribute takes over and gives us the path to the file that is calling this method. 
        /// Remember that the path given is a full path
        /// </summary>
        //public static log4net.ILog.GetLogger([CallerFilePathAttribute]string filename = "")
        //{
        //    // every time we call this method, we instantiate a new logger with this filename. 
        //    return log4net.LogManager.GetLogger(filename);
        //}

    }
}
