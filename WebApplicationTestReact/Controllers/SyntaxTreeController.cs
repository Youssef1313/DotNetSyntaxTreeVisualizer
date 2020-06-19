using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Logging;

namespace WebApplicationTestReact.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SyntaxTreeController : ControllerBase
    {
        private readonly ILogger<SyntaxTreeController> _logger;

        public SyntaxTreeController(ILogger<SyntaxTreeController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<SyntaxTreeNode> CSharpPost(CancellationToken cancellationToken)
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            string body = await reader.ReadToEndAsync().ConfigureAwait(false);
            SyntaxNode root = await CSharpSyntaxTree.ParseText(body).GetRootAsync(cancellationToken).ConfigureAwait(false);
            SyntaxTreeNode myRoot = CreateMyOwnTree(root);
            return myRoot;
        }

        private SyntaxTreeNode CreateMyOwnTree(SyntaxNodeOrToken nodeOrToken)
        {
            var root = new SyntaxTreeNode(GetSyntaxNodeOrTokenInformation(nodeOrToken));
            foreach (SyntaxNodeOrToken child in nodeOrToken.ChildNodesAndTokens())
            {
                root.AddChild(CreateMyOwnTree(child));
            }
            return root;
        }

        private IDictionary<string, string> GetSyntaxNodeOrTokenInformation(SyntaxNodeOrToken nodeOrToken)
        {
            return nodeOrToken.IsNode
                ? GetSyntaxInformation(nodeOrToken.AsNode())
                : GetSyntaxInformation(nodeOrToken.AsToken());
        }

        private IDictionary<string, string> GetSyntaxInformation<T>(T syntax)
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
    }
}
