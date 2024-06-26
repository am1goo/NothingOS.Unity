#if UNITY_ANDROID
using UnityEngine;
#endif

using System.Collections.Generic;
using NothingOS.Glyphs;

namespace NothingOS
{
    public static class Nothing
    {
        private static bool _sdkAvailable;
        public static bool sdkAvailable => _sdkAvailable;

        private static bool _isInitialized;
        public static bool isInitialized => _isInitialized;

        private static GlyphManager _glyphs;
        public static GlyphManager glyphs => _glyphs;

        private static Model _model;
        public static Model model => _model;

        private static readonly Dictionary<Model, string> _ids = new Dictionary<Model, string>();

        static Nothing()
        {
#if UNITY_ANDROID
            if (Application.isEditor)
                return;

            using (var osBuild = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                //This SDK only works on Nothing devices running Android version 14 (UPSIDE_DOWN_CAKE) or newer.
                var versionSdkInt = osBuild.GetStatic<int>("SDK_INT");
                var expectedSdkInt = 34;
                _sdkAvailable = versionSdkInt >= expectedSdkInt;
                if (!_sdkAvailable)
                    Debug.LogWarning($"Nothing: sdk not available on this os version {_sdkAvailable}, you must be use os version {expectedSdkInt} or newer");
            }

            using (var common = new AndroidJavaClass("com.nothing.ketchum.Common"))
            {
                _ids[Model.Phone1] = common.GetStatic<string>("DEVICE_20111");
                _ids[Model.Phone2] = common.GetStatic<string>("DEVICE_22111");
                _ids[Model.Phone2a] = common.GetStatic<string>("DEVICE_23111");
#if DEBUG
                foreach (var kv in _ids)
                {
                    var model = kv.Key;
                    var id = kv.Value;
                    Debug.Log($"Nothing: model {model} has id {id}");
                }
#endif
                if (common.CallStatic<bool>("is20111"))
                {
                    _model = Model.Phone1;
                }
                else if (common.CallStatic<bool>("is22111"))
                {
                    _model = Model.Phone2;
                }
                else if (common.CallStatic<bool>("is23111"))
                {
                    _model = Model.Phone2a;
                }
                else
                {
                    _model = Model.Undefined;
                }
            }
#endif
        }

        public static void Initialize()
        {
#if UNITY_ANDROID
            if (Application.isEditor)
                return;

            if (!_sdkAvailable)
                return;

            if (_model == Model.Undefined)
                return;

            if (_glyphs != null)
                return;
            
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (var context = activity.Call<AndroidJavaObject>("getApplicationContext"))
                    {
                        using (var glyphManager = new AndroidJavaClass("com.nothing.ketchum.GlyphManager"))
                        {
                            var obj = glyphManager.CallStatic<AndroidJavaObject>("getInstance", context);
                            if (obj != null)
                            {
                                var modelId = GetModelId(model);
                                _glyphs = new GlyphManager(modelId, obj);
                                _glyphs.Initialize();
                                _isInitialized = true;
                            }
                        }
                    }
                }
            }
#endif
        }

        public static void Shutdown()
        {
#if UNITY_ANDROID
            if (Application.isEditor)
                return;

            if (!_sdkAvailable)
                return;

            if (_model == Model.Undefined)
                return;

            if (_glyphs != null)
            {
                _glyphs.Shutdown();
                _glyphs = null;
            }
#endif
        }

        public static string GetModelId(Model model)
        {
            if (_ids.TryGetValue(model, out var result))
            {
                return result;
            }
            else
            {
                return string.Empty;
            }
        }

        public enum Model
        {
            Undefined   = 0,
            Phone1      = 1,
            Phone2      = 2,
            Phone2a     = 3,
        }
    }
}
