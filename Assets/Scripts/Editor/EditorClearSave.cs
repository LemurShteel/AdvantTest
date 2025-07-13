using Saving;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class EditorClearSave
    {
        [MenuItem("Tools/Clear Save")]
        public static void ClearPlayerSave()
        {
            PlayerPrefs.DeleteKey(GameStateSave.PlayerSaveKey);
            if (Application.isPlaying)
            {
                Application.Quit();
            }
        }
    }
}
