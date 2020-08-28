using Microsoft.CodeAnalysis;
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

        public static SyntaxTreeNode CreateMyOwnTree(SyntaxNodeOrToken nodeOrToken)
        {
            var root = new SyntaxTreeNode(GetSyntaxNodeOrTokenInformation(nodeOrToken));
            foreach (SyntaxNodeOrToken child in nodeOrToken.ChildNodesAndTokens())
            {
                root.AddChild(CreateMyOwnTree(child));
            }
            return root;
        }

        private static IDictionary<string, string> GetSyntaxNodeOrTokenInformation(SyntaxNodeOrToken nodeOrToken)
        {
            return nodeOrToken.IsNode
                ? GetSyntaxInformation(nodeOrToken.AsNode())
                : GetSyntaxInformation(nodeOrToken.AsToken());
        }

        private static IDictionary<string, string> GetSyntaxInformation<T>(T syntax)
        {
            var result = new Dictionary<string, string>();
            if (syntax is SyntaxNode node)
            {
                result.Add("NodeKind", node.Kind().ToString()); // TODO: Kind() here is for C#. Considering fixing that.
            }
            else if (syntax is SyntaxToken token)
            {
                result.Add("TokenKind", token.Kind().ToString());
            }
            else
            {
                throw new ArgumentException($"The specified {nameof(syntax)} is not a SyntaxNode nor a SyntaxToken.");
            }
            PropertyInfo[] properties = syntax.GetType().GetProperties();
            foreach (PropertyInfo info in properties)
            {
                // Language isn't important to include in each node.
                // Parent is redundant. I can already see the parent.
                // ValueText and Value are the same as Text.
                // SyntaxTree shows the complete source in each node. That's redundant.
                // RawKind is just the underlying numeric value of SyntaxKind enum. It's meaningless.
                if (info.Name == "Language" || info.Name == "Parent" ||
                    info.Name == "ValueText" || info.Name == "Value" ||
                    info.Name == "SyntaxTree" || info.Name == "RawKind")
                {
                    continue;
                }
                result.Add(info.Name, info.GetValue(syntax)?.ToString());
            }
            return result;
        }

        public override string ToString()
        {
            return Properties.Values.First();
        }
    }
}
