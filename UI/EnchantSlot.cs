using Kingmaker.Blueprints.Items.Ecnchantments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CraftMagicItems.UI
{
    public class EnchantSlot
    {
       

        public enum ExclusiveGroup
        {
            None,
            AcidResist,
            Adamantine,
            Bane,
            Charisma,
            EquipmentArmorBonus,
            LawOrChaos,
            Plus
        }

        public Enums.ItemType EnchantType;
        public EnchantSlot.ExclusiveGroup Grouping;
        public string DisplayName;
        public string Description;
        public int Cost;
        public int CL;
        public Transform DisableLayer;
        public BlueprintItemEnchantment Blueprint;
        public bool Enabled = true;
        public Sprite Sprite;
        public bool Selected = false;
    }
}
