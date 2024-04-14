#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace NothingOS
{
    public static class NothingEditor
    {
#if UNITY_EDITOR
        [MenuItem("Nothing GDK/Open Official Site ..")]
        private static void OpenOfficialSite()
        {
            Application.OpenURL("https://at.nothing.tech/");
        }

        [MenuItem("Nothing GDK/Open Plugin Site ..")]
        private static void OpenPluginSite()
        {
            Application.OpenURL("https://github.com/am1goo/nothing-glyph-developer-kit-for-unity");
        }
#endif
    }
}
