using Newtonsoft.Json;

public class ObjectConversionUtil
{

    public static string convertObjectIntoJson<T>(T objToConvert)
    {
        // Convert any object to JSON
        return JsonConvert.SerializeObject(objToConvert, Formatting.Indented);        
    }

}
