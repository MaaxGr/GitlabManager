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

        public static ReadOnlyCollection<TSource> ToReadonlyCollection<TSource>(this IEnumerable<TSource> source)
        {
            return source.ToList().AsReadOnly();
        }
        
        public static void Set<T>(this List<T> list, List<T> newList)
        {
            list.Clear();
            list.AddRange(newList);
        }
        
        public static void DeleteWhere<T>(this List<T> list, Func<T, bool> predicate)
        {
            list.Set(list.Where(element => !predicate.Invoke(element)).ToList());
        }

        public static void UpdateWhere<T>(this List<T> list, Func<T, bool> predicate, T newElement)
        {
            list.Set(list.Select(element => predicate(element) ? newElement : element).ToList());
        }
        
    }
}