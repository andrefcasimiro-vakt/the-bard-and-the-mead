using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RPG.EventSystem
{
    public enum CommandEnum
    {
        // Text
        DISPLAY_TEXT,
        DISPLAY_NOTIFICATION,
        // Yielders
        WAIT_FOR_SECONDS,
        // Music & Sound
        PLAY_MUSIC,
        PLAY_SOUND,
        // Transform
        MOVE_TOWARDS
    }


    [System.Serializable]
    public class E_Command : E_Template
    {
        public CommandEnum command;
        public string value;

        public override IEnumerator Dispatch()
        {
            yield return StartCoroutine(DoCommand());
        }

        public IEnumerator DoCommand()
        {
            switch (command)
            {
                case CommandEnum.WAIT_FOR_SECONDS:
                    yield return StartCoroutine(WaitForSeconds());
                    break;
                case CommandEnum.DISPLAY_TEXT:
                    yield return DisplayText();
                    break;
                default:
                    yield return null;
                    break;
            }
        }

        IEnumerator DisplayText()
        {
            print(value);
            yield return null;
        }

        IEnumerator WaitForSeconds()
        {
            yield return new WaitForSeconds(float.Parse(value));
        }
    }
}
