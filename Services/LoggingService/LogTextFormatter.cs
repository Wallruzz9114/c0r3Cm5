using System.IO;
using Serilog.Events;
using Serilog.Formatting;

namespace LoggingService
{
    public class LogTextFormatter : ITextFormatter
    {
        public void Format(LogEvent logEvent, TextWriter textWriter)
        {
            if (logEvent.Level.ToString() != "Information")
            {
                textWriter.WriteLine("---------------------------------------------------------------------------");
                textWriter.WriteLine($"Timestamp - { logEvent.Timestamp } | Level - { logEvent.Level }               |");
                textWriter.WriteLine("---------------------------------------------------------------------------");

                foreach (var property in logEvent.Properties)
                    textWriter.WriteLine(property.Key + " : " + property.Value);

                if (logEvent.Exception != null)
                {
                    textWriter.WriteLine("--------------------------- EXCEPTION DETAILS ------------------------------");
                    textWriter.Write("Exception - {0}", logEvent.Exception);
                    textWriter.Write("StackTrace - {0}", logEvent.Exception.StackTrace);
                    textWriter.Write("Message - {0}", logEvent.Exception.Message);
                    textWriter.Write("Source - {0}", logEvent.Exception.Source);
                    textWriter.Write("InnerException -{0}", logEvent.Exception.InnerException);
                }

                textWriter.WriteLine("---------------------------------------------------------------------------\n");
            }
        }
    }
}