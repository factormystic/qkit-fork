using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Windows.Globalization.Collation;

namespace QKit.JumpList
{
    /// <summary>
    /// Provides a utility to help group and sort data into JumpList compatible data.
    /// </summary>
    public static class JumpListHelper
    {
        /// <summary>
        /// Groups and sorts into a list of group lists based on a selector.
        /// </summary>
        /// <typeparam name="TSource">Type of the items in the list.</typeparam>
        /// <typeparam name="TSort">Type of value returned by sortSelector.</typeparam>
        /// <typeparam name="TGroup">Type of value returned by groupSelector.</typeparam>
        /// <param name="source">List to be grouped and sorted</param>
        /// <param name="sortSelector">A selector that provides the value that items will be sorted by.</param>
        /// <param name="groupSelector">A selector that provides the value that items will be grouped by.</param>
        /// <param name="groupDisplaySelector">A selector that will provide the value represent a group for display.</param>
        /// <returns>A list of JumpListGroups.</returns>
        public static List<JumpListGroup<TSource>> ToGroups<TSource, TSort, TGroup>(
            this IEnumerable<TSource> source,
            Func<TSource, TSort> sortSelector,
            Func<TSource, TGroup> groupSelector,
            Func<TGroup, String> groupDisplaySelector = null)
        {
            var groups = new List<JumpListGroup<TSource>>();

            // Group and sort items based on values returned from the selectors
            var query = from item in source
                        orderby groupSelector(item), sortSelector(item)
                        group item by groupSelector(item) into g
                        select new { GroupName = g.Key, Items = g };

            // For each group generated from the query, create a JumpListGroup
            // and fill it with its items
            foreach (var g in query)
            {
                JumpListGroup<TSource> group = new JumpListGroup<TSource>();
                group.Key = g.GroupName;
                group.KeyDisplay = groupDisplaySelector == null ? g.GroupName.ToString() : groupDisplaySelector(g.GroupName);
                foreach (var item in g.Items)
                    group.Add(item);

                groups.Add(group);
            }

            return groups;
        }

        /// <summary>
        /// Groups and sorts into a list of alpha groups based on a string selector.
        /// </summary>
        /// <typeparam name="TSource">Type of the items in the list.</typeparam>
        /// <param name="source">List to be grouped and sorted.</param>
        /// <param name="selector">A selector that will provide a value that items to be sorted and grouped by.</param>
        /// <returns>A list of JumpListGroups.</returns>
        public static List<JumpListGroup<TSource>> ToAlphaGroups<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, string> selector)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                var group = new JumpListGroup<TSource>() { Key = "\uD83C\uDF10", KeyDisplay = "\uD83C\uDF10" };
                foreach (var item in source.OrderBy(selector))
                    group.Add(item);
                return new List<JumpListGroup<TSource>> { group };
            }

            // Get the letters representing each group for current language using CharacterGroupings class
            var characterGroupings = new CharacterGroupings();

            // Create dictionary for the letters and replace '...' with proper globe icon
            var keys = characterGroupings.Where(x => x.Label.Count() >= 1).Select(x => x.Label).ToDictionary(x => x);
            keys["..."] = "\uD83C\uDF10";

            // Create groups for each letters
            var groupDictionary = keys.Select(x => new JumpListGroup<TSource>() { Key = x.Value, KeyDisplay = x.Value })
                .ToDictionary(x => (string)x.Key);

            // Sort and group items into the groups based on the value returned by the selector
            var query = from item in source
                        orderby selector(item)
                        select item;

            foreach (var item in query)
            {
                var sortValue = selector(item);
                var label = characterGroupings.Lookup(sortValue);
                var key = keys[label];
                groupDictionary[key].Add(item);
            }

            return groupDictionary.Select(x => x.Value).ToList();
        }

        /// <summary>
        /// Groups and sorts into a list of alpha groups based on a string selector.
        /// </summary>
        /// <typeparam name="TSource">Type of the items in the list.</typeparam>
        /// <param name="source">List to be grouped and sorted.</param>
        /// <param name="selector">A selector that will provide a value that items to be grouped by.</param>
        /// <param name="keySelector">A selector that will provide a value that items to be sorted by.</param>
        /// <returns>An ObservableCollection of JumpListGroups.</returns>
        public static ObservableCollection<JumpListGroup<TSource>> ToAlphaGroups<TSource>(
            this ObservableCollection<TSource> source,
            Func<TSource, string> selector,
            Func<TSource, IComparable> keySelector)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                var group = new JumpListGroup<TSource>() { Key = "\uD83C\uDF10", KeyDisplay = "\uD83C\uDF10" };
                foreach (var item in source.OrderBy(selector))
                    group.Add(item);
                return new ObservableCollection<JumpListGroup<TSource>> { group };
            }

            // Get the letters representing each group for current language using CharacterGroupings class
            var characterGroupings = new CharacterGroupings();

            // Create dictionary for the letters and replace '...' with proper globe icon
            var keys = characterGroupings.Where(x => x.Label.Count() >= 1).Select(x => x.Label).ToDictionary(x => x);
            keys["..."] = "\uD83C\uDF10";

            // Create groups for each letters
            var groupDictionary = keys.Select(x => new JumpListGroup<TSource>() { Key = x.Value, KeyDisplay = x.Value })
                .ToDictionary(x => (string)x.Key);

            // Sort and group items into the groups based on the value returned by the selector functions
            var sortedInsert = new Action<TSource>(item =>
            {
                var sortValue = selector(item);
                // todo: handle when sortValue is null
                var group = groupDictionary[keys[characterGroupings.Lookup(sortValue)]];

                var sortingCopy = group.ToList();
                sortingCopy.Add(item);
                sortingCopy = sortingCopy.OrderBy(keySelector).ToList();

                group.Insert(sortingCopy.IndexOf(item), item);
            });

            foreach (var item in source)
            {
                sortedInsert(item);
            }

            source.CollectionChanged += (s, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var item in e.NewItems.Cast<TSource>())
                        {
                            sortedInsert(item);
                        }
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        foreach (var item in e.OldItems.Cast<TSource>())
                        {
                            var sortValue = selector(item);
                            var group = groupDictionary[keys[characterGroupings.Lookup(sortValue)]];
                            group.Remove(item);
                        }
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        foreach (var kv in groupDictionary)
                        {
                            foreach (var item in kv.Value.Except(source).ToList())
                            {
                                kv.Value.Remove(item);
                            }
                        }

                        foreach (var item in source)
                        {
                            sortedInsert(item);
                        }
                        break;

                    default:
                        System.Diagnostics.Debugger.Break();
                        break;
                }
            };

            return new ObservableCollection<JumpListGroup<TSource>>(groupDictionary.Select(x => x.Value));
        }
    }
}
