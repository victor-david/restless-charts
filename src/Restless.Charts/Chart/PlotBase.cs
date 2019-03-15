using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a base for all elements that perform graphic plotting.
    /// This class must be inherited.
    /// </summary>
    public abstract class PlotBase : Panel
    {
        #region Constructors
        /// <summary>
        /// Initializes new instance of <see cref="PlotBase"/> class
        /// </summary>
        protected PlotBase()
        {
            XDataTransform = DataTransform.Identity;
            YDataTransform = DataTransform.Identity;
            //masterField = this;
            //Loaded += PlotBaseLoaded;
            //Unloaded += PlotBaseUnloaded;
        }
        #endregion

        /************************************************************************/

        #region IsXAxisReversed
        /// <summary>
        /// Gets or sets a flag that determines whether the x-axis is reversed.
        /// </summary>
        public bool IsXAxisReversed
        {
            get => (bool)GetValue(IsXAxisReversedProperty);
            set => SetValue(IsXAxisReversedProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsXAxisReversed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsXAxisReversedProperty = DependencyProperty.Register
            (
                nameof(IsXAxisReversed), typeof(bool), typeof(PlotBase), new PropertyMetadata(false)
            );
        #endregion

        /************************************************************************/

        #region IsYAxisReversed
        /// <summary>
        /// Gets or sets a flag that determines whether the y-axis is reversed.
        /// </summary>
        public bool IsYAxisReversed
        {
            get => (bool)GetValue(IsYAxisReversedProperty);
            set => SetValue(IsYAxisReversedProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsYAxisReversed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsYAxisReversedProperty = DependencyProperty.Register
            (
                nameof(IsYAxisReversed), typeof(bool), typeof(PlotBase), new PropertyMetadata(false)
            );
        #endregion

        /************************************************************************/

        #region XDataTransform
        /// <summary>
        /// Gets or sets the transform from user data to horizontal plot coordinate. 
        /// By default transform is <see cref="IdentityDataTransform"/>
        /// </summary>
        public DataTransform XDataTransform
        {
            get => (DataTransform)GetValue(XDataTransformProperty);
            set => SetValue(XDataTransformProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref=" XDataTransform"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty XDataTransformProperty = DependencyProperty.Register
            (
                nameof(XDataTransform), typeof(DataTransform), typeof(PlotBase), new PropertyMetadata(null, OnXDataTransformChanged)
            );

        private static void OnXDataTransformChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PlotBase c)
            {
                // c.OnXDataTransformChanged(e);
            }
        }

        #endregion

        /************************************************************************/

        #region YDataTransform
        /// <summary>
        /// Gets or sets the transform from user data to vertical plot coordinate. 
        /// By default transform is <see cref="IdentityDataTransform"/>
        /// </summary>
        public DataTransform YDataTransform
        {
            get => (DataTransform)GetValue(YDataTransformProperty);
            set => SetValue(YDataTransformProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref=" YDataTransform"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty YDataTransformProperty = DependencyProperty.Register
            (
                nameof(YDataTransform), typeof(DataTransform), typeof(PlotBase), new PropertyMetadata(null, OnYDataTransformChanged)
            );

        private static void OnYDataTransformChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PlotBase c)
            {
                // c.OnYDataTransformChanged(e);
            }
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Finds master plot for specified element
        /// </summary>
        /// <param name="element">Element, which mater plot should be found</param>
        /// <returns>Master Plot for specified element</returns>
        public static PlotBase FindMaster(DependencyObject element)
        {
            DependencyObject result = VisualTreeHelper.GetParent(element);

            while (result != null && !(result is PlotBase))
            {
                result = VisualTreeHelper.GetParent(result);
            }

            return result as PlotBase;
        }
        #endregion






    }
}
