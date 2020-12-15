using System;
using System.Collections.Generic;
using System.IO;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Json;

namespace WivCore.Utils.Logging
{
    public class RenderedJsonFormatter : ITextFormatter
    {
        private readonly JsonValueFormatter _valueFormatter;
        
        public RenderedJsonFormatter(JsonValueFormatter valueFormatter = null)
        {
            _valueFormatter = valueFormatter ?? new JsonValueFormatter("$type");
        } 
        
        public void Format(LogEvent logEvent, TextWriter output)
        {
            if (logEvent == null)
                throw new ArgumentNullException(nameof (logEvent));
            if (output == null)
                throw new ArgumentNullException(nameof (output));
            
            FormatEvent(logEvent, output, _valueFormatter);
            
            output.WriteLine();
        }

        private static void FormatEvent(LogEvent logEvent, TextWriter output, JsonValueFormatter valueFormatter)
        {
            output.Write("{\"time\":\"");
            output.Write(logEvent.Timestamp.DateTime.ToString("O"));
            output.Write("\",\"msg\":");
            JsonValueFormatter.WriteQuotedJsonString(logEvent.MessageTemplate.Render(logEvent.Properties), output);
          
            output.Write(",\"level\":\"");
            output.Write((object) logEvent.Level);
            output.Write('"');
          
            if (logEvent.Exception != null)
            {
                output.Write(",\"ex\":");
                JsonValueFormatter.WriteQuotedJsonString(logEvent.Exception.ToString(), output);
            }
            
            foreach (KeyValuePair<string, LogEventPropertyValue> property in logEvent.Properties)
            {
                string str = property.Key;
                if (str.Length > 0 && str[0] == '@')
                  str = "@" + str;
                output.Write(',');
                JsonValueFormatter.WriteQuotedJsonString(str, output);
                output.Write(':');
                valueFormatter.Format(property.Value, output); 
            }
            output.Write('}');
        }
    }
}