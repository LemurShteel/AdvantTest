using Components;
using Providers;

namespace Utils
{
    public static class MoneyUtils
    {
        public static double GetLevelUpCost(ref BusinessComponent business, BusinessDescription description)
        {
            //levelCost = cost = (lvl+1) * базовая_стоимость
            return (business.Level + 1) * description.BaseCost;
        }

        public static double GetIncome(ref BusinessComponent business, BusinessDescription description)
        {
            //lvl * базовый_доход * (1 + множитель_от_улучшения_1 + множитель_от_улучшения_2)
            var firstUpgrade = (business.Upgrade & UpgradeType.FirstUpgrade) != 0 ? description.FirstUpgradeMultiplier : 0; 
            var secondUpgrade = (business.Upgrade & UpgradeType.SecondUpgrade) != 0 ? description.SecondUpgradeMultiplier : 0; 
            return business.Level * description.BaseIncome * (1 + firstUpgrade + secondUpgrade);
        }

        public static void ChangePlayerMoney(ref PlayerMoneyComponent money, ref PlayerMoneyViewComponent view, double amount)
        {
            money.Money += amount;
            view.Money.SetValue(money.Money);
        }
    }
}
