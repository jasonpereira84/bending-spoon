using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Collections;

namespace JFPGeneric
{
    public partial class Functions
    {
        public static Result CreateTemporaryFile_WinForm(out String theFileName)
        {
            Result retVal;
            try
            {
                theFileName = string.Empty;

                // Get the full name of the newly created Temporary file. 
                // Note that the GetTempFileName() method actually creates
                // a 0-byte file and returns the name of the created file.
                string fileName = Path.GetTempFileName();

                // Create a FileInfo object to set the file's attributes
                FileInfo fileInfo = new FileInfo(fileName);

                // Set the Attribute property of this file to Temporary. 
                // Although this is not completely necessary, the .NET Framework is able 
                // to optimize the use of Temporary files by keeping them cached in memory.
                fileInfo.Attributes = FileAttributes.Temporary;

                theFileName = fileName;
                retVal = new Result(true);
            }
            catch (Exception x)
            {
                theFileName = string.Empty;
                retVal = new Result("Unable to create TEMP file or set its attributes: " + x.Message);
            }
            return retVal;
        }
    }
}
