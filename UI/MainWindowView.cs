using Kingmaker;
using Kingmaker.UI.MVVM._PCView.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ModMaker.Utility;
using TMPro;
using Kingmaker.Localization;
using Owlcat.Runtime.UI.VirtualListSystem;
using Kingmaker.UI.MVVM._PCView.Slots;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Kingmaker.UI.Common;

namespace CraftMagicItems.UI
{
    class MainWindowView : MonoBehaviour
    {
        public static MainWindowView CreateObject()
        {
            var staticCanvas = Game.Instance.UI.Canvas;
            
            if (!staticCanvas)
                return null;

            var vendorPCView = staticCanvas.transform.Find("VendorPCView");
            var partyPCView = staticCanvas.transform.Find("PartyPCView");
            var metaMagic = staticCanvas.transform.Find("ServiceWindowsPCView/SpellbookPCView/SpellbookScreen/MainContainer/MetamagicContainer/Metamagic/MetamagicSelector/SpellSlot/");

            if (!vendorPCView || !metaMagic)
                return null;

            var mainWindow = GameObject.Instantiate(vendorPCView, staticCanvas.transform, false);
            mainWindow.name = "CraftMagicRoot";

            var mainWindowContent = mainWindow.Find("MainContent");

            var vendorPCV = mainWindow.GetComponent<VendorPCView>();
            var vendorSlotPrefab = GameObject.Instantiate(vendorPCV.m_VendorSlotPrefab.transform);
            GameObject.DestroyImmediate(vendorPCV);

            mainWindowContent.Find("Doll/").SafeDestroy();
            mainWindowContent.Find("PlayerStash/").SafeDestroy();
            mainWindowContent.Find("PlayerExchangePart/").SafeDestroy();
            mainWindowContent.Find("VendorExchangePart/").SafeDestroy();

            var dealBlock = mainWindowContent.Find("DealBlock");
            dealBlock.localPosition = new Vector3(8f, dealBlock.localPosition.y, 0f);
            dealBlock.Find("DealButton").GetComponentInChildren<TextMeshProUGUI>().text = "Craft!";

            var vendorBlock = mainWindow.Find("MainContent/VendorBlock/");

            GameObject.DestroyImmediate(vendorBlock.GetComponentInChildren<LocalizedUIText>());
            GameObject.DestroyImmediate(vendorBlock.GetComponentInChildren<VirtualListGridVertical>());
            GameObject.DestroyImmediate(vendorBlock.GetComponentInChildren<ItemSlotsGroupView>());
            GameObject.DestroyImmediate(vendorBlock.GetComponentInChildren<ScrollHandler>());

            vendorBlock.Find("PC_FilterBlock/FilterPCView/").gameObject.SetActive(true);
            var vendorBlockText = vendorBlock.Find("VendorHeader").GetComponent<TextMeshProUGUI>();
            vendorBlockText.text = "<voffset=0em><font=\"Saber_Dist32\"><color=#672B31><size=140%>I</size></color></font></voffset>tem to Enchant";

            var enchantBlock = GameObject.Instantiate(vendorBlock, mainWindowContent, false);
            var enchantBlockText = enchantBlock.Find("VendorHeader").GetComponent<TextMeshProUGUI>();
            enchantBlockText.text = "<voffset=0em><font=\"Saber_Dist32\"><color=#672B31><size=140%>E</size></color></font></voffset>nchantments";
            enchantBlock.localPosition = new Vector3(266f, -17.8f, 0f);

            var mainHeader = GameObject.Instantiate(enchantBlockText.transform, mainWindow, false);
            mainHeader.localPosition = new Vector3(0f, 475f, 0f);
            var mainHeaderText = mainHeader.GetComponent<TextMeshProUGUI>();
            mainHeaderText.text = "<voffset=0em><font=\"Saber_Dist32\"><color=#672B31><size=140%>C</size></color></font></voffset>raft Magic Item";

            var resultWheel = GameObject.Instantiate(metaMagic, mainWindowContent, false);
            resultWheel.localPosition = new Vector3(9.4f, 49.2f, 0f);
            var spellSlot = resultWheel.gameObject.AddComponent<CenterView>();
            spellSlot.Initialize();

            var scrollRectTransform = vendorBlock.Find("VendorStashScrollView/");
            var itemSlotsView = scrollRectTransform.gameObject.AddComponent<ItemSlotsView>();
            itemSlotsView.Initialize(vendorSlotPrefab);
            AddScroll(scrollRectTransform);

            var enchScrollRect = enchantBlock.Find("VendorStashScrollView");
            var enchSlotsView = enchScrollRect.gameObject.AddComponent<EnchantSlotsView>();
            enchSlotsView.Initialize(vendorSlotPrefab);
            AddScroll(enchScrollRect);

            var pcFilterBlock = vendorBlock.Find("PC_FilterBlock/").gameObject.AddComponent<FilterGroupView>();
            pcFilterBlock.Initialize();

            mainWindow.gameObject.SetActive(true);
            mainWindow.SetSiblingIndex(partyPCView.GetSiblingIndex() + 1);
            return mainWindow.gameObject.AddComponent<MainWindowView>();
        }

        private static void AddScroll(Transform scrollRectTransform)
        {
            GameObject.DestroyImmediate(scrollRectTransform.GetComponentInChildren<DragTracker>());
            var scrollRect = scrollRectTransform.gameObject.AddComponent<ScrollRectExtended>();
            scrollRect.viewport = (RectTransform)scrollRectTransform.Find("Viewport");
            scrollRect.content = (RectTransform)scrollRectTransform.Find("Viewport/Content/");
            var scrollBar = scrollRectTransform.GetComponentInChildren<Scrollbar>();
            scrollBar.direction = Scrollbar.Direction.BottomToTop;
            scrollRect.verticalScrollbar = scrollBar;
            scrollRect.scrollSensitivity = 35f;
            scrollRect.movementType = ScrollRectExtended.MovementType.Clamped;
        }
    }
}
