using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Faster.Ioc.Zero.SourceGenerator
{
    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            // Find the main method
            var entryPoint = context.Compilation.GetEntryPoint(context.CancellationToken);

            var ns = entryPoint?.ContainingNamespace.ToString();

            var builder = new Builder(context);

            if (context.SyntaxReceiver is not DeclarationReceiver receiver)
            {
                return;
            }

            if (!receiver.Declarations.Any())
            {
                return;
            }

            var invokable = receiver.Declarations.SelectMany(d =>
                  d.DescendantNodes().OfType<MethodDeclarationSyntax>()
                      .FirstOrDefault(i => i.Identifier.Text == "Bootstrap")
                      ?.DescendantNodes()
                      .OfType<InvocationExpressionSyntax>());

            var classDeclaration = receiver.Declarations[0];
            var semantic = context.Compilation.GetSemanticModel(classDeclaration.SyntaxTree);

            GenerateContainer(context, semantic, invokable, builder, ns);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new DeclarationReceiver());
        }
        
        private void GenerateContainer(GeneratorExecutionContext context, SemanticModel semantic, IEnumerable<InvocationExpressionSyntax> invokers, Builder builder, string ns)
        {
            foreach (var invocation in invokers)
            {
                var arguments = invocation.ArgumentList.Arguments;
                var symbol = (IMethodSymbol)ModelExtensions.GetSymbolInfo(semantic, invocation).Symbol;

                if (symbol == null)
                {
                    continue;
                }

                var typeArguments = symbol.TypeArguments.Cast<INamedTypeSymbol>().ToList();
                var expressionBodyArguments = CustomConstructorResolver(arguments);

                var isSingleton = arguments.FirstOrDefault(i => i.ToString().Contains("Singleton"));
                if (isSingleton != null)
                {
                    builder.Register(typeArguments, expressionBodyArguments, Lifetime.Singleton, string.Empty);
                }
                else
                {
                    builder.Register(typeArguments, expressionBodyArguments, Lifetime.Transient, string.Empty);
                }
            }

            context.AddSource("Container.g.cs", string.Format(Declarations.Factory, ns));

            var classes = builder.Build(ns);
            foreach (var @class in classes)
            {
                context.AddSource($"{@class.Classes[0].FileName}Container.g.cs", @class.ToString());
            }
        }
        private static List<string> CustomConstructorResolver(SeparatedSyntaxList<ArgumentSyntax> arguments)
        {
            var isOveride = arguments.FirstOrDefault(i => i.ToString().Contains("new"));
            List<string> paramNames = new List<string>();

            if (isOveride == null)
            {
                return paramNames;
            }

            var overrideBody = ((ParenthesizedLambdaExpressionSyntax)isOveride.Expression).ExpressionBody;
            if (overrideBody != null)
            {
                var conti = ((ObjectCreationExpressionSyntax)overrideBody).ArgumentList;
                if (conti != null)
                {
                    var parameterArguments = conti.Arguments.ToList();
                    if (parameterArguments.Any())
                    {

                        foreach (var argument in parameterArguments)
                        {
                            var paramName = ((ObjectCreationExpressionSyntax)argument.Expression).Type.ToString();
                            paramNames.Add(paramName);
                        }
                    }
                }
            }

            return paramNames;
        }
    }
}
