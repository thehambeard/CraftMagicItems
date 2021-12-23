using CraftMagicItems.UI;
using Kingmaker.PubSubSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftMagicItems.Interfaces
{
    interface IEnchantSelectionChanged : ISubscriber, IGlobalSubscriber
    {
        void HandleEnchantSelectionChanged(List<EnchantSlot> slots);
    }
}
