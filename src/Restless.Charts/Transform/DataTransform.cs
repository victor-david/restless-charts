﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Performs transformations between data values and plot coordinates.
    /// This class must be inherited.
    /// </summary>
    public abstract class DataTransform : DependencyObject
    {
        /// <summary>
        /// Gets range of valid data values.
        /// <see cref="DataToPlot"/> method returns NaN for values outside this range. 
        /// </summary>
        public Range Domain
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DataTransform"/> class.
        /// </summary>
        /// <param name="domain">A range of valid data.</param>
        protected DataTransform(Range domain)
        {
            Domain = domain;
        }

        /// <summary>
        /// Converts value from data to plot coordinates.
        /// </summary>
        /// <param name="dataValue">A value in data coordinates.</param>
        /// <returns>
        /// Value converted to plot coordinates or NaN if <paramref name="dataValue"/>
        /// falls outside of <see cref="Domain"/>.
        /// </returns>
        public abstract double DataToPlot(double dataValue);

        /// <summary>
        /// Converts value from plot coordinates to data.
        /// </summary>
        /// <param name="plotValue">A value in plot coordinates.</param>
        /// <returns>
        /// Value converted to data coordinates or NaN if no value in data coordinates
        /// matches <paramref name="plotValue"/>.
        /// </returns>
        public abstract double PlotToData(double plotValue);

        /// <summary>
        /// Returns a static instance of <see cref="IdentityDataTransform"/>
        /// </summary>
        public static readonly DataTransform Identity = new IdentityDataTransform();
    }


}
