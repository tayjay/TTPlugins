using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Loader;

namespace TTCore.Reflection;

public class OptionalSCriPt : OptionalPlugin
{
    
    OptionalReference Connector { get; set; }
    
    public OptionalSCriPt() : base(Loader.GetPlugin("SCriPt"))
    {
        if(IsPresent)
            SetupConnector(Plugin);
    }

    private void SetupConnector(IPlugin<IConfig> plugin)
    {
        if(Plugin == null) return;
        if(Connector != null) return;
        if (Create(CallMethod("SetupConnector", plugin), out OptionalReference connector))
        {
            Connector = connector;
            return;
        }

        return;

    }
    
    public void AddGlobal(Type type, string name)
    {
        if (!IsPresent)
        {
            Log.Info("SCriPt is not present! Skipping AddGlobal for "+name);
            return;
        }
        Connector.CallMethod("AddGlobal", type, name);
    }

    public double GetStoredNumber(string table, string key)
    {
        if (!IsPresent) throw new NullReferenceException();
        return Connector.CallMethod("GetStoredNumber", table, key) as double? ?? 0;
    }
    
    public string GetStoredString(string table, string key)
    {
        if (!IsPresent) throw new NullReferenceException();
        return Connector.CallMethod("GetStoredString", table, key) as string;
    }
    
    public bool GetStoredBool(string table, string key)
    {
        if (!IsPresent) throw new NullReferenceException();
        return Connector.CallMethod("GetStoredBool", table, key) as bool? ?? false;
    }
    
    public Dictionary<string, object> GetStoredTable(string table, string key)
    {
        if (!IsPresent) throw new NullReferenceException();
        return Connector.CallMethod("GetStoredTable", table, key) as Dictionary<string, object>;
    }
    
    public void SetStoredNumber(string table, string key, double value)
    {
        if (!IsPresent) throw new NullReferenceException();
        Connector.CallMethod("SetStoredNumber", table, key, value);
    }
    
    public void SetStoredString(string table, string key, string value)
    {
        if (!IsPresent) throw new NullReferenceException();
        Connector.CallMethod("SetStoredString", table, key, value);
    }
    
    public void SetStoredBool(string table, string key, bool value)
    {
        if (!IsPresent) throw new NullReferenceException();
        Connector.CallMethod("SetStoredBool", table, key, value);
    }
    
    public void SetStoredTable(string table, string key, Dictionary<string, object> value)
    {
        if (!IsPresent) throw new NullReferenceException();
        Connector.CallMethod("SetStoredTable", table, key, value);
    }
    
    public int GetConfigInt(string key)
    {
        if (!IsPresent) throw new NullReferenceException();
        return Connector.CallMethod("GetConfigInt", key) as int? ?? 0;
    }
    
    public float GetConfigFloat(string key)
    {
        if (!IsPresent) throw new NullReferenceException();
        return Connector.CallMethod("GetConfigFloat", key) as float? ?? 0;
    }
    
    public string GetConfigString(string key)
    {
        if (!IsPresent) throw new NullReferenceException();
        return Connector.CallMethod("GetConfigString", key) as string;
    }
    
    public bool GetConfigBool(string key)
    {
        if (!IsPresent) throw new NullReferenceException();
        return Connector.CallMethod("GetConfigBool", key) as bool? ?? false;
    }
}