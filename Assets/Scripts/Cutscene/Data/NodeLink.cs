using System;
using UnityEngine;

namespace TheArchitect.Cutscene
{
    [Serializable]
    public class NodeLink
    {
        public string Port { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}