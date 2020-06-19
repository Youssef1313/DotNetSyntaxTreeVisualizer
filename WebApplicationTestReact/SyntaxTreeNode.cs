using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplicationTestReact
{
    [Serializable]
    public class SyntaxTreeNode
    {
        public string Name { get; }

        public IDictionary<string, string> Attributes { get; }
        public IList<SyntaxTreeNode> Children { get; } = new List<SyntaxTreeNode>();

        public SyntaxTreeNode(IDictionary<string, string> attributes) =>
            (Attributes, Name) = (attributes, attributes.Values.First());

        public void AddChild(SyntaxTreeNode child)
        {
            Children.Add(child);
        }

        public override string ToString()
        {
            return Attributes.Values.First();
        }
    }
}
