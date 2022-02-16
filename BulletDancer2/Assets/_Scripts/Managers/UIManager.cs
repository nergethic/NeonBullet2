using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Player.UI
{
    public class UiManager : SceneManager
    {
        [SerializeField] List<UIPanel> uiPanels;
        [SerializeField] PlayerStatusBar healthBar;
        [SerializeField] PlayerStatusBar energyBar;
        private UIPanel activePanel;
        public UIPanel ActivePanel => activePanel;
        public bool IsPanelActive => activePanel != null;

        public override void Init(MasterSystem masterSystem, SceneManagerData data)
        {
            base.Init(masterSystem, data);
            data.playerController.InitializeUIManager(this);
            data.player.InitializeUIManager(this);
            InitPlayerStatusBars();
            foreach (var panel in uiPanels)
            {
                panel.Initialize(masterSystem);
            }
            ChangeInitializationState(ManagerInitializationState.COMPLETED);
        }

        public void UpdateEnergyBar(int change) => energyBar.UpdateStatusBar(change);

        public void UpdateHealthBar(int change) => healthBar.UpdateStatusBar(change);

        public void ShowPanel(PanelType type)
        {
            foreach (var panel in uiPanels)
            {
                if (panel.panelType == type)
                {
                    panel.ShowPanel();
                    activePanel = panel;
                    break;
                }
            }
        }
        
        public void HidePanel()
        {
            ActivePanel?.HidePanel();
            activePanel = null;
        }

        public override void Tick(float dt){}

        private void InitPlayerStatusBars()
        {
            UpdateEnergyBar(data.player.Energy);
            UpdateHealthBar(data.player.Health);
        }
    }
}
