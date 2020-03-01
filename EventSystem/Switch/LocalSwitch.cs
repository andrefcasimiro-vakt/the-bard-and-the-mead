using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

public class LocalSwitch : MonoBehaviour, ISaveable
{
    public bool ON = false;
    

    public void SetLocalSwitch()
    {
        this.ON = true;
    }

    public bool GetLocalSwitch()
    {
        return ON;
    }

    public object CaptureState()
    {
        return ON;
    }

    public void RestoreState(object state)
    {
        ON = (bool)state;
    }

    public void OnCleanState() {
        
    }
}
