using System;
using System.IO;
using NovaFramework.Editor.Preference;
using NovaFramework.Serialization;
using UnityEditor;
using UnityEngine;

namespace NovaFramework.Editor
{
    public class EnvironmentInstallationStep : InstallationStep
    {
        const string _environmentConfiguresPath = "Assets/Resources/EnvironmentConfigures.asset";
        
        public void Install(Action onComplete, Action onError)
        {
            // 创建 EnvironmentConfigures 配置文件到 Resources 目录
            CreateEnvironmentConfigures();
            onComplete?.Invoke();
        }

        /// <summary>
        /// 创建 EnvironmentConfigures 配置文件到 Resources 目录
        /// </summary>
        private static void CreateEnvironmentConfigures()
        {
            string resourcesPath = Path.Combine(Application.dataPath, "Resources");

            // 确保 Resources 目录存在
            if (!Directory.Exists(resourcesPath))
            {
                Directory.CreateDirectory(resourcesPath);
                Logger.Info($"已创建 Resources 目录");
            }

            // 创建 ScriptableObject 实例并填入默认值
            EnvironmentConfigures configures = ScriptableObject.CreateInstance<EnvironmentConfigures>();
            ApplyDefaults(configures);

            // 创建资产文件
            AssetDatabase.CreateAsset(configures, _environmentConfiguresPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            //生成默认路径
            CreateDefaultDirectories(configures);
            AssetDatabase.Refresh();

            Logger.Info($"已创建环境配置文件: {_environmentConfiguresPath}");
        }
        
        private static void ApplyDefaults(EnvironmentConfigures configures)
        {
            // variables
            configures.variables.Add(new SerializedVariableObject { key = "ORIGINAL_RESOURCE_PATH", value = "Assets/_Resources" });
            configures.variables.Add(new SerializedVariableObject { key = "SOURCE_CODE_PATH", value = "Assets/Sources" });
            configures.variables.Add(new SerializedVariableObject { key = "AOT_LIBRARY_PATH", value = "Assets/_Resources/Aot" });
            configures.variables.Add(new SerializedVariableObject { key = "LINK_LIBRARY_PATH", value = "Assets/_Resources/Code" });
            configures.variables.Add(new SerializedVariableObject { key = "CONTEXT_PATH", value = "Assets/_Resources/Context" });
            configures.variables.Add(new SerializedVariableObject { key = "CONFIG_PATH", value = "Assets/_Resources/Config" });
            configures.variables.Add(new SerializedVariableObject { key = "GUI_PATH", value = "Assets/_Resources/Gui/" });

            // modules
            configures.modules.Add(new SerializedLibraryObject { name = "Game", order = 1102, tags = new() { "Game", "Compile" } });
            configures.modules.Add(new SerializedLibraryObject { name = "GameHotfix", order = 1104, tags = new() { "Game", "Compile", "Hotfix" } });

            // aots
            configures.aots.Add("System.Core.dll");
            configures.aots.Add("System.dll");
            configures.aots.Add("mscorlib.dll");
            configures.aots.Add("UnityEngine.CoreModule.dll");
        }

        private static void CreateDefaultDirectories(EnvironmentConfigures configures)
        {
            foreach (var variable in configures.variables)
            {
                string fullPath = Path.Combine(Application.dataPath, "..", variable.value);
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                    Logger.Info($"已创建目录: {variable.value}");
                }
            }
        }
        
        public void Uninstall(Action onComplete, Action onError)
        {
            AssetDatabase.DeleteAsset(_environmentConfiguresPath);
            onComplete?.Invoke();
        }
    }
}

