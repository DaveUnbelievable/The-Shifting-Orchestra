using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance { get; private set; }
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion


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
    }

    public void ShowUI(Enums.UserInterface type)
    {
        foreach (KeyValuePair<Enums.UserInterface, UIDocument> userInterface in userInterfaces)
        {
            if (userInterface.Key == type)
            {
                userInterface.Value.rootVisualElement.style.display = DisplayStyle.Flex;
            }
            else
            {
                userInterface.Value.rootVisualElement.style.display = DisplayStyle.None;
            }
        }
        // warn if the ui doesnt exist
        if (!userInterfaces.ContainsKey(type))
        {
            Debug.LogWarning($"UI vom Typ {type} nicht gefunden!");
        }
    }

}

[Serializable]
public struct UserInterfaceAssignment
{
    public Enums.UserInterface Type;
    public UIDocument UI;
}
