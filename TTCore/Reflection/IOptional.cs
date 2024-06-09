using System.Reflection;

namespace TTCore.Reflection;

public interface IOptional
{
    public abstract object OptionalObject { get; set; }
    public abstract string Name { get; }
    public abstract Assembly Assembly { get; }
    public bool IsPresent { get; }
    
    public abstract object CallMethod(string methodName, params object[] args);
    
    public abstract object GetOrSetField(string fieldName, object value=null);
    
}