using System;
using UnityEngine;

namespace TheArchitect.Core.Data
{
    [Serializable]
    public class WorldSelectionItem
    {
        public string Name { get; set; }
        public string DisplayName;
        public float InteractionDistance;
        public bool DisablePlayer = true;
    }
}