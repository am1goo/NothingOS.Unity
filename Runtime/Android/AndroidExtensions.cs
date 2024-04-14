using UnityEngine;

namespace NothingOS
{
    public static class AndroidExtensions
    {
        public static string AsString(this AndroidJavaObject javaObj)
        {
            return javaObj.Call<string>("toString");
        }

        public static bool IsNull(this AndroidJavaObject obj)
        {
            if (obj == null)
                return true;

            var raw = obj.GetRawObject();
            return raw.ToInt32() == 0;
        }

        public static T[] CallArray<T>(this AndroidJavaObject obj, string methodName, params object[] args)
        {
            var resObj = obj.Call<AndroidJavaObject>(methodName, args);
            if (resObj == null)
                return null;

            if (resObj.IsNull())
                return null;

            var rawObj = resObj.GetRawObject();
            return AndroidJNIHelper.ConvertFromJNIArray<T[]>(rawObj);
        }
    }
}
