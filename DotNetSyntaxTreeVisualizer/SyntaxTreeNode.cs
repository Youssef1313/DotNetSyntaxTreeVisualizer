using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;

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
            var root = new SyntaxTreeNode(GetSyntaxInformation(nodeOrToken));
            foreach (SyntaxNodeOrToken child in nodeOrToken.ChildNodesAndTokens())
            {
                root.AddChild(CreateMyOwnTree(child));
            }
            return root;
        }

        private static IDictionary<string, string> GetSyntaxInformation(SyntaxNodeOrToken syntax)
        {
            Func<SyntaxNodeOrToken, Microsoft.CodeAnalysis.CSharp.SyntaxKind> csharpKind = Microsoft.CodeAnalysis.CSharp.CSharpExtensions.Kind;
            Func<SyntaxNodeOrToken, Microsoft.CodeAnalysis.VisualBasic.SyntaxKind> vbKind = Microsoft.CodeAnalysis.VisualBasic.VisualBasicExtensions.Kind;
            string kind =
                syntax.Language == LanguageNames.CSharp
                ? csharpKind(syntax).ToString()
                : vbKind(syntax).ToString();

            var result = new Dictionary<string, string>
            {
                { "Kind", kind }
            };

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
