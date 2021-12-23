using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.PubSubSystem;
using UnityEngine;
using CraftMagicItems.Interfaces;
using UnityEngine.UI;
using Kingmaker;
using Kingmaker.UI.MVVM._PCView.ServiceWindows.Spellbook.Metamagic;
using TMPro;
using Kingmaker.UI.MVVM._PCView.Slots;
using Owlcat.Runtime.UI.Controls.Button;
using Kingmaker.UI.Tooltip;

namespace CraftMagicItems.UI
{
    public class CenterView : MonoBehaviour, IItemSelectionChanged, IEnchantSelectionChanged
    {
        private Image _centerIcon;
        private List<RingWrapper> _ringIcons = new List<RingWrapper>();
        public void HandleEnchantSelectionChanged(List<EnchantSlot> slots)
        {
            for(int i = 0; i < _ringIcons.Count; i++)
            {
                _ringIcons[i].Image.gameObject.SetActive(false);
                _ringIcons[i].Tooltip.enabled = false;

                if(slots != null && i < slots.Count)
                {
                    _ringIcons[i].Image.sprite = slots[i].Sprite;
                    _ringIcons[i].Image.gameObject.SetActive(true);
                    _ringIcons[i].Tooltip.enabled = true;
                    _ringIcons[i].Tooltip.SetNameAndDescription(slots[i].DisplayName, slots[i].Description);
                }
            }
                
        }

        public void HandleItemSelectionChanged(ItemSlot itemSlot)
        {
            if (itemSlot == null)
            {
                _centerIcon.color = new Color(0f, 0f, 0f, 0f);
                HandleEnchantSelectionChanged(null);
            }
            else
            {
                _centerIcon.sprite = itemSlot.Icon;
                _centerIcon.color = new Color(1f, 1f, 1f, 1f);
            }
        }

        public void Initialize()
        {
            EventBus.Subscribe(this);
            transform.name = "CenterView";
            _centerIcon = transform.Find("SpellSlot/IconImage/").GetComponent<Image>();
            _centerIcon.color = new Color(0f, 0f, 0f, 0f);

            foreach (var c in gameObject.GetComponentsInChildren<SpellbookMetamagicSlotPCView>())
            {
                var image = c.transform.Find("Image").GetComponent<Image>();
                image.transform.localScale = new Vector3(.65f, .65f, .65f);
                var tooltip = image.transform.parent.gameObject.AddComponent<TooltipTrigger>();
                tooltip.enabled = false;
                _ringIcons.Add(new RingWrapper() { Image = image, Tooltip = tooltip });
                GameObject.DestroyImmediate(c);
            }
        }

        internal class RingWrapper
        {
            public Image Image;
            public TooltipTrigger Tooltip;
        }
    }
}
