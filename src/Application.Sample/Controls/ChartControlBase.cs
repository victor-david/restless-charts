using Restless.Controls.Chart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Application.Sample
{
    /// <summary>
    /// Represents the base class for a chart control. This class must be inherited.
    /// </summary>
    public abstract class ChartControlBase : UserControl, INotifyPropertyChanged
    {
        #region Private
        private int deferredDataSet;
        private object selectedLegendItem;
        private int selectedLegendIndex = -1;
        private static Dictionary<Type, ChartControlBase> controlCache = new Dictionary<Type, ChartControlBase>();
        #endregion

        #region Constructors
        protected ChartControlBase()
        {
            deferredDataSet = 0;
            LastDataSet = 0;
            AddHandler(LoadedEvent, new RoutedEventHandler(ChartControlBaseLoaded));
            TopTitle = new TextBlock()
            {
                FontSize = 20.0,
                Foreground = Brushes.DarkGray,
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            BottomTitle = new TextBlock()
            {
                FontSize = 20.0,
                Foreground = Brushes.DarkGray,
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            LeftTitle = new TextBlock()
            {
                FontSize = 14.0,
                Foreground = Brushes.Crimson,
                Margin = new Thickness(5),
            };

            RightTitle = new TextBlock()
            {
                FontSize = 14.0,
                Foreground = Brushes.Crimson,
                Margin = new Thickness(5),
            };

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
        #endregion

        #region Properties
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
            private set;
        }

        /// <summary>
        /// Gets the top title.
        /// </summary>
        /// <remarks>
        /// Top title can be any object. Here we're using a text block
        /// and derived classes can just set its text property,
        /// but we could use another type of element.
        /// </remarks>
        public TextBlock TopTitle
        {
            get;
        }

        /// <summary>
        /// Gets the bottom title.
        /// </summary>
        /// <remarks>
        /// Bottom title can be any object. Here we're using a text block
        /// and derived classes can just set its text property,
        /// but we could use another type of element.
        /// </remarks>
        public TextBlock BottomTitle
        {
            get;
        }

        /// <summary>
        /// Gets the left title.
        /// </summary>
        /// <remarks>
        /// Left title can be any object. Here we're using a text block
        /// and derived classes can just set its text property,
        /// but we could use another type of element.
        /// </remarks>
        public TextBlock LeftTitle
        {
            get;
        }

        /// <summary>
        /// Gets the right title.
        /// </summary>
        /// <remarks>
        /// Right title can be any object. Here we're using a text block
        /// and derived classes can just set its text property,
        /// but we could use another type of element.
        /// </remarks>
        public TextBlock RightTitle
        {
            get;
        }

        /// <summary>
        /// Gets or sets the selected legend item. See remarks.
        /// </summary>
        /// <remarks>
        /// Derived classes that use an interactive legend can bind <see cref="ChartLegend.SelectedItem"/>
        /// to this property. When it changes, <see cref="OnSelectedLegendItemChanged(DataSeriesInfo)"/>
        /// will be called.
        /// </remarks>
        public object SelectedLegendItem
        {
            get => selectedLegendItem;
            set
            {
                if (SetProperty(ref selectedLegendItem, value))
                {
                    OnSelectedLegendItemChanged(selectedLegendItem as DataSeriesInfo);
                }
            }
        }


        /// <summary>
        /// Gets the selected legend index.
        /// </summary>
        /// <remarks>
        /// Derived classes that use an interactive legend should bind
        /// </remarks>
        public int SelectedLegendIndex
        {
            get => selectedLegendIndex;
            protected set => SetProperty(ref selectedLegendIndex, value);
        }
        #endregion

        #region Public and protected methods
        /// <summary>
        /// Creates chart data using the data set specified by <see cref="LastDataSet"/>.
        /// </summary>
        public void CreateChartData()
        {
            CreateChartData(LastDataSet > 0 ? LastDataSet : 1);
        }

        /// <summary>
        /// Creates chart data using the specified data set.
        /// </summary>
        /// <param name="dataSet">The data set number. Between 1 and <see cref="DataSetCount"/>.</param>
        public void CreateChartData(int dataSet)
        {
            if (!IsLoaded)
            {
                deferredDataSet = dataSet;
            }
            else
            {
                if (LastDataSet != dataSet)
                {
                    OnCreateChartData(dataSet);
                    LastDataSet = dataSet;
                }
            }
        }

        /// <summary>
        /// When overridden in a derived class, creates chart data using the specified data set.
        /// </summary>
        /// <param name="dataSet">The data set number. Between 1 and <see cref="DataSetCount"/>.</param>
        protected abstract void OnCreateChartData(int dataSet);

        /// <summary>
        /// Called when <see cref="SelectedLegendItem"/> changes. 
        /// The base implementation sets <see cref="SelectedLegendIndex"/>
        /// from <paramref name="info"/>. Override in a derived class to take
        /// other action.
        /// </summary>
        /// <param name="info">The info. May be null.</param>
        protected virtual void OnSelectedLegendItemChanged(DataSeriesInfo info)
        {
            SelectedLegendIndex = (info != null) ? info.Index : -1;
        }
        #endregion

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

        #region Private methods
        private void ChartControlBaseLoaded(object sender, RoutedEventArgs e)
        {
            if (deferredDataSet > 0)
            {
                CreateChartData(deferredDataSet);
                deferredDataSet = 0;
            }
        }
        #endregion
    }
}
