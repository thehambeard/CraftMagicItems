using Kingmaker.UI.MVVM._PCView.Slots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ModMaker.Utility;
using Owlcat.Runtime.UI.Controls.Button;
using Kingmaker.PubSubSystem;
using CraftMagicItems.Interfaces;

namespace CraftMagicItems.UI
{
    class FilterGroupView : MonoBehaviour
    {
        private List<Transform> _buttons = new List<Transform>();
        private OwlcatMultiButton _selected;
        public void Initialize()
        {
            GameObject.DestroyImmediate(transform.GetComponent<ItemsFilterPCView>());
            transform.Find("FilterPCView/SwitchBar/Ingredient").SafeDestroy();
            transform.Find("FilterPCView/SwitchBar/Notable").SafeDestroy();
            transform.Find("FilterPCView/SwitchBar/NonUsable").SafeDestroy();
            transform.Find("FilterPCView/SwitchBar").localPosition = new Vector3(68f, 0, 0);
            foreach (var c in transform.GetComponentsInChildren<ItemsFilterEntityPCView>())
            {
                _buttons.Add(c.transform);
                var button = c.GetComponent<OwlcatMultiButton>();
                button.OnLeftClick.RemoveAllListeners();
                button.OnLeftClick.AddListener(() => OnClick(button));
                GameObject.DestroyImmediate(c);
            }
        }

        public void OnClick(OwlcatMultiButton button)
        {
            if (_selected) _selected.SetSelected(false);
            _selected = button;
            button.SetSelected(true);
            EventBus.RaiseEvent((Action<IItemFilterChanged>)(h => h.HandleFilterChange(button.transform.GetSiblingIndex())));
        }
    }
}
