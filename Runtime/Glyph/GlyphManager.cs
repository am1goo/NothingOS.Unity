using System;
using UnityEngine;

namespace NothingGDK
{
    public class GlyphManager
    {
        private AndroidJavaObject _glyphManager;
        private AndroidJavaProxy _glyphCallbacks;

        private bool _managerWasInitialized;
        public bool managerWasInitialized => _managerWasInitialized;

        private bool _sessionWasOpened;
        public bool sessionWasOpened => _sessionWasOpened;

        public GlyphManager(AndroidJavaObject glyphManager)
        {
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
                var model = Nothing.model;
                var modelId = Nothing.GetModelId(model);
                var registered = _glyphManager.Call<bool>("register", modelId);
                Debug.Log($"Nothing: model {model}, modelId={modelId}, registered={registered}");

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
                    _sessionWasOpened = false;
                    Debug.Log($"Nothing: close session, sessionWasOpened={_sessionWasOpened}");
                }
            }
        }

        public void TurnOff()
        {
            _glyphManager.Call("turnOff");
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
