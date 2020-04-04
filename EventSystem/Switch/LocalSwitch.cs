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

    public void SetLocalSwitchOn()
    {
        this.ON = true;
    }

    public void SetLocalSwitchOff()
    {
        this.ON = false;
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
        Debug.Log("restoring state of: " + this.gameObject.name);
        ON = (bool)state;
    }

    public void OnCleanState() {
    }
}
