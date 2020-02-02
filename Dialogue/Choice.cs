using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Dialogue { 
    public class Choice
    {
        public string text;
        public string targetGUID;

        public Choice(string text, string targetGUID)
        {
            this.text = text;
            this.targetGUID = targetGUID;
        }
    }
}
