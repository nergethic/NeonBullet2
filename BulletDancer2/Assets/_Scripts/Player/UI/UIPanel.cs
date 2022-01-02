using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIPanel : MonoBehaviour
{
    protected MasterSystem _masterSystem;
    public void Initialize(MasterSystem masterSystem) => _masterSystem = masterSystem;
    public PanelType panelType;
    public void ShowPanel() => gameObject.SetActive(true);
    public void HidePanel() => gameObject.SetActive(false);
}

public enum PanelType
{
    Crafting,
    Inventory,
    Options
}