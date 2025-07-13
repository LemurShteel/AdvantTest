using System;
using System.Collections.Generic;
using UnityEngine;

namespace Saving
{
    [Serializable]
    public class GameStateSave
    {
        public const string PlayerSaveKey = "GameStateSave";

        public double playerMoney;
        public List<BusinessSave> businessSaves = new List<BusinessSave>();

        public void LoadFromPrefs()
        {
            if (!PlayerPrefs.HasKey(PlayerSaveKey))
            {
                return;
            }
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(PlayerSaveKey), this);
        }

        public void SaveToPrefs()
        {
            PlayerPrefs.SetString(PlayerSaveKey, JsonUtility.ToJson(this));
            PlayerPrefs.Save();
        }
    }
}
