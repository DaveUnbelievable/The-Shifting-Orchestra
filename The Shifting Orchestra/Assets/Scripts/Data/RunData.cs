using UnityEngine;
using Unity.Properties;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/RunData", order = 1)]
public class RunData : ScriptableObject
{
    [SerializeField, DontCreateProperty] public int CurrentMoney;
    [SerializeField, DontCreateProperty] public int CurrentReputation;
    [CreateProperty] public string CurrentMoneyText => $"Money: {CurrentMoney}€";
    [CreateProperty] public string CurrentReputationText => $"Reputation: {CurrentReputation}";
}
