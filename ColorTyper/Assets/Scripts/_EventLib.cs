using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{
    public static int currAvailableEventId = 0;
}

public abstract class BaseEvent
{
    protected int eventId = 0;
    public BaseEvent() { eventId = EventHandler.currAvailableEventId++; }

    public override string ToString()
    {
        return "E| Event id: " + eventId;
    }
}

public class AssignUIColorEvent : BaseEvent
{
    public Color[] colors;
    public AssignUIColorEvent(Color[] _colors)
    {
        colors = _colors;
    }
}

public class UpdateButtonColorEvent : BaseEvent
{
    public int correctColor;
    public UpdateButtonColorEvent(int _correctColor)
    {
        correctColor = _correctColor;
    }
}

public class ButtonPressedEvent : BaseEvent
{
    public int ID;
    public ButtonPressedEvent(int _ID)
    {
        ID = _ID;
    }
}

public class CorrectColorSelectedEvent : BaseEvent
{
    public CorrectColorSelectedEvent() { }
}

public class GameOverEvent : BaseEvent
{
    public GameOverEvent() { }
}

