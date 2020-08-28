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
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.Extensions.Logging;

namespace DotNetSyntaxTreeVisualizer.Controllers
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

        [HttpPost("CSharp")] // POST: /SyntaxTree/CSharp
        public async Task<SyntaxTreeNode> CSharpPost(CancellationToken cancellationToken)
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            string body = await reader.ReadToEndAsync().ConfigureAwait(false);
            SyntaxTree tree = CSharpSyntaxTree.ParseText(body);
            SyntaxNode root = await tree.GetRootAsync(cancellationToken).ConfigureAwait(false);
            Compilation compilation = CSharpCompilation.Create("HelloWorld", new[] { tree });
            SemanticModel model = compilation.GetSemanticModel(tree);
            SyntaxTreeNode myRoot = SyntaxTreeNode.CreateMyOwnTree(root, model);
            return myRoot;
        }

        [HttpPost("VisualBasic")] // POST: /SyntaxTree/VisualBasic
        public async Task<SyntaxTreeNode> VisualBasicPost(CancellationToken cancellationToken)
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            string body = await reader.ReadToEndAsync().ConfigureAwait(false);
            SyntaxTree tree = VisualBasicSyntaxTree.ParseText(body);
            SyntaxNode root = await tree.GetRootAsync(cancellationToken).ConfigureAwait(false);
            Compilation compilation = VisualBasicCompilation.Create("HelloWorld", new[] { tree });
            SemanticModel model = compilation.GetSemanticModel(tree);
            SyntaxTreeNode myRoot = SyntaxTreeNode.CreateMyOwnTree(root, model);
            return myRoot;
        }
    }
}
