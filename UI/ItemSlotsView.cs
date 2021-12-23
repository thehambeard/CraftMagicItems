using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ModMaker.Utility;
using Kingmaker.UI.MVVM._PCView.Vendor;
using Kingmaker.UI.MVVM._PCView.Slots;
using UniRx.Triggers;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints;
using TMPro;
using UnityEngine.UI;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Equipment;
using Owlcat.Runtime.UI.Controls.Button;
using CraftMagicItems.Interfaces;
using Kingmaker.PubSubSystem;
using Kingmaker.UI.Common;

namespace CraftMagicItems.UI
{
    class ItemSlotsView : MonoBehaviour, IItemFilterChanged
    {
        private Transform _prefab;
        private Transform _content;
        private OwlcatMultiButton _selected;
        private int _currentFilterIndex = 0;
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

        private List<BlueprintWeaponType> _weaponTypes = new List<BlueprintWeaponType>()
        {
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("b1cbf457fd471d148b39ae56667f405a"), //bardiche
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("d2fe2c5516b56f04da1d5ea51ae3ddfe"), //Bastard Sword
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("2bc77aa47f97de348aefcf03ec8fa43b"), //Battle Axe
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("26aa0672af2c7d84ba93bec37758c712"), //Club
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("1ac79088a7e5dde46966636a3ac71c35"), //Comp Longbow
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("011f6f86a0b16df4bbf7f40878c3e80b"), //Comp Shortbow
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("07cc1a7fceaee5b42b3e43da960fe76d"), //Dagger
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("f415ae950523a7843a74d7780dd551af"), //Dart
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("87d76c7534506a546a065731ee6a38cd"), //Double Axe
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("e95efb85a0912a7489be69d5f03a5308"), //Double Sword
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("a6f7e3dc443ff114ba68b4648fd33e9f"), //Dueling Sword
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("0ec97c08fdf87e44f8f16ba87b511743"), //Dwarf Urgrosh
            //ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("c4e95e3c489721e4382fd8514a522f9d"), //Dwarf Urgrosh Spear
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("a6925f5f897801449a648d865637e5a0"), //Dwarf axe
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("54e960775daa4714fa52e0954d8cf862"), //Earth Breaker
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("b5e6838ad2a62b146b49619bcf9f42aa"), //Elven Curved Blade
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("d516765b3c2904e4a939749526a52a9a"), //Estoc
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("1af5621e2ae551e42bd1dd6744d98639"), //Falcata
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("6ddc9acbbb6e40746a6a1671df1f7b47"), //Falchion
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("7a40899c4defec94bb9c291bde74f1a8"), //Fauchard
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("bf1e53f7442ed0c43bf52d3abe55e16a"), //Flail
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("7a14a1b224cd173449cb7ffc77d5f65c"), //Glaive
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("91645b645cf121a479c1fabc5678dcb3"), //Hooked Hammer Head
            //ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("018ad48ffd3460d47900491656d2ff26"), //Hooked Hammer Hook
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("e8059a8eac62cd74f9171d748a5ae428"), //Greataxe
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("1b8c24cd1f9358c48839bb39266468c3"), //Great Club
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("5f824fbb0766a3543bbd6ae50248688f"), //Great Sword
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("54e7473ff73910c47812fd39c897dc1a"), //Handaxe
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("36d0551b8a28587438a47fcbbf53c083"), //Heavy Crossbox
        };

        private List<BlueprintArmorType> _armorTypes = new List<BlueprintArmorType>()
        {
            ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("da1b160cd13f16a429499b96636f6ed9"), //Banded
            ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("d326c3c61a84c6f40977c84fab41503d"), //Breastplate
        };

        private List<BlueprintItemEquipment> _accessoryTypes = new List<BlueprintItemEquipment>()
        {
            ResourcesLibrary.TryGetBlueprint<BlueprintItemEquipmentBelt>("b6af4c1834999e74497b41588d1071cd"), //belt
            ResourcesLibrary.TryGetBlueprint<BlueprintItemEquipmentFeet>("1223ceb45ed647b44a04b44a9312328b"), //feet
        };

