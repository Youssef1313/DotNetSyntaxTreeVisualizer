﻿using System;
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

        [HttpPost]
        public async Task<SyntaxTreeNode> CSharpPost(CancellationToken cancellationToken)
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            string body = await reader.ReadToEndAsync().ConfigureAwait(false);
            SyntaxNode root = await CSharpSyntaxTree.ParseText(body).GetRootAsync(cancellationToken).ConfigureAwait(false);
            SyntaxTreeNode myRoot = SyntaxTreeNode.CreateMyOwnTree(root);
            return myRoot;
        }
    }
}
