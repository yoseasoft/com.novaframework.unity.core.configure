using NovaFramework.Editor.Preference;
using NovaFramework.Serialization;
using UnityEditor;
using UnityEngine;

namespace NovaFramework.Editor
{
    public class EnvironmentPreferenceWindow : PreferenceWindow
    {
        public override string PagingName => "环境配置";

        private EnvironmentConfigures _configures;
        private SerializedObject _serializedObject;
        private Vector2 _scrollPos;

        public override void OnDraw()
        {
            if (_configures == null)
            {
                _configures = EnvironmentConfigures.Instance;
            }

            if (_configures == null)
            {
                EditorGUILayout.HelpBox("未找到 EnvironmentConfigures 资源文件，请在 Resources 目录下创建。", MessageType.Warning);
                if (GUILayout.Button("创建 EnvironmentConfigures", GUILayout.Height(30)))
                {
                    CreateEnvironmentConfigures();
                }
                return;
            }

            if (_serializedObject == null || _serializedObject.targetObject != _configures)
            {
                _serializedObject = new SerializedObject(_configures);
            }

            _serializedObject.Update();

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            // 遍历绘制所有可见属性
            SerializedProperty iterator = _serializedObject.GetIterator();
            iterator.NextVisible(true); // 跳过 m_Script
            while (iterator.NextVisible(false))
            {
                EditorGUILayout.PropertyField(iterator, true);
            }

            EditorGUILayout.EndScrollView();

            if (_serializedObject.hasModifiedProperties)
            {
                EditorGUILayout.Space(10);
                if (GUILayout.Button("保存", GUILayout.Height(30)))
                {
                    _serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(_configures);
                    AssetDatabase.SaveAssets();
                    Logger.Info("[EnvironmentPreference] 环境配置已保存");
                }
            }
        }

        private void OnGUI()
        {
            OnDraw();
        }

        private void CreateEnvironmentConfigures()
        {
            string dir = "Assets/Resources";
            if (!AssetDatabase.IsValidFolder(dir))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Application.dataPath, "Resources"));
                AssetDatabase.Refresh();
            }

            var asset = CreateInstance<EnvironmentConfigures>();
            AssetDatabase.CreateAsset(asset, dir + "/EnvironmentConfigures.asset");
            AssetDatabase.SaveAssets();
            _configures = asset;
            Logger.Info("[EnvironmentPreference] 已创建 EnvironmentConfigures 资源文件");
        }

        private void OnDestroy()
        {
            _serializedObject = null;
        }
    }
}
