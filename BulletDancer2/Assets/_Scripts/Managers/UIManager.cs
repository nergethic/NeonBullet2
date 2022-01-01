using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Player.UI
{
    public class UIManager : SceneManager
    {
        [SerializeField] List<UIPanel> uiPanels;
        private UIPanel activePanel;
        public UIPanel ActivePanel => activePanel;
        public bool IsPanelActive => activePanel != null;

        public override void Init(MasterSystem masterSystem, SceneManagerData data)
        {
            base.Init(masterSystem, data);
            data.playerController.InitializeUIManager(this);
            ChangeInitializationState(ManagerInitializationState.COMPLETED);
        }

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
    }
}
