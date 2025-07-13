using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Providers
{
    [CreateAssetMenu(fileName = "New Description Provider", menuName = "ScriptableObjects/New Description Provider", order = 1)]
    public class DescriptionsProvider : ScriptableObject, IDescriptionsProvider
    {
        [SerializeField]
        private List<BusinessDescription> _businessData;
        private Dictionary<int, BusinessDescription> _businessDataDictionary;

        public void InitProvider()
        {
            _businessDataDictionary = _businessData.ToDictionary((element) => element.BusinessId.GetHashCode());
        }

        public Dictionary<int, BusinessDescription> GetBusinessDescriptions()
        {
            return _businessDataDictionary;
        }
    }
}