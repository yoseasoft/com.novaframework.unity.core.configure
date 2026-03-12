using NovaFramework.Editor.Preference;
using UnityEditor;
using UnityEngine;

namespace NovaFramework.Editor
{
    public class EnvironmentTestWindow : PreferenceWindow
    {
        public override string PagingName => "环境配置2";

        public override void OnDraw()
        {
            OnGUI();
        }
        
        public void OnGUI()
        {
            EditorGUILayout.LabelField("环境配置", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("环境配置页面（待实现）", MessageType.Info);
        }
    }
}
