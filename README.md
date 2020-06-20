# DotNetSyntaxTreeVisualizer

.NET Syntax Tree Visualizer, A C# ASP.NET Core app with ReactJS in front-end that shows Roslyn's syntax tree for a given source code.

The app is deployed [here](https://DotNetSyntaxTreeVisualizer.azurewebsites.net/).

## Visual Studio already has a graph visualizer

That's true. But not all developers work on Visual Studio. Some developers might be using Rider, VS Code, or whatever. Or you might just be lazy to open Visual Studio!

## Why to inspect a syntax tree

- For fun, if you just want to see what's the syntax tree Roslyn is generating.
- For writing Roslyn's analyzers and codefixes requies knowledge of how the syntax tree for the case you're inspecting looks like.

## Screenshot

![image](https://user-images.githubusercontent.com/31348972/85202525-31363300-b307-11ea-8b96-2d44fc742bf4.png)
