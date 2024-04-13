using UnityEngine;

namespace NothingGDK
{
    public static class AndroidExtensions
    {
        public static string AsString(this AndroidJavaObject javaObj)
        {
            return javaObj.Call<string>("toString");
        }
    }
}
