using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JFPGeneric
{
    public class Response
    {
        public enum Severity { None = 0, Information = 1, Warning = 2, Error = 3 };

        public Response(Severity severity, String message)
        {
            MsgSeverity = severity;
            Message = message;
        }

        public static Response Uninitialized
        {
            get { return new Response(Severity.Information, "Uninitialized"); }
        }

        public Severity MsgSeverity { get; private set; }

        public String Message { get; private set; }
    }
}
