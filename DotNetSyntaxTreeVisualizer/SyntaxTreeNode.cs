using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetSyntaxTreeVisualizer
{
    [Serializable]
    public class SyntaxTreeNode
    {
        public string Name { get; }

        public IDictionary<string, string> Properties { get; }
        public IList<SyntaxTreeNode> Children { get; } = new List<SyntaxTreeNode>();

        public SyntaxTreeNode(IDictionary<string, string> properties) =>
            (Properties, Name) = (properties, properties.Values.First());

        public void AddChild(SyntaxTreeNode child)
        {
            Children.Add(child);
        }

        public override string ToString()
        {
            return Properties.Values.First();
        }
    }
}
