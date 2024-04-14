#if UNITY_EDITOR
using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
#if UNITY_ANDROID
using UnityEditor.Android;
#endif
#endif
using UnityEngine;

namespace NothingOS
{
    public static class NothingEditor
    {
#if UNITY_EDITOR
        [MenuItem("Nothing GDK/Debug mode on Device/Enable", priority = 0)]
        private static void EnableDebugModeOnDevice()
        {
            SetDebugModeOnDevice(true);
        }

        [MenuItem("Nothing GDK/Debug mode on Device/Disable", priority = 1)]
        private static void DisableDebugModeOnDevice()
        {
            SetDebugModeOnDevice(false);
        }

        [MenuItem("Nothing GDK/Open Official Site ..", priority = 100)]
        private static void OpenOfficialSite()
        {
            Application.OpenURL("https://at.nothing.tech/");
        }

        [MenuItem("Nothing GDK/Open Plugin Site ..", priority = 101)]
        private static void OpenPluginSite()
        {
            Application.OpenURL("https://github.com/am1goo/nothing-glyph-developer-kit-for-unity");
        }

        private static void SetDebugModeOnDevice(bool value)
        {
            var key = "nt_glyph_interface_debug_enable";
            var result = SetAdbProperty(key, value ? 1 : 0);
            if (result.returnCode != 0)
            {
                EditorUtility.DisplayDialog("SetDebugModeOnDevice", result.log, "Okay");
                return;
            }

            var state = value ? "enabled" : "disabled";
            EditorUtility.DisplayDialog("SetDebugModeOnDevice", $"Debug Mode switched to {state} state", "Okay");
        }

        private static (int returnCode, string log) SetAdbProperty(string key, object value)
        {
            var sdkRootPath = string.Empty;
#if UNITY_ANDROID
            sdkRootPath = AndroidExternalToolsSettings.sdkRootPath;
#endif
            if (string.IsNullOrWhiteSpace(sdkRootPath))
                return (-1, "Android SDK folder not found");

            var adbPath = Path.Combine(sdkRootPath, "platform-tools", "adb");
            var adbFileInfo = new FileInfo(adbPath);
            if (!adbFileInfo.Directory.Exists)
                return (-1, $"folder {adbFileInfo.Directory.FullName} doesn't exists");

            var args = $"shell settings put global {key} {value}";
            return RunAdb(adbPath, args);
        }

        private static (int returnCode, string log) RunAdb(string adbPath, string args)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = adbPath,
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            try
            {
                var process = Process.Start(startInfo);
                process.WaitForExit();

                var retCode = process.ExitCode;
                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();

                var log = string.Join(Environment.NewLine, output, error).Trim();
                return (retCode, log);
            }
            catch (Exception ex)
            {
                return (-1, ex.ToString());
            }
        }
#endif
    }
}
