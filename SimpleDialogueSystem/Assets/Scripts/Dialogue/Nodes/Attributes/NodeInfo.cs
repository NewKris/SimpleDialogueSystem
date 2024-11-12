using System;

namespace Dialogue.Nodes.Attributes {
    public class NodeInfo : Attribute {
        public readonly string nodeName;
        public readonly string nodeGroup;

        public NodeInfo(string nodeName, string nodeGroup = "General") {
            this.nodeName = nodeName;
            this.nodeGroup = nodeGroup;
        }
    }
}
