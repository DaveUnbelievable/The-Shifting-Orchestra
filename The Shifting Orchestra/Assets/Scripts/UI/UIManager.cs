using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public List<UserInterfaceAssignment> UserInterfaces;

    private Dictionary<Enums.UserInterface, UIDocument> userInterfaces;

    void Start()
    {
        userInterfaces = new Dictionary<Enums.UserInterface, UIDocument>();
        foreach (UserInterfaceAssignment assignment in UserInterfaces)
        {
            if (!userInterfaces.ContainsKey(assignment.Type))
            {
                userInterfaces.Add(assignment.Type, assignment.UI);
            }
            else
            {
                Debug.LogWarning($"doppelte UI vom Typ {assignment.Type}!");
            }
        }

        RunManager.Instance.StateChanged += ShowUI;
    }

    // public void ShowUI(Enums.UserInterface type)
    public void ShowUI(Enums.RunState runState)
    {
        Enums.UserInterface uiType = Enums.UserInterface.PREPARATION;
        switch (runState) {
            case Enums.RunState.GIG_OFFERS:
                uiType = Enums.UserInterface.GIG_OFFERS;
                break;
            case Enums.RunState.PREPARATION:
                uiType = Enums.UserInterface.PREPARATION;
                break;
            case Enums.RunState.GIG:
                uiType = Enums.UserInterface.GIG;
                break;
            case Enums.RunState.EVALUATION:
                uiType = Enums.UserInterface.EVALUATION;
                break;
        }
        foreach (KeyValuePair<Enums.UserInterface, UIDocument> userInterface in userInterfaces)
        {
            if (userInterface.Key == uiType)
            {
                userInterface.Value.rootVisualElement.style.display = DisplayStyle.Flex;
            }
            else
            {
                userInterface.Value.rootVisualElement.style.display = DisplayStyle.None;
            }
        }
        // warn if the ui doesnt exist
        if (!userInterfaces.ContainsKey(uiType))
        {
            Debug.LogWarning($"UI vom Typ {uiType} nicht gefunden!");
        }
    }

}

[Serializable]
public struct UserInterfaceAssignment
{
    public Enums.UserInterface Type;
    public UIDocument UI;
}