        public Dictionary<Transform, ItemSlot> TransformDict = new Dictionary<Transform, ItemSlot>();

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
            _prefab.Find("Type").SafeDestroy();
            _prefab.Find("Slot/Item/ItemCanNotEquip").gameObject.SetActive(false);
            _prefab.Find("Slot/Item/NeedCheckLayer").gameObject.SetActive(false);
            _prefab.Find("Slot/Item/MagicLayer").gameObject.SetActive(false);
            _prefab.Find("Slot/Item/NotableLayer").gameObject.SetActive(false);
            _prefab.Find("Slot/Item/Feather").gameObject.SetActive(false);
            _prefab.Find("Slot/Item/Count").gameObject.SetActive(false);

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
            var displayName = _prefab.Find("DisplayName").GetComponent<TextMeshProUGUI>();
            var icon = _prefab.Find("Slot/Item/Icon").GetComponent<Image>();
            LoadWeaponTypes(displayName, icon);
            LoadArmorTypes(displayName, icon);
            LoadAccessoryTypes(displayName, icon);
            SortTransformsByName();
        }

        private void SortTransformsByName()
        {
            int count = 0;
            foreach(var t in TransformDict.OrderBy(x => x.Value.DisplayName))
            {
                t.Key.SetSiblingIndex(count++);
            }
        }

        private void LoadWeaponTypes(TextMeshProUGUI displayName, Image icon)
        {
            foreach (var wt in _weaponTypes)
            {
                displayName.text = wt.DefaultName;
                icon.sprite = wt.Icon;
                LoadTypes(displayName, icon, Enums.ItemType.Weapon);
            }
        }

        private void LoadArmorTypes(TextMeshProUGUI displayName, Image icon)
        {
            foreach (var wt in _armorTypes)
            {
                displayName.text = wt.DefaultName;
                icon.sprite = wt.Icon;
                LoadTypes(displayName, icon, Enums.ItemType.Armor);
            }
        }

        private void LoadAccessoryTypes(TextMeshProUGUI displayName, Image icon)
        {
            foreach (var wt in _accessoryTypes)
            {
                switch(wt)
                {
                    case BlueprintItemEquipmentBelt beb:
                        displayName.text = "Belt";
                        break;
                    case BlueprintItemEquipmentFeet bef:
                        displayName.text = "Boots";
                        break;
                }
                icon.sprite = wt.Icon;
                LoadTypes(displayName, icon, Enums.ItemType.Accessory);
            }
        }

        private void LoadTypes(TextMeshProUGUI displayName, Image icon, Enums.ItemType type)
        {
            var transfom = GameObject.Instantiate(_prefab, _content, false);
            transfom.gameObject.SetActive(type == (Enums.ItemType)_currentFilterIndex);
            var multiButton = transfom.GetComponent<OwlcatMultiButton>();
            multiButton.OnLeftClick.RemoveAllListeners();
            multiButton.OnLeftClick.AddListener(() => OnLeftClick(transfom));
            TransformDict.Add(transfom, new ItemSlot() { DisplayName = displayName.text, ItemType = type, Icon = icon.sprite});
        }

        public void OnLeftClick(Transform transform)
        {
            if (TransformDict.ContainsKey(transform))
            {
                if (_selected) _selected.SetSelected(false);
                _selected = transform.GetComponent<OwlcatMultiButton>();
                _selected.SetSelected(true);
                EventBus.RaiseEvent((Action<IItemSelectionChanged>)(h => h.HandleItemSelectionChanged(TransformDict[transform])));
            }
        }

        public void HandleItemFilterChange(int index)
        {
            if (index == _currentFilterIndex)
                return;
            _currentFilterIndex = index;
            foreach (var t in TransformDict)
            {
                    if (t.Value.ItemType == (Enums.ItemType)index)
                        t.Key.gameObject.SetActive(true);
                    else
                        t.Key.gameObject.SetActive(false);
            }
            
            _selected.SetSelected(false);
            _selected = null;
            EventBus.RaiseEvent((Action<IItemSelectionChanged>)(h => h.HandleItemSelectionChanged(null)));
        }
    }
}
