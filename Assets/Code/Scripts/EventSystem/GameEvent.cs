using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("GameEvent"))]
public class GameEvent : ScriptableObject
{
    public List<GameEventListener> listensers = new List<GameEventListener>();

    public void Raise()
    {
        for (int i = 0; i < listensers.Count; i++)
        {
            listensers[i].OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (listensers.Contains(listener))
        {
            listensers.Add(listener);
        }
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (listensers.Contains(listener))
        {
            listensers.Remove(listener);
        }
    }
}
