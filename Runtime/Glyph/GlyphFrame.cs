using UnityEngine;

namespace NothingOS.Glyphs
{
    public class GlyphFrame
    {
        private AndroidJavaObject _javaObject;

        public GlyphFrame(AndroidJavaObject javaObject)
        {
            this._javaObject = javaObject;
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
