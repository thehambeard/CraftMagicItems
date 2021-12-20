using Kingmaker.PubSubSystem;

namespace CraftMagicItems.Interfaces
{
    public interface IItemFilterChanged : ISubscriber, IGlobalSubscriber
    {
         void HandleFilterChange(int index);
    }
}
