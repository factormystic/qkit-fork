using System.Collections.ObjectModel;

namespace QKit.JumpList
{
    /// <summary>
    /// A keyed list of objects that provides additional info for presention in a JumpListBase
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JumpListGroup<T> : ObservableCollection<T>
    {
        /// <summary>
        /// Key that represents the identifier of group of objects.
        /// </summary>
        public object Key { get; set; }

        /// <summary>
        /// Display value that represents the group and used as the group header.
        /// </summary>
        public string KeyDisplay { get; set; }
    }
}
