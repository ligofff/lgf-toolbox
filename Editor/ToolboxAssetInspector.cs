#if !ODIN_INSPECTOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LGF_Toolbox
{
    [CustomEditor(typeof(ToolboxAsset))]
    public class ToolboxAssetInspector : Editor
    {
        private SerializedProperty predefinedTools;

        private void OnEnable()
        {
            predefinedTools = serializedObject.FindProperty("predefinedTools");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(predefinedTools, true);
            if (GUILayout.Button("Add Tool", GUILayout.Width(100)))
            {
                CreateNewTool();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void CreateNewTool()
        {
            var window = EditorWindow.GetWindow<TypePickerWindow>("Select ITool", true);
            window.SetCallback(AddToolCallback, target.GetType().GetField("predefinedTools").GetValue(target) as List<ITool>);
            window.Show();
        }

        private void AddToolCallback(ITool tool)
        {
            predefinedTools.InsertArrayElementAtIndex(predefinedTools.arraySize);
            var toolProperty = predefinedTools.GetArrayElementAtIndex(predefinedTools.arraySize - 1);
            toolProperty.managedReferenceValue = tool;
            serializedObject.ApplyModifiedProperties();
            SetDirty();
        }

        private void SetDirty()
        {
            EditorUtility.SetDirty(target);
        }
    }

    public class TypePickerWindow : EditorWindow
    {
        private System.Action<ITool> callback;
        private List<ITool> _tools;

        public void SetCallback(System.Action<ITool> callback, List<ITool> tools)
        {
            this.callback = callback;
            _tools = tools;
        }

        private void OnGUI()
        {
            var tools = typeof(ITool).GetImplementedTypes();
            foreach (var toolType in tools)
            {
                var buttonLabel = toolType.Name;
                if (_tools.Any(tool => tool.GetType() == toolType))
                {
                    buttonLabel += " (Exists)";
                }
                
                if (GUILayout.Button(buttonLabel))
                {
                    if (_tools.Any(tool => tool.GetType() == toolType))
                    {
                        Close();
                        throw new Exception("Tool already exists!");
                        return;
                    }
                    
                    var newTool = Activator.CreateInstance(toolType);
                    callback?.Invoke(newTool as ITool);
                    Close();
                }
            }
        }
    }

    public static class TypeExtensions
    {
        public static IEnumerable<System.Type> GetImplementedTypes(this System.Type type)
        {
            return TypeCache.GetTypesDerivedFrom(typeof(ITool)).Where(t => t.GetInterfaces().Contains(type) && t.IsSerializable);
        }
    }
}

#endif