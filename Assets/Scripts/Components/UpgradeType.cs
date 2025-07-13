using System;

namespace Components
{
    [Flags]
    public enum UpgradeType
    {
        None = 0,
        FirstUpgrade = 1 << 0,
        SecondUpgrade = 1 << 1
    }
}
