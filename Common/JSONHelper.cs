//https://stackoverflow.com/questions/53223517 by Brian Rogers
using System.IO;
using Newtonsoft.Json;

namespace MagicDye.Common;

public static class JsonHelper
{
    public static string SerializeWithCustomIndenting(object obj)
    {
        using (StringWriter sw = new StringWriter())
        using (JsonWriter jw = new CustomJsonTextWriter(sw))
        {
            jw.Formatting = Formatting.Indented;
            JsonSerializer ser = new JsonSerializer();
            ser.Serialize(jw, obj);
            return sw.ToString();
        }
    }
}

public class CustomJsonTextWriter : JsonTextWriter
{
    public CustomJsonTextWriter(TextWriter writer) : base(writer)
    {
    }

    protected override void WriteIndent()
    {
        if (WriteState != WriteState.Array)
            base.WriteIndent();
        else
            WriteIndentSpace();
    }
}