using Bindings;

namespace Components
{
    public struct BusinessViewComponent
    {
        public int BusinessId;
        public ValueBinder<int> Level;
        public ValueBinder<double> LevelUpCost;
        public ValueBinder<double> TotalIncome;
        public ValueBinder<bool> FirstUpgradeBought;
        public ValueBinder<bool> SecondUpgradeBought;
        public ValueBinder<float> Progress;
    }
}
