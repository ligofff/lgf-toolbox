using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

[CreateAssetMenu(fileName = "new Toolbox Asset", menuName = "Ligofff/Toolbox")]
public class ToolboxAsset : ScriptableObject
{
#if ODIN_INSPECTOR
    [ValidateInput(nameof(ValidateList), "List contains duplicates!"), ListDrawerSettings(DefaultExpandedState = true), TypeFilter(nameof(GetTypes))]
#endif
    [SerializeReference]
    public List<ITool> predefinedTools = new List<ITool>();

    private Toolbox _cachedToolbox;

    public Toolbox Get
    {
        get
        {
            if (_cachedToolbox != null)
                return _cachedToolbox;
            
            _cachedToolbox = new Toolbox();
            
            AddPredefinedTools(_cachedToolbox);
            AddTools(_cachedToolbox);
            
            return _cachedToolbox;
        }
    }

    public void ResetToolbox()
    {
        _cachedToolbox = null;
    }

    private void AddPredefinedTools(Toolbox toolbox)
    {
        foreach (var tool in predefinedTools)
        {
            toolbox.Add(tool);
        }
    }
        
    protected virtual void AddTools(Toolbox toolbox)
    {
            
    }
    
    // Editor things
    private bool ValidateList()
    {
        return predefinedTools.Select(tool => tool.GetType()).Distinct().Count() == predefinedTools.Count;
    }

    private IEnumerable<Type> GetTypes()
    {
#if UNITY_EDITOR
        return TypeCache.GetTypesDerivedFrom<ITool>().Where(type => !predefinedTools.Any(tool => tool.GetType() == type));
#endif
        return Enumerable.Empty<Type>();
    }
}