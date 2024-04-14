#if UNITY_EDITOR
#if UNITY_ANDROID
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Android;
using UnityEngine;

namespace NothingOS
{
    public class NothingPostGradleAndroidProject : IPostGenerateGradleAndroidProject
    {
        private const string ANDROID_MANIFEST = "AndroidManifest.xml";
        public int callbackOrder => 0;

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            var androidManifestPath = Path.Combine(path, "src", "main", ANDROID_MANIFEST);
            var androidManifestFile = new FileInfo(androidManifestPath);
            if (!androidManifestFile.Exists)
            {
                Debug.LogError($"OnPostGenerateGradleAndroidProject: {ANDROID_MANIFEST} not found at path {androidManifestPath}");
                return;
            }

            Debug.Log($"OnPostGenerateGradleAndroidProject: modifying {ANDROID_MANIFEST} at path {androidManifestPath}");

            var settings = NothingSettings.FindInAssets();
            if (settings == null)
            {
                Debug.LogError($"OnPostGenerateGradleAndroidProject: {typeof(NothingSettings)} not found in Assets");
                return;
            }

            var settingsPath = AssetDatabase.GetAssetPath(settings);
            Debug.Log($"OnPostGenerateGradleAndroidProject: {typeof(NothingSettings)} found at path {settingsPath}");

            var applicationTag = "</application>";
            var dataToInject = $"<meta-data android:name=\"NothingKey\" android:value=\"{settings.apiKey}\" />";

            var xml = File.ReadAllText(androidManifestFile.FullName);
            xml = xml.Replace(applicationTag, $"  {dataToInject}{Environment.NewLine}  {applicationTag}");
            File.WriteAllText(androidManifestFile.FullName, xml);
            Debug.Log($"OnPostGenerateGradleAndroidProject: {ANDROID_MANIFEST} patched successfully");
        }
    }
}
#endif
#endif
