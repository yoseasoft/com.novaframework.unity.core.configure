using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NovaFramework.Editor.Preference;
using NovaFramework.Serialization;
using UnityEditor;
using UnityEngine;

namespace NovaFramework.Editor
{
    public class EnvironmentInstallationStep : InstallationStep
    {
        
        public void Install(Action onComplete)
        {
            // 创建 EnvironmentConfigures 配置文件到 Resources 目录
            CreateEnvironmentConfigures();
            onComplete?.Invoke();
        }
        
        [MenuItem("Tools/自动安装aaaxxx")]
        public static void Install2ddd()
        {
            // 创建 EnvironmentConfigures 配置文件到 Resources 目录
            CreateEnvironmentConfigures();
        }

        /// <summary>
        /// 创建 EnvironmentConfigures 配置文件到 Resources 目录
        /// </summary>
        private static void CreateEnvironmentConfigures()
        {
            string resourcesPath = Path.Combine(Application.dataPath, "Resources");
            string assetPath = Path.Combine(resourcesPath, "EnvironmentConfigures.asset");
            string unityAssetPath = "Assets/Resources/EnvironmentConfigures.asset";

            // 如果已经存在，不需要重新创建
            // EnvironmentConfigures existingAsset = Resources.Load<EnvironmentConfigures>(nameof(EnvironmentConfigures));
            // if (existingAsset != null)
            // {
            //     Debug.Log($"[EnvironmentInstallationStep] EnvironmentConfigures 已存在，跳过创建");
            //     return;
            // }

            // 确保 Resources 目录存在
            if (!Directory.Exists(resourcesPath))
            {
                Directory.CreateDirectory(resourcesPath);
                Debug.Log($"[EnvironmentInstallationStep] 创建 Resources 目录: {resourcesPath}");
            }

            // 创建 ScriptableObject 实例
            EnvironmentConfigures configures = ScriptableObject.CreateInstance<EnvironmentConfigures>();

            // 创建资产文件
            AssetDatabase.CreateAsset(configures, unityAssetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"[EnvironmentInstallationStep] EnvironmentConfigures 配置文件已创建: {unityAssetPath}");
        }

        public void Uninstall(Action onComplete = null)
        {
            onComplete?.Invoke();
        }
    }
}

