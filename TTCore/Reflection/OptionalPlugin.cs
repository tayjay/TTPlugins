using System.Reflection;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Loader;
using Log = PluginAPI.Core.Log;

namespace TTCore.Reflection;

public class OptionalPlugin : OptionalReference
{
    
    public IPlugin<IConfig> Plugin { get; set; }
    public string Name => Plugin.Name;
    public Assembly Assembly => Plugin.Assembly;

    protected OptionalPlugin(IPlugin<IConfig> plugin) : base(plugin)
    {
        Plugin = plugin;
    }

    public override object CallMethod(string methodName, params object[] args)
    {
        if(Plugin == null) return null;
        try
        {
            var method = Plugin.GetType().GetMethod(methodName);
            if (method?.ReturnType != typeof(void))
            {
                return method?.Invoke(Plugin, args);
            }
            else
            {
                method?.Invoke(Plugin, args);
                return null;
            }
            
        } catch (System.Exception e)
        {
            Log.Error("Failed to call method: "+methodName + " on plugin: "+Name);
            Log.Error(e.Message);
        }
        return null;
    }

    /// <summary>
    /// Gets or sets the value of a field in the Plugin object using reflection.
    /// </summary>
    /// <param name="fieldName">The name of the field to get or set.</param>
    /// <param name="value">The value to set the field to. If null, the method performs a get operation.</param>
    /// <returns>The value of the field if performing a get operation; otherwise, null if performing a set operation or if an error occurs.</returns>
    /// <remarks>
    /// This method uses reflection to access fields of the Plugin object, allowing dynamic interaction with its fields.
    /// If the Plugin is null or the field is not found, the method logs an error and returns null.
    /// </remarks>
    /// <example>
    /// To get the value of a field:
    /// <code>
    /// var fieldValue = pluginHandler.GetOrSetField("FieldName");
    /// </code>
    /// To set the value of a field:
    /// <code>
    /// pluginHandler.GetOrSetField("FieldName", newValue);
    /// </code>
    /// </example>
    public override object GetOrSetField(string fieldName, object value = null)
    {
        if (Plugin == null) return null;
        try
        {
            var field = Plugin.GetType().GetField(fieldName);
            if (field == null)
            {
                Log.Error("Field not found: " + fieldName + " on plugin: " + Name);
                return null;
            }
        
            if (value != null)
            {
                field.SetValue(Plugin, value);
                return null; // Return null to indicate the operation was a set
            }
            return field.GetValue(Plugin);
        }
        catch (System.Exception e)
        {
            Log.Error("Failed to get/set field: " + fieldName + " on plugin: " + Name);
            Log.Error(e.Message);
            return null;
        }
    }

    public static bool GetPlugin(string name, out OptionalPlugin optionalPlugin)
    {
        var plugin = Loader.GetPlugin(name);
        if(plugin == null)
        {
            Log.Info("Optional Plugin not found: "+name);
            optionalPlugin = null;
            return false;
        }
        optionalPlugin = new OptionalPlugin(plugin);
        return true;
    }
    
    public static bool GetPlugin(IPlugin<IConfig> plugin, out OptionalPlugin optionalPlugin)
    {
        if(plugin == null)
        {
            Log.Info("Optional Plugin was null!");
            optionalPlugin = null;
            return false;
        }
        optionalPlugin = new OptionalPlugin(plugin);
        return true;
    }
    
    
}