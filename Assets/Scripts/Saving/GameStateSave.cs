using System;
using System.Collections.Generic;
using Components;
using Leopotam.EcsLite;
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

        public void SaveToPrefs(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var moneyFilter = world.Filter<PlayerMoneyComponent>().End();
            ref var money = ref world.GetPool<PlayerMoneyComponent>().Get(moneyFilter.GetRawEntities()[0]);
            playerMoney = money.Money;

            businessSaves.Clear();
            var businessFilter = world.Filter<BusinessComponent>().End();
            foreach (var businessEntity in businessFilter)
            {
                ref var business = ref world.GetPool<BusinessComponent>().Get(businessEntity);
                businessSaves.Add(new BusinessSave()
                {
                    Id = business.ID,
                    IncomeTimeProgress = business.IncomeTimeProgress,
                    Level = business.Level,
                    Upgrade = business.Upgrade
                });
            }
            PlayerPrefs.SetString(PlayerSaveKey, JsonUtility.ToJson(this));
            PlayerPrefs.Save();
        }
    }
}
