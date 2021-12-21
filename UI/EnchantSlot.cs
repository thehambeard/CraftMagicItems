using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftMagicItems.UI
{
    class EnchantSlot
    {
        public enum Type
        {
            All,
            Weapon,
            Armor,
            Accessory,
            Usable
        }

        public EnchantSlot.Type EnchantType;
        public string DisplayName;
        public string Description;
    }
}
