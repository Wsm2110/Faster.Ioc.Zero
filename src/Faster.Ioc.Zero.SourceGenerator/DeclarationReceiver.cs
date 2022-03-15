using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Faster.Ioc.Zero.SourceGenerator
{
    public class DeclarationReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> Declarations { get; } = new List<ClassDeclarationSyntax>();
        
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            switch (syntaxNode)
            {
                case ClassDeclarationSyntax classDeclaration:
                    if (classDeclaration.BaseList?.Types
                            .Any(o => o.Type.ToString().EndsWith("ContainerBuilder")) ?? false)
                    {
                        Declarations.Add(classDeclaration);
                    }

                    break;
            }
        }
    }
}
