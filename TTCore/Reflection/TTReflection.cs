using System;
using System.Reflection;
using PluginAPI.Core;

namespace TTCore.Reflection;

public class TTReflection
{
    
    
    public static object CallMethod(object obj, string methodName, params object[] args)
    {
        if(obj == null) return null;
        try
        {
            var method = obj.GetType().GetMethod(methodName);
            if (method?.ReturnType != typeof(void))
            {
                return method?.Invoke(obj, args);
            }
            else
            {
                method?.Invoke(obj, args);
                return null;
            }
            
        } catch (System.Exception e)
        {
            Log.Error("Failed to call method: "+methodName + " on object: "+obj);
            Log.Error(e.Message);
        }
        return null;
    }
    
    public static object GetProperty(object obj, string propertyName)
    {
        if(obj == null) return null;
        try
        {
            var property = obj.GetType().GetProperty(propertyName);
            return property?.GetValue(obj);
        } catch (System.Exception e)
        {
            Log.Error("Failed to get property: "+propertyName + " on object: "+obj);
            Log.Error(e.Message);
            return null;
        }
    }
    
    public static void SetProperty(object obj, string propertyName, object value)
    {
        if(obj == null) return;
        try
        {
            var property = obj.GetType().GetProperty(propertyName);
            property?.SetValue(obj, value);
        } catch (System.Exception e)
        {
            Log.Error("Failed to set property: "+propertyName + " on object: "+obj);
            Log.Error(e.Message);
        }
    }
    
    public static object GetObject(object obj, string fieldName)
    {
        if(obj == null) return null;
        try
        {
            var field = obj.GetType().GetField(fieldName);
            return field?.GetValue(obj);
        } catch (System.Exception e)
        {
            Log.Error("Failed to get field: "+fieldName + " on object: "+obj);
            Log.Error(e.Message);
            return null;
        }
    }
    
    public static void SetObject(object obj, string fieldName, object value)
    {
        if(obj == null) return;
        try
        {
            var field = obj.GetType().GetField(fieldName);
            field?.SetValue(obj, value);
        } catch (System.Exception e)
        {
            Log.Error("Failed to set field: "+fieldName + " on object: "+obj);
            Log.Error(e.Message);
        }
    }
    
    
    
    public static void CallVoidMethod(object obj, string methodName, params object[] args)
    {
        if(obj == null) return;
        try
        {
            var method = obj.GetType().GetMethod(methodName);
            method?.Invoke(obj, args);
        } catch (System.Exception e)
        {
            // ignored
            Log.Debug("Failed to call void method: " + methodName);
            Log.Debug(e.Message);
        }
    }
    
    public static object CallReturnMethod(object obj, string methodName, params object[] args)
    {
        if(obj == null) return null;
        try
        {
            var method = obj.GetType().GetMethod(methodName);
            return method?.Invoke(obj, args);
        } catch (System.Exception e)
        {
            Log.Debug("Failed to call return method: " + methodName);
            Log.Debug(e.Message);
            return null;
        }
    }
    
    
    public static object CallStaticMethod(Assembly assembly, string typeName, string methodName, params object[] args)
    {
        if(assembly == null) return null;
        try
        {
            var type = assembly.GetType(typeName);
            var method = type?.GetMethod(methodName);
            return method?.Invoke(null, args);
        } catch (System.Exception e)
        {
            Log.Debug("Failed to call static method: " + methodName);
            Log.Debug(e.Message);
            return null;
        }
    }
    
    public static object CallStaticMethod(Type type, string methodName, params object[] args)
    {
        if(type == null) return null;
        try
        {
            var method = type.GetMethod(methodName);
            return method?.Invoke(null, args);
        } catch (System.Exception e)
        {
            Log.Debug("Failed to call static method: " + methodName);
            Log.Debug(e.Message);
            return null;
        }
    }
    
}