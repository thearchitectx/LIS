using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheArchitect.Core.Data;

namespace TheArchitect.Exploration
{
    public class WorldSelectableController : MonoBehaviour
    {
        [SerializeField] private Transform m_ActivationChild;
        [SerializeField] private WorldSelectionItem item = new WorldSelectionItem(){
            Name = "Unamed Object",
            InteractionDistance = 1.5f
        };

        public Transform GetActivationChild()
        {
            if (this.m_ActivationChild==null)
                this.m_ActivationChild = this.transform.Find("Activation");

            return this.m_ActivationChild;
        }

        public WorldSelectionItem AsSelectionItem()
        {
            item.Name = gameObject.name;
            return item;
        }

    }
}