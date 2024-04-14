using System;
using UnityEngine;

namespace NothingOS.Glyphs
{
    public class GlyphBuilder : IDisposable
    {
        private AndroidJavaObject _javaObject;

        private bool _isDisposed;
        public bool isDisposed => _isDisposed;

        internal GlyphBuilder(string targetDevice)
        {
            this._javaObject = new AndroidJavaObject("com.nothing.ketchum.GlyphFrame$Builder", targetDevice);
        }

        internal GlyphBuilder(AndroidJavaObject javaObject)
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

        public GlyphBuilder SetPeriod(TimeSpan period)
        {
            _javaObject.Call<AndroidJavaObject>("buildPeriod", (int)period.TotalMilliseconds);
            return this;
        }

        public GlyphBuilder SetCycles(int cycles)
        {
            _javaObject.Call<AndroidJavaObject>("buildCycles", cycles);
            return this;
        }

        public GlyphBuilder SetInterval(TimeSpan interval)
        {
            _javaObject.Call<AndroidJavaObject>("buildInterval", (int)interval.TotalMilliseconds);
            return this;
        }

        public GlyphBuilder SetChannel(int channel)
        {
            _javaObject.Call<AndroidJavaObject>("buildChannel", channel);
            return this;
        }

        public GlyphBuilder SetChannel(int channel, int light)
        {
            _javaObject.Call<AndroidJavaObject>("buildChannel", channel, light);
            return this;
        }

        public GlyphBuilder SetChannel(Channel channel)
        {
            switch (channel)
            {
                case Channel.ChannelA:
                    SetChannelA();
                    break;

                case Channel.ChannelB:
                    SetChannelB();
                    break;

                case Channel.ChannelC:
                    SetChannelC();
                    break;

                case Channel.ChannelD:
                    SetChannelD();
                    break;

                case Channel.ChannelE:
                    SetChannelE();
                    break;
            }
            return this;
        }

        public GlyphBuilder SetChannelA()
        {
            _javaObject.Call<AndroidJavaObject>("buildChannelA");
            return this;
        }

        public GlyphBuilder SetChannelB()
        {
            _javaObject.Call<AndroidJavaObject>("buildChannelB");
            return this;
        }

        public GlyphBuilder SetChannelC()
        {
            _javaObject.Call<AndroidJavaObject>("buildChannelC");
            return this;
        }

        public GlyphBuilder SetChannelD()
        {
            _javaObject.Call<AndroidJavaObject>("buildChannelD");
            return this;
        }

        public GlyphBuilder SetChannelE()
        {
            _javaObject.Call<AndroidJavaObject>("buildChannelE");
            return this;
        }

        public GlyphFrame Build()
        {
            var javaObject = _javaObject.Call<AndroidJavaObject>("build");
            return new GlyphFrame(javaObject);
        }

        public enum Channel
        {
            ChannelA = 0,
            ChannelB = 1,
            ChannelC = 2,
            ChannelD = 3,
            ChannelE = 4,
        }
    }
}
