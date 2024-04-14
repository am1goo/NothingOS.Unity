using System;
using UnityEngine;

namespace NothingOS.Glyphs
{
    public class GlyphFrame : IDisposable
    {
        private AndroidJavaObject _javaObject;

        private bool _isDisposed;
        public bool isDisposed => _isDisposed;

        public GlyphFrame(AndroidJavaObject javaObject)
        {
            this._javaObject = javaObject;
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            if (_javaObject != null)
            {
                _javaObject.Dispose();
                _javaObject = null;
            }
            _isDisposed = true;
        }

        public int GetPeriod()
        {
            return _javaObject.Call<int>("getPeriod");
        }

        public int GetCycles()
        {
            return _javaObject.Call<int>("getCycles");
        }

        public int GetInterval()
        {
            return _javaObject.Call<int>("getInterval");
        }

        public int[] GetChannel()
        {
            return _javaObject.CallArray<int>("getChannel");
        }

        public AndroidJavaObject AsJavaObject()
        {
            return _javaObject;
        }
    }
}
