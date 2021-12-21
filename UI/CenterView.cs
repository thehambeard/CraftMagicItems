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

namespace CraftMagicItems.UI
{
    public class CenterView : MonoBehaviour, IItemSelectionChanged
    {
        private Image _centerIcon;
        private Dictionary<Transform, ButtonWrapper> _transformDict = new Dictionary<Transform, ButtonWrapper>();
        private Transform _selected;
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

            var multiButtonTransform = Game.Instance.UI.Canvas.transform.Find("VendorPCView/MainContent/VendorBlock/PC_FilterBlock/FilterPCView/SwitchBar/NonUsable/");
            var textMesh = Game.Instance.UI.Canvas.transform.Find("VendorPCView/MainContent/VendorBlock/VendorHeader/").GetComponent<TextMeshProUGUI>();

            var temp = new GameObject("Text");
            temp.AddComponent<RectTransform>();
            foreach (var c in gameObject.GetComponentsInChildren<SpellbookMetamagicSlotPCView>())
            {
                var button = GameObject.Instantiate(multiButtonTransform, c.transform, false);
                var multiButton = button.GetComponent<OwlcatMultiButton>();
                GameObject.DestroyImmediate(button.GetComponent<ItemsFilterEntityPCView>());
                button.localPosition = new Vector3(0f, 0f, 0f);
                button.localScale = new Vector3(1.1f, 1.1f, 1.1f);

                var tObject = GameObject.Instantiate(temp, button, false);
                tObject.transform.localPosition = new Vector3(-1f, -2f);
                var text = tObject.AddComponent<TextMeshProUGUI>();
                text.alignment = TextAlignmentOptions.Center;

                multiButton.OnLeftClick.RemoveAllListeners();
                multiButton.OnLeftClick.AddListener(() => OnLeftClick(button));

                _transformDict.Add(button, new ButtonWrapper(text, button.parent.GetSiblingIndex(), multiButton));
                GameObject.DestroyImmediate(c);
            }
        }

        private void OnLeftClick(Transform transform)
        {
            if (_selected) _transformDict[_selected].Button.SetSelected(false);
            _selected = transform;
            _transformDict[transform].Button.SetSelected(true);
        }

        internal class ButtonWrapper
        {
            public TextMeshProUGUI TextMesh { get; set; }
            public OwlcatMultiButton Button { get; set; }
            public int SiblingIndex { get; set; }
            public int Plus
            {
                get 
                {
                    switch(SiblingIndex)
                    {
                        case 0:
                            return 1;
                        case 1:
                            return 2;
                        case 2:
                            return 8;
                        case 3:
                            return 3;
                        case 4:
                            return 7;
                        case 5:
                            return 4;
                        case 6:
                            return 6;
                        case 7:
                            return 5;
                    }
                    return 0;
                }
            }

            public ButtonWrapper(TextMeshProUGUI textmesh, int siblingindex, OwlcatMultiButton button)
            {
                Button = button;
                SiblingIndex = siblingindex;
                TextMesh = textmesh;

                TextMesh.text = $"+{Plus}";
            }
        }
    }
}
