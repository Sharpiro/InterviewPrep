using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using DSharpAnalyzer.New;

namespace DSharpAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MethodAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "DS02";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.DS02Title), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.DS02MessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.DS02Description), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Initialization";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.ConstructorDeclaration);
        }

        private void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var constructor = context.Node as ConstructorDeclarationSyntax;
            var parameterList = constructor.ParameterList;
            var parameters = parameterList.Parameters.ToList();

            if (constructor == null || !parameters.Any()) return;

            var valueTypeParameters = 0;
            var nullChecks = 0;
            foreach (var parameter in parameters)
            {
                var symbolInfo = context.SemanticModel.GetSymbolInfo(parameter.Type).Symbol as INamedTypeSymbol;
                if (symbolInfo == null) return;
                if (symbolInfo.IsValueType)
                {
                    valueTypeParameters++;
                    continue;
                }

                var statementNodes = constructor.Body.Statements.SelectMany(s => s.DescendantNodesAndSelf()).ToList();
                var hasThrowStatement = statementNodes.OfType<ThrowStatementSyntax>()
                    .Any(ts => ts.DescendantNodes().OfType<IdentifierNameSyntax>()
                    .Any(idn => idn.Identifier.ValueText == parameter.Identifier.ValueText
                ));

                var hasThrowExpression = statementNodes.OfType<ThrowExpressionSyntax>()
                    .Any(ts => ts.DescendantNodes().OfType<IdentifierNameSyntax>()
                    .Any(idn => idn.Identifier.ValueText == parameter.Identifier.ValueText
                ));

                if (hasThrowStatement || hasThrowExpression)
                {
                    nullChecks++;
                    continue;
                }
            }

            if (valueTypeParameters == parameters.Count) return;
            if (nullChecks == parameters.Count - valueTypeParameters) return;

            var diagnosticLocation = Location.Create(context.Node.SyntaxTree, TextSpan.FromBounds(constructor.Span.Start, parameterList.FullSpan.End));
            context.ReportDiagnostic(Diagnostic.Create(Rule, diagnosticLocation, constructor.Identifier));
        }
    }
}