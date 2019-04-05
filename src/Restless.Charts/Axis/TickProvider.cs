using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Provides mechanisms to generate ticks and their corresponding labels that are displayed on an axis.
    /// </summary>
    public class TickProvider
    {
        #region Private
        private const int MaxTickArrangeIterations = 120;
        private const int MaxTicks = 2000;
        private const int MinTicks = 1;
        private const double IncreaseRatio = 8.0;
        private const double DecreaseRatio = 8.0;

        private int delta = 1;
        private int beta = 0;

        private readonly MinorTickProvider minorProvider;
        //private double[] majorTickValues;
        //private TickText[] majorTickLabels;
        //private double[] minorTickValues;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of <see cref="TickProvider"/> class with default <see cref="MinorTickProvider"/>.
        /// </summary>
        internal TickProvider()
        {
            MajorTicks = new MajorTickCollection();
            MinorTicks = new DoubleCollection();
            minorProvider = new MinorTickProvider();
            //majorTickValues = new double[0];
            //majorTickLabels = new TickText[0];
            //minorTickValues = new double[0];
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the collection of <see cref="MajorTick"/> objects.
        /// </summary>
        public MajorTickCollection MajorTicks
        {
            get;
        }

        /// <summary>
        /// Gets a collection of double values that represent minor ticks.
        /// </summary>
        public DoubleCollection MinorTicks
        {
            get;
        }

        /// <summary>
        /// Gets or (from this assembly) sets the format used when converting a ticking value to text.
        /// </summary>
        public string TextFormat
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or (from this assembly) sets an <see cref="IDoubleConverter"/> to convert tick values to text.
        /// </summary>
        public IDoubleConverter TextProvider
        {
            get;
            internal set;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        ///// <summary>
        ///// Provides an enumerator that enumerates <see cref="MajorTick"/> objects of this instance.
        ///// </summary>
        ///// <returns>An enumerator</returns>
        //public IEnumerable<MajorTick> EnumerateMajorTicks()
        //{
        //    int len = majorTickValues.Length;
        //    if (len != majorTickLabels.Length)
        //    {
        //        throw new IndexOutOfRangeException($"Length mismatch. Ticks {len} Labels: {majorTickLabels.Length}");
        //    }

        //    for (int k = 0; k < len; k++)
        //    {
        //        yield return new MajorTick(majorTickValues[k], majorTickLabels[k]);
        //    }

        //    yield break;
        //}

        ///// <summary>
        ///// Provides an enumerator that enumerates minor ticks.
        ///// </summary>
        ///// <returns>An enumerator</returns>
        //public IEnumerable<double> EnumerateMinorTicks()
        //{
        //    for (int k = 0; k < minorTickValues.Length; k++)
        //    {
        //        yield return minorTickValues[k];
        //    }
        //    yield break;
        //}

        /// <summary>
        /// Creates major ticks with their corresponding labels and minor ticks.
        /// </summary>
        /// <param name="axisSize">The size of the axis.</param>
        /// <param name="range">The range of values.</param>
        /// <param name="labelValidator">A method that examines the size of the labels and returns an evaulation result.</param>
        /// <remarks>
        /// Once this method calculates ticks and creates labels, you can use
        /// <see cref="EnumerateMajorTicks"/> and <see cref="EnumerateMinorTicks"/>
        /// to enumerate the results.
        /// </remarks>
        public void CreateTicks(Size axisSize, Range range, Func<Size, TickEvaluationResult> labelValidator)
        {
            if (range == null) throw new ArgumentNullException(nameof(range));
            if (labelValidator == null) throw new ArgumentNullException(nameof(labelValidator));

            MajorTicks.Clear();
            MinorTicks.Clear();

            CreateMajorTicks(axisSize, range); //, labelValidator);
            CreateMinorTicks(range);
        }

        public void CreateTicks(Size axisSize, IEnumerable<double> enumerator)
        {
            if (enumerator == null) throw new ArgumentNullException(nameof(enumerator));

            MajorTicks.Clear();
            MinorTicks.Clear();

            foreach (double value in enumerator)
            {
                //Debug.WriteLine(value.ToString());
            }
        }

        ///// <summary>
        ///// Gets a size struct that represents the maximum width and maximum height of all major tick labels.
        ///// </summary>
        ///// <returns>A size structure.</returns>
        //public Size GetMaxTextSize()
        //{
        //    Size size = new Size();
        //    foreach (TickText text in majorTickLabels)
        //    {
        //        size.Width = Math.Max(size.Width, text.DesiredSize.Width);
        //        size.Height = Math.Max(size.Height, text.DesiredSize.Height);
        //    }
        //    return size;
        //}
        #endregion

        /************************************************************************/

        #region Private methods

        private void CreateMajorTicks(Size axisSize, Range range)
        {
            MajorTicks.Add(new MajorTick(16, new TickText() { Text = "TEST" }));
            //Size size = MajorTicks.GetMaxTextSize();
        }

        //private void CreateMajorTicksOriginal(Size axisSize, Range range, Func<Size, TickEvaluationResult> labelValidator)
        //{
        //    delta = 1;
        //    beta = (int)Math.Round(Math.Log10(range.Max - range.Min)) - 1;

        //    if (range.IsPoint)
        //    {
        //        majorTickValues = new double[] { range.Min };
        //        CreateMajorTickLabels(majorTickValues);
        //        return;
        //    }

        //    majorTickValues = GetMajorTicks(range);
        //    CreateMajorTickLabels(majorTickValues);

        //    TickEvaluationResult tickChange;
        //    if (majorTickValues.Length > MaxTicks)
        //        tickChange = TickEvaluationResult.Decrease;
        //    else if (majorTickValues.Length < MinTicks)
        //        tickChange = TickEvaluationResult.Increase;
        //    else
        //        tickChange = labelValidator(axisSize);

        //    int iterations = 0;
        //    int prevLength = majorTickValues.Length;

        //    while (tickChange != TickEvaluationResult.Ok && iterations++ < MaxTickArrangeIterations)
        //    {
        //        if (tickChange == TickEvaluationResult.Increase)
        //            IncreaseTickCount();
        //        else
        //            DecreaseTickCount();

        //        double[] newTicks = GetMajorTicks(range);
        //        if (newTicks.Length > MaxTicks && tickChange == TickEvaluationResult.Increase)
        //        {
        //            DecreaseTickCount(); // Step back and stop to not get more than MaxTicks
        //            break;
        //        }
        //        else if (newTicks.Length < MinTicks && tickChange == TickEvaluationResult.Decrease)
        //        {
        //            IncreaseTickCount(); // Step back and stop to not get less than 2
        //            break;
        //        }
        //        var prevTicks = majorTickValues;
        //        majorTickValues = newTicks;
        //        var prevLabels = majorTickLabels;
        //        CreateMajorTickLabels(newTicks);
        //        TickEvaluationResult newResult = labelValidator(axisSize);
        //        if (newResult == tickChange) // Continue in the same direction
        //        {
        //            prevLength = newTicks.Length;
        //        }
        //        else // Direction changed or layout OK - stop the loop
        //        {
        //            if (newResult != TickEvaluationResult.Ok) // Direction changed - time to stop
        //            {
        //                if (tickChange == TickEvaluationResult.Decrease)
        //                {
        //                    if (prevLength < MaxTicks)
        //                    {
        //                        majorTickValues = prevTicks;
        //                        majorTickLabels = prevLabels;
        //                        IncreaseTickCount();
        //                    }
        //                }
        //                else
        //                {
        //                    if (prevLength >= 2)
        //                    {
        //                        majorTickValues = prevTicks;
        //                        majorTickLabels = prevLabels;
        //                        DecreaseTickCount();
        //                    }
        //                }
        //                break;
        //            }
        //            break;
        //        }
        //    }
        //}

        /// <summary>
        /// Gets an array of ticks for specified axis range.
        /// </summary>
        private double[] GetMajorTicks(Range range)
        {
            Debug.WriteLine($"GetMajorTicks. Range {range}");

            double temp = delta * Math.Pow(10, beta);
            double min = Math.Floor(range.Min / temp);
            double max = Math.Floor(range.Max / temp);
            int count = (int)(max - min + 1);
            List<double> res = new List<double>();
            double x0 = min * temp;
            for (int i = 0; i < count + 1; i++)
            {
                double v = MathHelper.Round(x0 + i * temp, beta);
                if (range.Includes(v)) res.Add(v);
            }
            return res.ToArray();
        }

        /// <summary>
        /// Increases the tick count.
        /// </summary>
        private void IncreaseTickCount()
        {
            switch (delta)
            {
                case 1:
                    delta = 5;
                    beta--;
                    break;
                case 2:
                    delta = 1;
                    break;
                case 5:
                    delta = 2;
                    break;
            }
        }

        /// <summary>
        /// Decreases the tick count.
        /// </summary>
        private void DecreaseTickCount()
        {
            switch (delta)
            {
                case 1:
                    delta = 2;
                    break;
                case 2:
                    delta = 5;
                    break;
                case 5:
                    delta = 1;
                    beta++;
                    break;
            }
        }

        /// <summary>
        /// Generates minor ticks in specified range.
        /// </summary>
        /// <param name="range">The range.</param>
        private void CreateMinorTicks(Range range)
        {
            //var ticks = new List<double>(majorTickValues);
            //double temp = delta * Math.Pow(10, beta);
            //ticks.Insert(0, MathHelper.Round(ticks[0] - temp, beta));
            //ticks.Add(MathHelper.Round(ticks[ticks.Count - 1] + temp, beta));
            //minorTickValues = minorProvider.CreateTicks(range, ticks.ToArray());
        }

        ///// <summary>
        ///// Creates the major tick labels from the specified ticks.
        ///// </summary>
        ///// <param name="ticks">An array of double ticks.</param>
        //private void CreateMajorTickLabels(double[] ticks)
        //{
        //    if (ticks == null) throw new ArgumentNullException(nameof(ticks));

        //    majorTickLabels = new TickText[ticks.Length];

        //    for (int idx = 0; idx < ticks.Length; idx++)
        //    {
        //        majorTickLabels[idx] = new TickText
        //        {
        //            Text = (TextProvider != null) ? TextProvider.Convert(ticks[idx], TextFormat) : ticks[idx].ToString(TextFormat)
        //        };
        //    }
        //}
        #endregion
    }
}

