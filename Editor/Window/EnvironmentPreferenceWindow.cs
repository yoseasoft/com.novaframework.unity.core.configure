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
            EnvironmentInstallationStep installStep = new EnvironmentInstallationStep();

            if (installStep.IsInstall())
            {
                if (_configures == null)
                {
                    _configures = EnvironmentConfigures.Instance;
                
                    if (_configures == null)
                    {
                        EditorGUILayout.HelpBox("未找到 EnvironmentConfigures 资源文件，请在 Resources 目录下创建。", MessageType.Warning);
                        return;
                    }
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
                    _serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(_configures);
                    AssetDatabase.SaveAssets();
                }
            }
            
            // 初始化按钮
            GUIStyle initButtonStyle = RichTextUtils.GetButtonTextOnlyStyle(Color.green);
            if (GUILayout.Button("初始化", initButtonStyle, GUILayout.Height(35)))
            {
                installStep.Install(() =>
                {
                    Debug.Log("初始化完成");
                }, () =>
                {
                    Debug.LogError("初始化失败");
                });
            }
        }
        
        private void OnGUI()
        {
            OnDraw();
        }
        
        private void OnDestroy()
        {
            _configures = null;
            _serializedObject = null;
        }
    }
}
