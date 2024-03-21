
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zeng.mdx.handlers.w3x
{
    [Serializable]
    public class CliffItem
    {
        public string path;

        [SerializeField]
        public List<float> locations = new List<float>();
        [SerializeField]
        public List<int> textures = new List<int>();
    }
}
