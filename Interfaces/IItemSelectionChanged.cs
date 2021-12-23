using Kingmaker.PubSubSystem;
using CraftMagicItems.UI;

namespace CraftMagicItems.Interfaces
{
    public interface IItemSelectionChanged : ISubscriber, IGlobalSubscriber
    {
        void HandleItemSelectionChanged(ItemSlot itemSlot);
    }
}

