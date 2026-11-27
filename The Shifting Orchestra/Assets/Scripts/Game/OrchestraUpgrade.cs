using System;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "OrchestraUpgrade", menuName = "ScriptableObjects/OrchestraUpgrade")]
public class OrchestraUpgrade : ScriptableObject
{
    [SerializeField, DontCreateProperty] public Enums.OrchestraUpgrade Upgrade;
    [SerializeField, DontCreateProperty] public float UpgradeValue;
    [SerializeField, DontCreateProperty] public int BaseUpgradeCost;
    [SerializeField] public int AmountUpgrades;
    [SerializeField, CreateProperty] public string UpgradeText;
    [SerializeField, CreateProperty] public string MaxUpgradeText;
    [SerializeField, CreateProperty] public Sprite UpgradeImage;
    [CreateProperty] public string UpgradeHeadline => EnumValues.OrchestraUpgradeHeadlines[Upgrade];
    [CreateProperty] public string UpgradeButtonText => $"Upgrade {DynamicUpgradeCost}€";
    [CreateProperty] public string UpgradesLeftText => $"Upgrades left: {AmountUpgradesLeft}";
    [HideInInspector, DontCreateProperty] public float upgradeMultiplier = 1;
    [HideInInspector] public int DynamicUpgradeCost => Mathf.FloorToInt(BaseUpgradeCost * upgradeMultiplier);
    [HideInInspector, CreateProperty] public int AmountUpgradesLeft;
    [CreateProperty] public bool PlayerHasEnoughMoneyToBuyUpgrade => RunManager.Instance.PlayerHasEnoughMoney(DynamicUpgradeCost) && AmountUpgradesLeft > 0;
    [CreateProperty] public StyleEnum<DisplayStyle> DisplayNormalText => (StyleEnum<DisplayStyle>)((AmountUpgradesLeft > 0) ? DisplayStyle.Flex : DisplayStyle.None);
    [CreateProperty] public StyleEnum<DisplayStyle> DisplayMaxText => (StyleEnum<DisplayStyle>)((AmountUpgradesLeft == 0) ? DisplayStyle.Flex : DisplayStyle.None);

    public void Reset()
    {
        AmountUpgradesLeft = AmountUpgrades;
    }
}
