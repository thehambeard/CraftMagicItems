using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Ecnchantments;
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
    class EnchantSlotsView : MonoBehaviour
    {
        private Transform _prefab;
        private Transform _content;
        private OwlcatMultiButton _selected;
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

        private List<BlueprintWeaponEnchantment> _weaponEnchants = new List<BlueprintWeaponEnchantment>()
        {
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>("a36ad92c51789b44fa8a1c5c116a1328"),
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>("57315bc1e1f62a741be0efde688087e9"),
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>("0ca43051edefcad4b9b2240aa36dc8d4"),
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>("ee71cc8848219c24b8418a628cc3e2fa"),
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>("78cf9fabe95d3934688ea898c154d904"),
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>("73d30862f33cc754bb5a5f3240162ae6"),
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>("e5cb46a0a658b0a41854447bea32d2ee"),
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>("b6948040cdb601242884744a543050d4"),
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>("dcecb5f2ffacfd44ead0ed4f8846445d"),
            ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>("1e25d1f515c867d40b9c0642e0b40ec2"),
        };

        private List<BlueprintArmorEnchantment> _armorEnchants = new List<BlueprintArmorEnchantment>()
        {
            ResourcesLibrary.TryGetBlueprint<BlueprintArmorEnchantment>("dd0e096412423d646929d9b945fd6d4c"),
            ResourcesLibrary.TryGetBlueprint<BlueprintArmorEnchantment>("09e0be00530efec4693a913d6a7efe23"),
            ResourcesLibrary.TryGetBlueprint<BlueprintArmorEnchantment>("1346633e0ff138148a9a925e330314b5"),
            ResourcesLibrary.TryGetBlueprint<BlueprintArmorEnchantment>("e6fa2f59c7f1bb14ebfc429f17d0a4c6"),
            ResourcesLibrary.TryGetBlueprint<BlueprintArmorEnchantment>("933456ff83c454146a8bf434e39b1f93"),
            ResourcesLibrary.TryGetBlueprint<BlueprintArmorEnchantment>("5faa3aaee432ac444b101de2b7b0faf7"),
        };

        private Sprite _weaponSprite = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponType>("2bc77aa47f97de348aefcf03ec8fa43b").Icon;
        private Sprite _armorSprite = ResourcesLibrary.TryGetBlueprint<BlueprintArmorType>("d326c3c61a84c6f40977c84fab41503d").Icon;

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

            _prefab.gameObject.AddComponent<TooltipTrigger>();
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
            var description = _prefab.Find("Type").GetComponent<TextMeshProUGUI>();
            var icon = _prefab.Find("Slot/Item/Icon").GetComponent<Image>();
            LoadWeaponTypes(displayName, icon, description);
            LoadArmorTypes(displayName, icon, description);
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

        private void LoadWeaponTypes(TextMeshProUGUI displayName, Image icon, TextMeshProUGUI description)
        {
            foreach (var wt in _weaponEnchants)
            {
                displayName.text = wt.Name;
                description.text = wt.Description;
                icon.sprite = _weaponSprite;
                LoadTypes(displayName, icon, EnchantSlot.Type.Weapon, description);
            }
        }

        private void LoadArmorTypes(TextMeshProUGUI displayName, Image icon, TextMeshProUGUI description)
        {
            foreach (var wt in _armorEnchants)
            {
                displayName.text = wt.Name;
                description.text = wt.Description;
                icon.sprite = _armorSprite;
                LoadTypes(displayName, icon, EnchantSlot.Type.Weapon, description);
            }
        }

        private void LoadTypes(TextMeshProUGUI displayName, Image icon, EnchantSlot.Type type, TextMeshProUGUI description)
        {
            var transfom = GameObject.Instantiate(_prefab, _content, false);
            var tooltip = transfom.gameObject.AddComponent<TooltipTrigger>();
            tooltip.enabled = true;
            tooltip.SetNameAndDescription(displayName.text, description.text);
            var multiButton = transfom.GetComponent<OwlcatMultiButton>();
            multiButton.OnLeftClick.RemoveAllListeners();
            multiButton.OnLeftClick.AddListener(() => OnLeftClick(transfom));
            _transformDict.Add(transfom, new EnchantSlot() { DisplayName = displayName.text, EnchantType = type});
        }

        private void OnLeftClick(Transform transform)
        {
        }
    }
}
