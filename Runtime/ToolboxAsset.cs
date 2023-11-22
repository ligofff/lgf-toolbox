using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

[CreateAssetMenu(fileName = "new Toolbox Asset", menuName = "Ligofff/Toolbox")]
public class ToolboxAsset : ScriptableObject
{
#if ODIN_INSPECTOR
    [ValidateInput(nameof(ValidateList), "List contains duplicates!"), ListDrawerSettings(DefaultExpandedState = true)]
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

    private bool ValidateList()
    {
        return predefinedTools.Select(tool => tool.GetType()).Distinct().Count() == predefinedTools.Count;
    }
}