using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnButtonClick : MonoBehaviour
{
    public int btnID;

    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(_SendID);
    }

    void _SendID()
    {
        ButtonPressedEvent e = new ButtonPressedEvent(btnID);
        _EventBus.Publish<ButtonPressedEvent>(e);
    }
}
