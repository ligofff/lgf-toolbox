using System;
using System.Collections.Generic;
using System.Reflection;

namespace LGF_Toolbox
{
    public class Toolbox
    {
        public IEnumerable<ITool> Tools => _tools.Values;
    
        private Dictionary<Type, ITool> _tools = new Dictionary<Type, ITool>();

        public void Add<T>(T toolInstance) where T : ITool
        {
            if (_tools.ContainsKey(toolInstance.GetType()))
                throw new Exception($"Toolbox already contains a tool of type {typeof(T)}!");

            var toolType = toolInstance.GetType();
            
            if (toolType.GetMethod("Init", BindingFlags.Instance | BindingFlags.NonPublic) is {} method)
            {
                method.Invoke(toolInstance, null);
            }
            
            _tools[toolType] = toolInstance;
            
        }

        public void Remove<T>() where T : ITool
        {
            if (!_tools.ContainsKey(typeof(T)))
                throw new Exception($"Toolbox does not contain a tool of type {typeof(T)}!");
        
            _tools.Remove(typeof(T));
        }
    
        public T Get<T>() where T : ITool
        {
            if (_tools.TryGetValue(typeof(T), out var tool))
                return (T)tool;
        
            throw new Exception($"Toolbox does not contain a tool of type {typeof(T)}!");
        }
    }
}