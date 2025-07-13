using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Providers
{
    [CreateAssetMenu(fileName = "New Naming Provider", menuName = "ScriptableObjects/New Naming Provider", order = 1)]
    public class NamingProvider : ScriptableObject, INamingProvider
    {
        [Serializable]
        private struct NamingKeys
        {
            public string key;
            public string name;
        }

        [SerializeField]
        private List<NamingKeys> _namingKeys = new List<NamingKeys>();
        private Dictionary<string, string> _namingDictionary = new Dictionary<string, string>();

        public void InitProvider()
        {
            _namingDictionary = _namingKeys.ToDictionary(e => e.key, e => e.name);
        }

        public string GetName(string key)
        {
            return string.IsNullOrEmpty(key) ? key : _namingDictionary.GetValueOrDefault(key, key);
        }
    }
}
