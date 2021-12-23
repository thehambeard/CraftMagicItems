using CraftMagicItems.Interfaces;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.PubSubSystem;
using Kingmaker.ResourceLinks;
using Kingmaker.UI.Common;
using Kingmaker.UI.MVVM._PCView.Slots;
using Kingmaker.UI.MVVM._PCView.Vendor;
using Kingmaker.UI.Tooltip;
using ModMaker.Utility;
using Owlcat.Runtime.UI.Controls.Button;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace CraftMagicItems.UI
{
    class EnchantSlotsView : MonoBehaviour, IItemFilterChanged
    {
        private Transform _prefab;
        private Transform _content;
        private HashSet<OwlcatMultiButton> _selected = new HashSet<OwlcatMultiButton>();
        private int _currentFilterIndex = 0;
        private Dictionary<Transform, EnchantSlot> _transformDict = new Dictionary<Transform, EnchantSlot>();
        /* Transfrom Stucture of prefab
         * Componets: VendorSlotPCView, ItemSlotPCView, OwlCatMultiButton, ObservableEnableTrigger, UniRx.Triggers.ObservableEnableTrigger,
         *   UniRx.Triggers.ObservableBeginDragTrigger, UniRx.Triggers.ObservableDragTrigger, UniRx.Triggers.ObservableEndDragTrigger, UniRx.Triggers.ObservableDropTrigger
         * Topline 
         * Bottomline 
         * BodyCanNotEquip 
         * Highlight (DragHandler)
         * Slot
         * --Frame
         * --Item
         * ----ItemCanNotEquip 
         * ----NeedCheckLayer 
         * ----MagicLayer 
         * ------MagicLayerFX 
         * ----NotableLayer
         * ------NotableLayerFX
         * ----Icon
         * ----Feather
         * ----Count
         * DisplayName
         * Type
         * Price
         * --Icon
         * Weight
         * --Icon
         */

        private Sprite _weaponSprite = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("2bc77aa47f97de348aefcf03ec8fa43b").Icon;
        private Sprite _armorSprite = ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("d326c3c61a84c6f40977c84fab41503d").Icon;
        private Sprite _accessorySprite = ResourcesLibrary.TryGetBlueprint<BlueprintItemEquipmentBelt>("b6af4c1834999e74497b41588d1071cd").Icon;

        public void Initialize(Transform prefab)
        {
            _prefab = GameObject.Instantiate(prefab);

            _content = transform.Find("Viewport/Content/");
            ((RectTransform)_content).pivot = new Vector2(0f, 1f);
            var csfe = ((RectTransform)_content).gameObject.AddComponent<ContentSizeFitterExtended>();
            csfe.m_VerticalFit = ContentSizeFitterExtended.FitMode.MinSize;
            transform.Find("Viewport/Placeholder").SafeDestroy();

            var vlg = _content.gameObject.AddComponent<VerticalLayoutGroup>();
            vlg.childForceExpandHeight = false;
            vlg.childForceExpandWidth = false;
            vlg.childControlHeight = false;
            vlg.childControlWidth = false;
            vlg.padding = new RectOffset(0, 0, 0, 0);

            _prefab.Find("BodyCanNotEquip").SafeDestroy();
            _prefab.Find("Price").SafeDestroy();
            _prefab.Find("Weight").SafeDestroy();
            
            _prefab.Find("Slot/Item/ItemCanNotEquip").gameObject.SetActive(false);
            _prefab.Find("Slot/Item/NeedCheckLayer").gameObject.SetActive(false);
            _prefab.Find("Slot/Item/MagicLayer").gameObject.SetActive(true);
            _prefab.Find("Slot/Item/NotableLayer").gameObject.SetActive(false);
            _prefab.Find("Slot/Item/Feather").gameObject.SetActive(false);
            _prefab.Find("Slot/Item/Count").gameObject.SetActive(false);
            _prefab.Find("Slot/Item/Icon").gameObject.SetActive(true);

            GameObject.DestroyImmediate(_prefab.GetComponent<VendorSlotPCView>());
            GameObject.DestroyImmediate(_prefab.GetComponent<ItemSlotPCView>());
            GameObject.DestroyImmediate(_prefab.GetComponent<ObservableEnableTrigger>());
            GameObject.DestroyImmediate(_prefab.GetComponent<ObservableBeginDragTrigger>());
            GameObject.DestroyImmediate(_prefab.GetComponent<ObservableDragTrigger>());
            GameObject.DestroyImmediate(_prefab.GetComponent<ObservableEnableTrigger>());
            GameObject.DestroyImmediate(_prefab.GetComponent<ObservableEndDragTrigger>());
            GameObject.DestroyImmediate(_prefab.GetComponent<ObservableDropTrigger>());

            EventBus.Subscribe(this);
        }

        public void Awake()
        {
            _content = transform.Find("Viewport/Content/");
            
            LoadAllEnchants();
            SortTransformsByName();
        }

        private void SortTransformsByName()
        {
            int count = 0;
            foreach (var t in _transformDict.OrderBy(x => x.Value.DisplayName))
            {
                t.Key.SetSiblingIndex(count++);
            }
        }

        private void LoadAllEnchants()
        {
            #region WEAPONENCHANTS
            BlueprintWeaponEnchantment weaponBP = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>("a36ad92c51789b44fa8a1c5c116a1328");
            EnchantSlot slot = new EnchantSlot()
            {
                Blueprint = weaponBP,
                DisplayName = "Agile",
                Description = weaponBP.Description,
                EnchantType = Enums.ItemType.Weapon,
                Cost = 1000,
                CL = 1,
                Grouping = EnchantSlot.ExclusiveGroup.None,
                Sprite = _weaponSprite

            };
            BuildEnchant(slot);
            weaponBP = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>("57315bc1e1f62a741be0efde688087e9");
            slot = new EnchantSlot()
            {
                Blueprint = weaponBP,
                DisplayName = "Anarchic",
                Description = weaponBP.Description,
                EnchantType = Enums.ItemType.Weapon,
                Cost = 1000,
                CL = 1,
                Grouping = EnchantSlot.ExclusiveGroup.LawOrChaos,
                Sprite = _weaponSprite
            };
            BuildEnchant(slot);
            weaponBP = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>("0ca43051edefcad4b9b2240aa36dc8d4");
            slot = new EnchantSlot()
            {
                Blueprint = weaponBP,
                DisplayName = "Axiomatic",
                Description = weaponBP.Description,
                EnchantType = Enums.ItemType.Weapon,
                Cost = 1000,
                CL = 1,
                Grouping = EnchantSlot.ExclusiveGroup.LawOrChaos,
                Sprite = _weaponSprite
            };
            BuildEnchant(slot);
            weaponBP = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>("d42fc23b92c640846ac137dc26e000d4");
            slot = new EnchantSlot()
            {
                Blueprint = weaponBP,
                DisplayName = "Weapon +1",
                Description = weaponBP.Description,
                EnchantType = Enums.ItemType.Weapon,
                Cost = 1000,
                CL = 1,
                Grouping = EnchantSlot.ExclusiveGroup.Plus,
                Sprite = _weaponSprite
            };
            BuildEnchant(slot);
            weaponBP = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>("eb2faccc4c9487d43b3575d7e77ff3f5");
            slot = new EnchantSlot()
            {
                Blueprint = weaponBP,
                DisplayName = "Weapon +2",
                Description = weaponBP.Description,
                EnchantType = Enums.ItemType.Weapon,
                Cost = 1000,
                CL = 1,
                Grouping = EnchantSlot.ExclusiveGroup.Plus,
                Sprite = _weaponSprite
            };
            BuildEnchant(slot);
            weaponBP = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>("80bb8a737579e35498177e1e3c75899b");
            slot = new EnchantSlot()
            {
                Blueprint = weaponBP,
                DisplayName = "Weapon +3",
                Description = weaponBP.Description,
                EnchantType = Enums.ItemType.Weapon,
                Cost = 1000,
                CL = 1,
                Grouping = EnchantSlot.ExclusiveGroup.Plus,
                Sprite = _weaponSprite
            };
            BuildEnchant(slot);
            weaponBP = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>("783d7d496da6ac44f9511011fc5f1979");
            slot = new EnchantSlot()
            {
                Blueprint = weaponBP,
                DisplayName = "Weapon +4",
                Description = weaponBP.Description,
                EnchantType = Enums.ItemType.Weapon,
                Cost = 1000,
                CL = 1,
                Grouping = EnchantSlot.ExclusiveGroup.Plus,
                Sprite = _weaponSprite
            };
            BuildEnchant(slot);
            weaponBP = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>("bdba267e951851449af552aa9f9e3992");
            slot = new EnchantSlot()
            {
                Blueprint = weaponBP,
                DisplayName = "Weapon +5",
                Description = weaponBP.Description,
                EnchantType = Enums.ItemType.Weapon,
                Cost = 1000,
                CL = 1,
                Grouping = EnchantSlot.ExclusiveGroup.Plus,
                Sprite = _weaponSprite
            };
            BuildEnchant(slot);
            weaponBP = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>("0326d02d2e24d254a9ef626cc7a3850f");
            slot = new EnchantSlot()
            {
                Blueprint = weaponBP,
                DisplayName = "Weapon +6",
                Description = weaponBP.Description,
                EnchantType = Enums.ItemType.Weapon,
                Cost = 1000,
                CL = 1,
                Grouping = EnchantSlot.ExclusiveGroup.Plus,
                Sprite = _weaponSprite
            };
            BuildEnchant(slot);
            #endregion
            BlueprintArmorEnchantment armorBP = ResourcesLibrary.TryGetBlueprint<BlueprintArmorEnchantment>("dd0e096412423d646929d9b945fd6d4c");
            slot = new EnchantSlot()
            {
                Blueprint = armorBP,
                DisplayName = "Acid Resist 10",
                Description = armorBP.Description,
                EnchantType = Enums.ItemType.Armor,
                Cost = 1000,
                CL = 1,
                Grouping = EnchantSlot.ExclusiveGroup.None,
                Sprite = _armorSprite
            };
            BuildEnchant(slot);
            BlueprintEquipmentEnchantment equipBP = ResourcesLibrary.TryGetBlueprint<BlueprintEquipmentEnchantment>("3619b3ddc8e156a498b7fd5db02457b8");
            slot = new EnchantSlot()
            {
                Blueprint = equipBP,
                DisplayName = "Charisma +1",
                Description = armorBP.Description,
                EnchantType = Enums.ItemType.Accessory,
                Cost = 1000,
                CL = 1,
                Grouping = EnchantSlot.ExclusiveGroup.Charisma,
                Sprite = _accessorySprite
            };
            BuildEnchant(slot);
            equipBP = ResourcesLibrary.TryGetBlueprint<BlueprintEquipmentEnchantment>("f945413e1b120294780ba9de26bd2f7a");
            slot = new EnchantSlot()
            {
                Blueprint = equipBP,
                DisplayName = "Charisma +2",
                Description = armorBP.Description,
                EnchantType = Enums.ItemType.Accessory,
                Cost = 1000,
                CL = 1,
                Grouping = EnchantSlot.ExclusiveGroup.Charisma,
                Sprite = _accessorySprite
            };
            BuildEnchant(slot);
        }

        private void BuildEnchant(EnchantSlot slot)
        {
            var displayName = _prefab.Find("DisplayName").GetComponent<TextMeshProUGUI>();
            var description = _prefab.Find("Type").GetComponent<TextMeshProUGUI>();
            var icon = _prefab.Find("Slot/Item/Icon").GetComponent<Image>();

            displayName.text = slot.DisplayName;
            description.text = slot.Blueprint.Description;
            icon.sprite = slot.Sprite;

            var transform = GameObject.Instantiate(_prefab, _content, false);
            transform.name = "EnchantSlotView";
            var tooltip = transform.gameObject.AddComponent<TooltipTrigger>();
            tooltip.enabled = true;
            tooltip.SetNameAndDescription(displayName.text, description.text);
            var multiButton = transform.GetComponent<OwlcatMultiButton>();
            multiButton.OnLeftClick.RemoveAllListeners();
            multiButton.OnLeftClick.AddListener(() => OnLeftClick(transform));
            slot.DisableLayer = transform.GetChild(3).GetChild(1).GetChild(0);
            transform.gameObject.SetActive((int)slot.EnchantType == _currentFilterIndex);
            _transformDict.Add(transform, slot);
        }

        private void OnLeftClick(Transform transform)
        {
            var button = transform.GetComponent<OwlcatMultiButton>();
            if (!_selected.Contains(button) && _transformDict[transform].Enabled)
            {
                _selected.Add(button);
                button.SetSelected(true);
                _transformDict[transform].Selected = true;
            }
            else if(_selected.Contains(button))
            {
                _selected.Remove(button);
                button.SetSelected(false);
                _transformDict[transform].Selected = false;
            }
            EnsureValidSelections();
            EventBus.RaiseEvent((Action<IEnchantSelectionChanged>)(h => h.HandleEnchantSelectionChanged(_transformDict.Where(x => x.Value.Selected).Select(x => x.Value).ToList())));
        }

        public void EnsureValidSelections()
        {
            foreach (var tran in _transformDict)
            {
                tran.Value.Enabled = true;

                foreach (var select in _selected)
                {
                   if (_transformDict[select.transform].Grouping == tran.Value.Grouping && tran.Key != select.transform)
                        tran.Value.Enabled = false;
                }
                tran.Value.DisableLayer.gameObject.SetActive(!tran.Value.Enabled);
            }
        }

        public void HandleItemFilterChange(int index)
        {
            if (index == _currentFilterIndex)
                return;
            _currentFilterIndex = index;
            foreach (var t in _transformDict)
            {
                t.Value.Selected = false;
                if (t.Value.EnchantType == (Enums.ItemType)index)
                    t.Key.gameObject.SetActive(true);
                else
                    t.Key.gameObject.SetActive(false);
            }

            _selected = new HashSet<OwlcatMultiButton>();
            EnsureValidSelections();
        }
    }
}
