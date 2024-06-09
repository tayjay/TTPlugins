using System;
using System.Reflection;
using Exiled.API.Features;

namespace TTCore.Reflection;

public class OptionalReference : IOptional
{
    public object OptionalObject { get; set; }
    public string Name => OptionalObject.GetType().Name;
    public Assembly Assembly => OptionalObject.GetType().Assembly;
    public bool IsPresent => OptionalObject != null;
    
    protected OptionalReference(object optionalObject)
    {
        OptionalObject = optionalObject;
    }
    
    public virtual object CallMethod(string methodName, params object[] args)
    {
        if(OptionalObject == null) return null;
        try
        {
            var method = OptionalObject.GetType().GetMethod(methodName);
            if (method?.ReturnType != typeof(void))
            {
                return method?.Invoke(OptionalObject, args);
            }
            else
            {
                method?.Invoke(OptionalObject, args);
                return null;
            }
            
        } catch (System.Exception e)
        {
            Log.Error("Failed to call method: "+methodName + " on optional: "+Name);
            Log.Error(e.Message);
        }
        return null;
    }

    public virtual object GetOrSetField(string fieldName, object value = null)
    {
        if (OptionalObject == null) return null;
        try
        {
            var field = OptionalObject.GetType().GetField(fieldName);
            if (field == null)
            {
                Log.Error("Field not found: " + fieldName + " on optional: " + Name);
                return null;
            }
            if (value != null)
            {
                field.SetValue(OptionalObject, value);
                return null; // Return null to indicate the operation was a set
            }
            return field.GetValue(OptionalObject);
        }
        catch (System.Exception e)
        {
            Log.Error("Failed to get/set field: " + fieldName + " on optional: " + Name);
            Log.Error(e.Message);
            return null;
        }
        
        
    }

    public virtual T GetOrSetField<T>(string fieldName, T value = default)
    {
        if (OptionalObject == null) return default;
        try
        {
            var field = OptionalObject.GetType().GetField(fieldName);
            if (field == null)
            {
                Log.Error("Field not found: " + fieldName + " on optional: " + Name);
                return default;
            }
        
            if (value != null)
            {
                field.SetValue(OptionalObject, value);
                return default; // Return null to indicate the operation was a set
            }
            return (T)field.GetValue(OptionalObject);
        }
        catch (System.Exception e)
        {
            Log.Error("Failed to get/set field: " + fieldName + " on optional: " + Name);
            Log.Error(e.Message);
            return default;
        }
    }
    
    public static bool Create(object obj, out OptionalReference optionalReference)
    {
        if(obj == null)
        {
            Log.Info("Referenced Object was null!");
            optionalReference = null;
            return false;
        }
        optionalReference = new OptionalReference(obj);
        return true;
    }

}