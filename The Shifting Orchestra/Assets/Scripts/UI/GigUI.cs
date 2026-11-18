using UnityEngine;
using UnityEngine.UIElements;

public class GigUI : MonoBehaviour
{
    private void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        
        RunManager.Instance.StateChanged += (state) =>
        {
            // show gig if run state is GIG
            if (state == Enums.RunState.GIG)
            {
                
            }
        };
    }
}
