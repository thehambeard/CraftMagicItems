using Kingmaker.PubSubSystem;

namespace CraftMagicItems.Interfaces
{
    public interface IItemFilterChanged : ISubscriber, IGlobalSubscriber
    {
         void HandleItemFilterChange(int index);
    }
}
