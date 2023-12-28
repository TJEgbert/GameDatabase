using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameDatabase.Common
{
    public class clsErrorHandler
    {
        /// <summary>
        /// Adds the thrown exception to error_log text file 
        /// </summary>
        /// <param name="sClass"> The class that the exception occurred</param>
        /// <param name="sMethod">The method where the exception occurred</param>
        /// <param name="sMessage">The exception  that occurred</param>
        public void logError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                File.AppendAllText("error_log.txt", Environment.NewLine + "Class: " + sClass + " Method: " + sMethod + " Message: " + sMessage + DateTime.Now.ToString());
                MessageBox.Show("Please go to the github repository and follow the instructions from the 'Exception' section thank you", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            catch (Exception ex)
            {
                File.AppendAllText("logError_log.txt", Environment.NewLine + "Class: " + sClass + " Method: " + sMethod + " Message: " + ex + DateTime.Now.ToString());
                MessageBox.Show("Please go to the github repository and follow the instructions from the 'Exception' section thank you", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

    }
}


//              Top level methods
//try
//{
//    
//}
//catch (Exception ex)
//{
//    errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
//}

//              All other methods
//try
//{
//
//}
//catch (Exception ex)
//{
//    throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
//    MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
//}





