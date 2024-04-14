#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace NothingOS
{
    [CreateAssetMenu(fileName = "NothingSettings", menuName = "Nothing GDK/Create Settings")]
    public class NothingSettings : ScriptableObject
    {
        [SerializeField]
        private string _apiKey;
        public string apiKey => _apiKey;

#if UNITY_EDITOR
        public static NothingSettings FindInAssets()
        {
            var guids = AssetDatabase.FindAssets("t:Object");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var obj = AssetDatabase.LoadAssetAtPath<NothingSettings>(path);
                if (obj == null)
                    continue;

                return obj;
            }

            return null;
        }
#endif
    }
}
