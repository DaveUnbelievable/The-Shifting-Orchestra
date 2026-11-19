using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public List<UserInterfaceAssignment> UserInterfaces;
    public UIDocument MainMenuUI;

    private Dictionary<Enums.UserInterface, UIDocument> userInterfaces;

    void Start()
    {
        // convert list with user interface assignments into dictionary for easier access
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

        HideRunUI();

        RunManager.Instance.StateChanged += ShowRunUI;
        RunManager.Instance.RunStarted += () =>
        {
            ShowMainMenu(false);
        };
        RunManager.Instance.RunEnded += () =>
        {
            ShowMainMenu(true);
        };
    }

    public void ShowRunUI(Enums.RunState runState)
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
            case Enums.RunState.END:
                uiType = Enums.UserInterface.END;
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

    private void HideRunUI()
    {
        foreach (KeyValuePair<Enums.UserInterface, UIDocument> runUI in userInterfaces)
        {
            runUI.Value.rootVisualElement.style.display = DisplayStyle.None;
        }
    }

    public void ShowMainMenu(bool show)
    {
        if (show)
        {
            HideRunUI();
            MainMenuUI.rootVisualElement.style.display = DisplayStyle.Flex;
        }
        else
        {
            MainMenuUI.rootVisualElement.style.display = DisplayStyle.None;
        }
    }

}

[Serializable]
public struct UserInterfaceAssignment
{
    public Enums.UserInterface Type;
    public UIDocument UI;
}
