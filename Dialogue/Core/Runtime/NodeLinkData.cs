using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue.Core
{
    [System.Serializable]
    public class NodeLinkData
    {
        public string BaseNodeGuid;
        public string PortName;
        public string TargetNodeGuid;
    }
}
