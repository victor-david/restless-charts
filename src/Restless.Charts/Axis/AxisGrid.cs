using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a panel that displays horizontal and vertical grid lines
    /// inside the chart area.
    /// </summary>
    public class AxisGrid : ChartBase
    {
        #region Private
        private readonly ChartContainer owner;
        private readonly Pen gridPen;
        private double minEdgeDistance;
        private bool isGridVisible;
        #endregion

        /************************************************************************/

        #region Public fields
        /// <summary>
        /// Gets the default brush for the grid lines.
        /// </summary>
        public static readonly Brush DefaultGridBrush = Axis.DefaultMinorTickBrush;

        /// <summary>
        /// Gets the default brush for the border of the grid axis.
        /// </summary>
        public static readonly Brush DefaultBorderBrush = Axis.DefaultMinorTickBrush;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes new instance of the <see cref="AxisGrid"/> class.
        /// </summary>
        internal AxisGrid(ChartContainer owner)
        {
            this.owner = owner ?? throw new ArgumentNullException(nameof(owner));

            // AxisGrid doesn't get its data from this collection,
            // but it needs to be non null for CreateChildren to be called.
            Data = DataSeries.Create();

            gridPen = new Pen(DefaultGridBrush, 1.0);

            minEdgeDistance = 3.0;
            IsGridVisible = true;
            IsHitTestVisible = false;
        }
        #endregion

        /************************************************************************/

        #region Brushes
        /// <summary>
        /// Gets or sets the brush for the axis grid lines.
        /// </summary>
        public Brush GridBrush
        {
            get => gridPen.Brush;
            internal set
            {
                gridPen.Brush = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum distance that a grid line can be from the edge.
        /// This value is clamped between 1.0 and 8.0. The default is 3.0
        /// </summary>
        public double MinEdgeDistance
        {
            get => minEdgeDistance;
            set
            {
                value = value.Clamp(1.0, 8.0);
                if (value != minEdgeDistance)
                {
                    minEdgeDistance = value;
                    InvalidateMeasure();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value that determines if the axis grid is visible.
        /// </summary>
        public bool IsGridVisible
        {
            get => isGridVisible;
            set
            {
                if (value != isGridVisible)
                {
                    isGridVisible = value;
                    Visibility = isGridVisible ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }
        #endregion

        /************************************************************************/

        #region Protected Methods
        /// <summary>
        /// Called by <see cref="ChartBase"/> to create child visuals.
        /// </summary>
        /// <param name="desiredSize">The desired size</param>
        protected override void CreateChildren(Size desiredSize)
        {
            if (Owner.Orientation == Orientation.Vertical)
            {
                CreateVerticalGeometry(owner.XAxis.MajorTickCoordinates, desiredSize);
                CreateHorizontalGeometry(owner.YAxis.MajorTickCoordinates, desiredSize);
            }
            else
            {
                CreateVerticalGeometry(owner.YAxis.MajorTickCoordinates, desiredSize);
                CreateHorizontalGeometry(owner.XAxis.MajorTickCoordinates, desiredSize);
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void CreateVerticalGeometry(TickCoordinateCollection tickCoordinates, Size size)
        {
            foreach (double tickCoordinate in tickCoordinates)
            {
                double x = tickCoordinate - owner.GridBorderSize;

                // don't make grid lines too close to the edge
                if (x > MinEdgeDistance && x < size.Width - MinEdgeDistance)
                {
                    Children.Add(CreateLineVisual(gridPen, x, 0, x, size.Height));
                }
            }
        }

        private void CreateHorizontalGeometry(TickCoordinateCollection tickCoordinates, Size size)
        {
            foreach (double tickCoordinate in tickCoordinates)
            {
                double y = tickCoordinate - owner.GridBorderSize;

                // don't make grid lines too close to the edge
                if (y > MinEdgeDistance && y < size.Height - MinEdgeDistance)
                {
                    Children.Add(CreateLineVisual(gridPen, 0, y, size.Width, y));
                }
            }
        }
        #endregion
    }
}