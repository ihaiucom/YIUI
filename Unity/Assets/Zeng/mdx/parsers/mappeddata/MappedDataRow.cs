using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Zeng.mdx.parsers
{
    [Serializable]
    public class MappedDataRow
    {
        [SerializeField]
        private Dictionary<string, string> map = new Dictionary<string, string>();

        public void Set(string key, string value)
        {
            map[key.ToLower()] = value;
        }

        public void Set(string key, int value)
        {
            map[key.ToLower()] = value.ToString();
        }

        public string GetString(string key)
        {
            if (map.ContainsKey(key.ToLower()))
            {
                return map[key.ToLower()];
            }
            return null;
        }

        public int GetNumber(string key)
        {
            if (map.ContainsKey(key.ToLower()))
            {
                string stringValue = map[key.ToLower()];
                if (int.TryParse(stringValue, out int numberValue))
                {
                    return numberValue;
                }
            }
            return 0;
        }
    }
}
