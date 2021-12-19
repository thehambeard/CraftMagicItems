using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.PubSubSystem;
using UnityEngine;
using CraftMagicItems.Interfaces;
using UnityEngine.UI;

namespace CraftMagicItems.UI
{
    public class CenterView : MonoBehaviour, IItemSelectionChanged
    {
        private Image _centerIcon;
        public void HandleSelectionChanged(ItemSlot itemSlot)
        {
            _centerIcon.sprite = itemSlot.Icon;
            Main.Mod.Debug(itemSlot.DisplayName);
        }

        public void Initialize()
        {
            EventBus.Subscribe(this);
            transform.name = "CenterView";
            _centerIcon = transform.Find("SpellSlot/IconImage/").GetComponent<Image>();
        }
    }
}
