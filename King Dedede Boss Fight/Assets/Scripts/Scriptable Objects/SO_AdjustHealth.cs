using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Adjust Health Event Sender", menuName = "ScriptableObjects/AdjustHealth")]
public class SO_AdjustHealth : ScriptableObject
{
    /// <summary>
    /// Using the Scriptable Object as the middle man
    /// Other scripts call this event which sends it to the UI Manager
    /// Reduces Dependencies on other scripts
    /// </summary>
 
    [System.NonSerialized]
    public ChangeBossHealth changeBossHealthEvent = new ChangeBossHealth();

    [System.NonSerialized]
    public ChangeKirbyHealth changeKirbyHealthEvent = new ChangeKirbyHealth();

    public void ChangeBossHealthEventSend(ChangeHealth changeHealth)
    {
        changeBossHealthEvent.Invoke(changeHealth);
    }

    public void ChangeKirbyHealthEventSend(ChangeHealth changeHealth)
    {
        changeKirbyHealthEvent.Invoke(changeHealth);
    }
}

public class ChangeBossHealth : UnityEvent<ChangeHealth> { }
public class ChangeKirbyHealth : UnityEvent<ChangeHealth> { }





