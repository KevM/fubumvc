﻿using System;
using System.Linq.Expressions;
using System.Reflection;
using FubuCore.Reflection;
using FubuMVC.Core.Registration.Nodes;

namespace FubuMVC.Core.Registration.Querying
{
    public interface IChainResolver
    {
        BehaviorChain Find(Type handlerType, MethodInfo method);

        BehaviorChain FindUnique(object model, string category = null);
        
        BehaviorChain FindCreatorOf(Type type);

        void RootAt(string baseUrl);
        IChainForwarder FindForwarder(object model, string category = null);
        BehaviorChain FindUniqueByInputType(Type modelType, string category = null);
    }

    public static class ChainResolverExtensions
    {
        public static BehaviorChain Find<T>(this IChainResolver resolver, Expression<Action<T>> expression)
        {
            return resolver.Find(typeof (T), ReflectionHelper.GetMethod(expression));
        }
    }

    public enum CategorySearchMode
    {
        /// <summary>
        /// Only match if the category of the chains explicitly matches the search criteria
        /// </summary>
        Strict,

        /// <summary>
        /// If only one chain is found for all the other criteria, it's okay
        /// to ignore the exact match on Category
        /// </summary>
        Relaxed
    }

    public enum TypeSearchMode
    {
        Any,
        InputModelOnly,
        HandlerOnly
    }
}