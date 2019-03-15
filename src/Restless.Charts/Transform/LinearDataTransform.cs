using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Provides linear transform u = <see cref="Scale"/> * d + <see cref="Offset"/> from data value d to plot coordinate u.
    /// </summary>
    public class LinearDataTransform : DataTransform
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="LinearDataTransform"/> class.
        /// </summary>
        public LinearDataTransform() : base(new Range(double.MinValue, double.MaxValue))
        {
        }
        #endregion

        /************************************************************************/

        #region Scale
        /// <summary>
        /// Gets or sets the scale factor.
        /// </summary>
        public double Scale
        {
            get => (double)GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }
        /// <summary>
        /// Identifies the <see cref="Scale"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register
            (
                nameof(Scale), typeof(double), typeof(LinearDataTransform), new PropertyMetadata(1.0)
            );
        #endregion

        /************************************************************************/

        #region Offset
        /// <summary>
        /// Gets or sets the distance to translate an value.
        /// </summary>
        public double Offset
        {
            get => (double)GetValue(OffsetProperty);
            set => SetValue(OffsetProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Offset"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register
            (
                nameof(Offset), typeof(double), typeof(LinearDataTransform), new PropertyMetadata(0.0)
            );
        #endregion

        /************************************************************************/

        #region Methods
        /// <summary>
        /// Transforms a value according to defined <see cref="Scale"/> and <see cref="Offset"/>.
        /// </summary>
        /// <param name="dataValue">A value in data coordinates.</param>
        /// <returns>Transformed value.</returns>
        public override double DataToPlot(double dataValue)
        {
            return dataValue * Scale + Offset;
        }

        /// <summary>
        /// Returns a value in data coordinates from its transformed value.
        /// </summary>
        /// <param name="plotValue">Transformed value.</param>
        /// <returns>Original value or NaN if <see cref="Scale"/> is 0.</returns>
        public override double PlotToData(double plotValue)
        {
            if (Scale != 0)
            {
                return (plotValue - Offset) / Scale;
            }
            else
                return double.NaN;
        }
        #endregion
    }
}