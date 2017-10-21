using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MSCRMToolKit
{
    /// <summary>
    /// SortableBindingList class extending BindingList
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SortableBindingList<T> : BindingList<T>
    {
        /// <summary>
        /// The comparers
        /// </summary>
        private readonly Dictionary<Type, PropertyComparer<T>> comparers;
        /// <summary>
        /// The is sorted
        /// </summary>
        private bool isSorted;
        /// <summary>
        /// The list sort direction
        /// </summary>
        private ListSortDirection listSortDirection;
        /// <summary>
        /// The property descriptor
        /// </summary>
        private PropertyDescriptor propertyDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortableBindingList{T}"/> class.
        /// </summary>
        public SortableBindingList()
            : base(new List<T>())
        {
            this.comparers = new Dictionary<Type, PropertyComparer<T>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortableBindingList{T}"/> class.
        /// </summary>
        /// <param name="enumeration">The enumeration.</param>
        public SortableBindingList(IEnumerable<T> enumeration)
            : base(new List<T>(enumeration))
        {
            this.comparers = new Dictionary<Type, PropertyComparer<T>>();
        }

        /// <summary>
        /// Gets a value indicating whether [supports sorting core].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [supports sorting core]; otherwise, <c>false</c>.
        /// </value>
        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is sorted core.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is sorted core; otherwise, <c>false</c>.
        /// </value>
        protected override bool IsSortedCore
        {
            get { return this.isSorted; }
        }

        /// <summary>
        /// Gets the sort property core.
        /// </summary>
        /// <value>
        /// The sort property core.
        /// </value>
        protected override PropertyDescriptor SortPropertyCore
        {
            get { return this.propertyDescriptor; }
        }

        /// <summary>
        /// Gets the sort direction core.
        /// </summary>
        /// <value>
        /// The sort direction core.
        /// </value>
        protected override ListSortDirection SortDirectionCore
        {
            get { return this.listSortDirection; }
        }

        /// <summary>
        /// Gets a value indicating whether [supports searching core].
        /// </summary>
        /// <value>
        /// <c>true</c> if [supports searching core]; otherwise, <c>false</c>.
        /// </value>
        protected override bool SupportsSearchingCore
        {
            get { return true; }
        }

        /// <summary>
        /// Applies the sort core.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="direction">The direction.</param>
        protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
        {
            List<T> itemsList = (List<T>)this.Items;

            Type propertyType = property.PropertyType;
            PropertyComparer<T> comparer;
            if (!this.comparers.TryGetValue(propertyType, out comparer))
            {
                comparer = new PropertyComparer<T>(property, direction);
                this.comparers.Add(propertyType, comparer);
            }

            comparer.SetPropertyAndDirection(property, direction);
            itemsList.Sort(comparer);

            this.propertyDescriptor = property;
            this.listSortDirection = direction;
            this.isSorted = true;

            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        /// <summary>
        /// Removes the sort core.
        /// </summary>
        protected override void RemoveSortCore()
        {
            this.isSorted = false;
            this.propertyDescriptor = base.SortPropertyCore;
            this.listSortDirection = base.SortDirectionCore;

            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        /// <summary>
        /// Finds the core.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected override int FindCore(PropertyDescriptor property, object key)
        {
            int count = this.Count;
            for (int i = 0; i < count; ++i)
            {
                T element = this[i];
                if (property.GetValue(element).Equals(key))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
