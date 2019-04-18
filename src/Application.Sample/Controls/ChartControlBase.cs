using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Application.Sample
{
    public abstract class ChartControlBase : UserControl, INotifyPropertyChanged
    {
        private static Dictionary<Type, ChartControlBase> controlCache = new Dictionary<Type, ChartControlBase>();

        protected ChartControlBase()
        {
            LastDataSet = 1;
            //DataContext = this;
        }



















        /// <summary>
        /// Gets a chart control, either freshly created or from cache if it's already in there.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <returns>An instance of T</returns>
        public static T GetChartControl<T>() where T : ChartControlBase, new()
        {
            Type type = typeof(T);

            if (controlCache.ContainsKey(type))
            {
                return (T)controlCache[type];
            }

            T result = new T();
            controlCache.Add(type, result);
            return result;
        }

        /// <summary>
        /// When overriden in a derived class, returns the number of data options available.
        /// </summary>
        public abstract int DataSetCount
        {
            get;
        }

        /// <summary>
        /// Gets the last generated data set. Between 1 and <see cref="DataSetCount"/>.
        /// </summary>
        public int LastDataSet
        {
            get;
            protected set;
        }

        /// <summary>
        /// Creates chart data using the data set specified by <see cref="LastDataSet"/>.
        /// </summary>
        public void CreateChartData()
        {
            CreateChartData(LastDataSet);
        }

        /// <summary>
        /// When overriden in a derived class, creates chart data using the specified data set.
        /// </summary>
        /// <param name="dataSet">The data set number. Between 1 and <see cref="DataSetCount"/>.</param>
        public abstract void CreateChartData(int dataSet);


        #region INotifyPropertyChanged
        /// <summary>
        /// Implementation of INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Sets a property if it has changed and notifies listeners.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="storage">The backing store reference.</param>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">Property name. Leave null if calling from the property setter.</param>
        /// <returns>true if the property was changed; false if the property already has <paramref name="value"/>.</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
