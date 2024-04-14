using System;
using UnityEngine;

namespace NothingOS.Glyphs
{
    public class GlyphManager
    {
        private string _modelId;

        private AndroidJavaObject _glyphManager;
        private AndroidJavaProxy _glyphCallbacks;

        private bool _managerWasInitialized;
        public bool managerWasInitialized => _managerWasInitialized;

        private bool _deviceWasRegistered;
        public bool deviceWasRegistered => _deviceWasRegistered;

        private bool _sessionWasOpened;
        public bool sessionWasOpened => _sessionWasOpened;

        internal GlyphManager(string modelId, AndroidJavaObject glyphManager)
        {
            this._modelId = modelId;
            this._glyphManager = glyphManager;
        }

        public void Initialize()
        {
            if (!_managerWasInitialized)
            {
                _glyphCallbacks = new GlyphManagerCallback(this);
                _glyphManager.Call("init", _glyphCallbacks);
                _managerWasInitialized = true;
                Debug.Log("Nothing: initialized");
            }
        }

        public void Shutdown()
        {
            CloseSession();

            if (_managerWasInitialized)
            {
                if (_glyphCallbacks != null)
                {
                    //do nothing
                    _glyphCallbacks = null;
                }

                if (_glyphManager != null)
                {
                    _glyphManager.Call("unInit");
                    _glyphManager = null;
                }
                _managerWasInitialized = false;
                Debug.Log("Nothing: shutdown");
            }
        }

        internal void OpenSession()
        {
            if (!_sessionWasOpened)
            {
                _deviceWasRegistered = _glyphManager.Call<bool>("register", _modelId);
                Debug.Log($"Nothing: modelId={_modelId}, deviceWasRegistered={_deviceWasRegistered}");

                try
                {
                    _glyphManager.Call("openSession");
                    _sessionWasOpened = true;
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
                finally
                {
                    Debug.Log($"Nothing: open session, sessionWasOpened={_sessionWasOpened}");
                }
            }
        }

        internal void CloseSession()
        {
            if (_sessionWasOpened)
            {
                try
                {
                    _glyphManager.Call("closeSession");
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
                finally
                {
                    _deviceWasRegistered = false;
                    _sessionWasOpened = false;
                    Debug.Log($"Nothing: close session, sessionWasOpened={_sessionWasOpened}");
                }
            }
        }

        public void SetFrameColors(int[] colors)
        {
            _glyphManager.Call("setFrameColors", colors);
        }

        public void Toggle(GlyphFrame frame)
        {
            _glyphManager.Call("toggle", frame.AsJavaObject());
        }

        public void Animate(GlyphFrame frame)
        {
            _glyphManager.Call("animate", frame.AsJavaObject());
        }

        public void TurnOff()
        {
            _glyphManager.Call("turnOff");
        }

        public void DisplayProgressAndToggle(GlyphFrame frame, int progress, bool isReverse)
        {
            _glyphManager.Call("displayProgressAndToggle", frame.AsJavaObject(), progress, isReverse);
        }

        public void DisplayProgress(GlyphFrame frame, int progress)
        {
            _glyphManager.Call("displayProgress", frame.AsJavaObject(), progress);
        }

        public void DisplayProgress(GlyphFrame frame, int progress, bool isReverse)
        {
            _glyphManager.Call("displayProgress", frame.AsJavaObject(), progress, isReverse);
        }

        public GlyphBuilder Builder()
        {
            var javaObject = _glyphManager.Call<AndroidJavaObject>("getGlyphFrameBuilder");
            if (javaObject != null)
                return new GlyphBuilder(javaObject);
            else
                return new GlyphBuilder(_modelId);
        }

        private class GlyphManagerCallback : AndroidJavaProxy
        {
            private GlyphManager _glyphManager;

            public GlyphManagerCallback(GlyphManager glyphManager) : base("com.nothing.ketchum.GlyphManager$Callback")
            {
                _glyphManager = glyphManager;
            }

            public void onServiceConnected(AndroidJavaObject javaComponentName)
            {
                var componentName = javaComponentName.AsString();
                Debug.Log($"Nothing: onServiceConnected: {componentName}");

                _glyphManager.OpenSession();
            }

            public void onServiceDisconnected(AndroidJavaObject javaComponentName)
            {
                var componentName = javaComponentName.AsString();
                Debug.Log($"Nothing: onServiceDisconnected: {componentName}");

                _glyphManager.CloseSession();
            }
        }
    }
}
