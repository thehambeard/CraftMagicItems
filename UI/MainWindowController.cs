using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker;
using Kingmaker.PubSubSystem;
using ModMaker;
using ModMaker.Utility;
using UnityEngine;
using static CraftMagicItems.Main;

namespace CraftMagicItems.UI
{
    class MainWindowController : IModEventHandler, IAreaLoadingStagesHandler
    {
        public int Priority => 100;

        public MainWindowView MainWindow { get; private set; }
        public void Attach()
        {
            if (!MainWindow)
                MainWindow = MainWindowView.CreateObject();
        }

        public void Detach()
        {
            MainWindow.SafeDestroy();
            MainWindow = null;
        }

        public void Update()
        {
            Detach();
            Attach();
        }

        public void Clear()
        {
            Transform quickInventory;
            while (quickInventory = Game.Instance.UI.Common.transform.Find("CraftMagicRoot"))
            {
                quickInventory.SafeDestroy();
            }
            quickInventory = null;
        }

        public void HandleModDisable()
        {
            Detach();
            Mod.Core.MainWindowController = null;
            EventBus.Unsubscribe(this);
        }

        public void HandleModEnable()
        {
            Attach();
            Mod.Core.MainWindowController = this;
            EventBus.Subscribe(this);
        }

        public void OnAreaLoadingComplete()
        {
            Attach();
        }

        public void OnAreaScenesLoaded()
        {
            throw new NotImplementedException();
        }
    }
}
