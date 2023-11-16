using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Toolbox Asset", menuName = "Ligofff/Toolbox")]
public class ToolboxAsset : ScriptableObject
{
    [SerializeReference]
    public List<ITool> predefinedTools = new List<ITool>();

    private Toolbox _cachedToolbox;

    public Toolbox GetToolbox()
    {
        if (_cachedToolbox != null)
            return _cachedToolbox;
            
        _cachedToolbox = new Toolbox();
            
        AddPredefinedTools(_cachedToolbox);
        AddTools(_cachedToolbox);
            
        return _cachedToolbox;
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
}