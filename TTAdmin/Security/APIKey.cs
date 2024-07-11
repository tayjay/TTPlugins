using System;
using System.IO;
using Exiled.API.Features;
using Utf8Json;

namespace TTAdmin.Security;

public class APIKey
{
    public string KeyString { get; set; }
    public DateTime Created { get; set; }
    
    public APIKey()
    {
        KeyString = Guid.NewGuid().ToString();
        Created = DateTime.Now;
    }

    public static APIKey FromFile(string path)
    {
        if(!Directory.Exists("TTCore"))
        {
            Directory.CreateDirectory("TTCore");
        }
        if(!Directory.Exists("TTCore/TTAdmin"))
        {
            Directory.CreateDirectory("TTCore/TTAdmin");
        }
        if (!File.Exists(path))
        {
            File.WriteAllText(path,JsonSerializer.ToJsonString<APIKey>(new APIKey()));
        }
        APIKey key = JsonSerializer.Deserialize<APIKey>(File.ReadAllText(path));
        if(!Guid.TryParse(key.KeyString,out _))
            Log.Error("API Key is not a valid GUID, may not be secure. Please generate a new one.");
        return key;
    }
}