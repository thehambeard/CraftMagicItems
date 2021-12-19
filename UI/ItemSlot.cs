using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CraftMagicItems.UI
{
    public class ItemSlot
    {
        public enum Type
        {
            Weapon,
            Armor,
            Accessory,
            Usable
        }

        public ItemSlot.Type ItemType { get; set; }
        public string DisplayName {get; set;}
        public Sprite Icon { get; set; }
    }
}
