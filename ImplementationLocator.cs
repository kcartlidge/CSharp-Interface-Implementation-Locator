using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KCartlidge.Support
{
    /// <summary>
    /// Finds and creates instances of all implementations of
    /// an interface that reside in the current assembly.
    /// </summary>
    static class ImplementationLocator
    {
        /// <summary>
        /// Cache of the collection of possible types.
        /// </summary>
        static private List<TypeInfo> types;

        /// <summary>
        /// Locates and provides instance collections of requested interfaces.
        /// </summary>
        static ImplementationLocator()
        {
            var thisAssembly = Assembly.GetExecutingAssembly();
            types = thisAssembly.DefinedTypes.ToList();
        }

        /// <summary>
        /// Gets an instance of the first found implementation of the requested interface.
        /// </summary>
        /// <typeparam name="T">The interface type.</typeparam>
        static public T GetImplementation<T>() where T : class
        {
            return GetImplementations<T>().First();
        }

        /// <summary>
        /// Gets a collection of instances of every possible implementation of the requested interface.
        /// </summary>
        /// <typeparam name="T">The interface type.</typeparam>
        static public List<T> GetImplementations<T>() where T : class
        {
            var result = new List<T>();
            Type t = typeof(T);
            string interfaceNamespace = t.Namespace;
            string interfaceName = t.Name;
            var possibles = FindImplementations(interfaceNamespace, interfaceName);
            foreach (var c in possibles)
            {
                result.Add((T)(Activator.CreateInstance(Type.GetType(c.FullName))));
            }
            return result;
        }

        /// <summary>
        /// Gets a collection of TypeInfos for every possible implementation of the requested interface.
        /// </summary>
        /// <param name="interfaceNamespace"></param>
        /// <param name="interfaceName"></param>
        static private List<TypeInfo> FindImplementations(string interfaceNamespace, string interfaceName)
        {
            var result = new List<TypeInfo>();

            // Why's this not all one LINQ statement? For this method I prefer readability. Sorry.
            foreach (var typeInfo in types)
            {
                foreach (var implements in typeInfo.ImplementedInterfaces.Where(
                    a => a.IsInterface && a.Namespace == interfaceNamespace && a.Name == interfaceName))
                {
                    result.Add(typeInfo);
                }
            }
            return result;
        }
    }
}
