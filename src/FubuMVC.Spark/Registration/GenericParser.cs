using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FubuCore;

namespace FubuMVC.Spark.Registration
{
    public class GenericParser 
    {
        private readonly IEnumerable<Assembly> _assemblies;

        public GenericParser(IEnumerable<Assembly> assemblies)
        {
            _assemblies = assemblies;
        }

        public Type Parse(string typeName)
        {
            if (!IsGeneric(typeName))
                return findType(typeName);

            var leftGenericDelimiter = typeName.IndexOf('<');
            var rightGenericDelimiter = typeName.IndexOf('>');
                
            var genericArgumentsNames = typeName.Substring(leftGenericDelimiter+1, rightGenericDelimiter - leftGenericDelimiter - 1);
            var genericArguments = genericArgumentsNames.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => findType(t)).ToArray();

            var openTypeName = typeName.Substring(0, leftGenericDelimiter);
            var openTypeNameWithArgCount = openTypeName + "`" + genericArguments.Length;
            var openType = findType(openTypeNameWithArgCount);

            if (openType == null)
                throw new ArgumentException("Could not find the open type {0} for your type name {1}.".ToFormat(openTypeNameWithArgCount, typeName));

            return openType.MakeGenericType(genericArguments);
        }

        private Type findType(string typeName)
        {
            return _assemblies
                .Select(assembly => assembly.GetType(typeName))
                .FirstOrDefault(type => type != null);
        }

        public static bool IsGeneric(string typeName)
        {
            return typeName.IndexOf('<') != -1;
        }
    }
}