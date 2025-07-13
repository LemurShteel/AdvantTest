using System;
using Components;

namespace Saving
{
    [Serializable]
    public struct BusinessSave
    {
        public int Id;
        public int Level;
        public float IncomeTimeProgress;
        public UpgradeType Upgrade;
    }
}
