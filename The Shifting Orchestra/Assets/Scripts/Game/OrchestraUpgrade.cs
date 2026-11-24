using System;
using Unity.Properties;
using UnityEngine;

[CreateAssetMenu(fileName = "OrchestraUpgrade", menuName = "ScriptableObjects/OrchestraUpgrade")]
public class OrchestraUpgrade : ScriptableObject
{
    [SerializeField, DontCreateProperty] public Enums.OrchestraUpgrade Upgrade;
    [SerializeField, DontCreateProperty] public float UpgradeValue;
    [SerializeField, DontCreateProperty] public int BaseUpgradeCost;
    [SerializeField, CreateProperty] public string UpgradeText;
    [SerializeField, CreateProperty] public Sprite UpgradeImage;
    [CreateProperty] public string UpgradeHeadline => EnumValues.OrchestraUpgradeHeadlines[Upgrade];
    [CreateProperty] public string UpgradeButtonText => $"Upgrade {DynamicUpgradeCost}€";
    [HideInInspector, DontCreateProperty] public float upgradeMultiplier = 1;
    [HideInInspector] public int DynamicUpgradeCost => Mathf.FloorToInt(BaseUpgradeCost * upgradeMultiplier);
    [CreateProperty] public bool PlayerHasEnoughMoneyToBuyUpgrade => RunManager.Instance.PlayerHasEnoughMoney(DynamicUpgradeCost);
}
