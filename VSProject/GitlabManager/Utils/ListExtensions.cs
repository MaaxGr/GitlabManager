using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GitlabManager.Utils
{
    
    /**
     * Some extensions for a more convenient list access.
     */
    public static class ListExtensions
    {

        /// <summary>
        /// Convert Enumerable (e.g. List) to ReadonlyCollection
        /// </summary>
        /// <param name="source">Writable List</param>
        /// <typeparam name="TSource">Type of list entries</typeparam>
        /// <returns></returns>
        public static ReadOnlyCollection<TSource> ToReadonlyCollection<TSource>(this IEnumerable<TSource> source)
        {
            return source.ToList().AsReadOnly();
        }
        
        /// <summary>
        /// Replace all values of list with new values (but same instance!)
        /// </summary>
        /// <param name="list">List reference</param>
        /// <param name="newList">Values that will be in the returned list</param>
        /// <typeparam name="T">Type of list entries</typeparam>
        private static void Set<T>(this List<T> list, IEnumerable<T> newList)
        {
            list.Clear();
            list.AddRange(newList);
        }
        
        /// <summary>
        /// Delete all elements out of the list, where predicate matches
        /// </summary>
        /// <param name="list">Source list</param>
        /// <param name="predicate">predicate that has to match</param>
        /// <typeparam name="T">Type of list entries</typeparam>
        public static void DeleteWhere<T>(this List<T> list, Func<T, bool> predicate)
        {
            list.Set(list.Where(element => !predicate.Invoke(element)).ToList());
        }

        /// <summary>
        /// Replace all elements of list, where predicate matches with newElement
        /// </summary>
        /// <param name="list">Source list</param>
        /// <param name="predicate">predicate that has to match</param>
        /// <param name="newElement">Element that should be set</param>
        /// <typeparam name="T">Type of list entries</typeparam>
        public static void UpdateWhere<T>(this List<T> list, Func<T, bool> predicate, T newElement)
        {
            list.Set(list.Select(element => predicate(element) ? newElement : element).ToList());
        }
        
    }
}