﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace D2L.CodeStyle.Analyzers.Common.DependencyInjection {
	// void RegisterSubInterface<TSubInterfaceType, TInjectableSuperInterfaceType>(
	//		this IDependencyRegistry @this,
	//		ObjectScope scope
	// ) where TInjectableSuperInterfaceType : TSubInterfaceType
	internal sealed class RegisterSubInterfaceExpression : DependencyRegistrationExpression {
		internal override bool CanHandleMethod( IMethodSymbol method ) {
			return method.Name == "RegisterSubInterface"
				&& method.IsExtensionMethod
				&& method.TypeArguments.Length == 2
				&& method.Parameters.Length == 1;
		}

		internal override DependencyRegistration GetRegistration( IMethodSymbol method, SeparatedSyntaxList<ArgumentSyntax> arguments, SemanticModel semanticModel ) {
			if( arguments.Count != 1 ) {
				return null;
			}

			ObjectScope scope;
			if( !TryGetObjectScope( arguments[0], semanticModel, out scope ) ) {
				return null;
			}
			return DependencyRegistration.Factory(
				scope: scope,
				dependencyType: method.TypeArguments[0],
				factoryType: method.TypeArguments[1]
			);
		}
	}
}
