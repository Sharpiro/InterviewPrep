﻿using Microsoft.CodeAnalysis;
using System.Linq;

namespace DSharpAnalyzer
{
    public static class NodeExtensions
    {
        public static T FindDescendantByAnnotation<T>(this SyntaxNode syntaxNode, SyntaxAnnotation annotation) where T : SyntaxNode
        {
            return (T)syntaxNode.DescendantNodes().SingleOrDefault(n => n.HasAnnotation(annotation));
        }

        public static T FindAncestorByAnnotation<T>(this SyntaxNode syntaxNode, SyntaxAnnotation annotation) where T : SyntaxNode
        {
            return (T)syntaxNode.Ancestors().SingleOrDefault(n => n.HasAnnotation(annotation));
        }
    }
}