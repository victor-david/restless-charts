using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Restless.Controls.Chart
{
    public class AxisGrid : Panel
    {
        #region Private
        private readonly Path horzPath;
        private readonly Path vertPath;
        private readonly Axis xAxis;
        private readonly Axis yAxis;
        private Range xAxisRange;
        private Range yAxisRange;
        private readonly Border gridBorder;
        private GeometryGroup vertGeometryGroup;
        private GeometryGroup horzGeometryGroup;
        #endregion

        /************************************************************************/

        #region Public fields
        /// <summary>
        /// Gets the default brush for the grid lines.
        /// </summary>
        public static readonly Brush GridDefaultBrush = Axis.MinorTickDefaultBrush;

        /// <summary>
        /// Gets the default brush for the border of the grid axis.
        /// </summary>
        public static readonly Brush GridBorderDefaultBrush = Axis.MinorTickDefaultBrush;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes new instance of <see cref="AxisGrid"/> class.
        /// </summary>
        internal AxisGrid(Axis xAxis, Axis yAxis)
        {
            this.xAxis = xAxis ?? throw new ArgumentNullException(nameof(xAxis));
            this.yAxis = yAxis ?? throw new ArgumentNullException(nameof(yAxis));

            xAxis.MajorTickGeometryCreated += XAxisMajorTickGeometryCreated;
            yAxis.MajorTickGeometryCreated += YAxisMajorTickGeometryCreated;

            horzPath = new Path()
            {
                Stroke = GridDefaultBrush,
                StrokeThickness = 1.0
            };

            vertPath = new Path()
            {
                Stroke = GridDefaultBrush,
                StrokeThickness = 1.0
            };

            gridBorder = new Border()
            {
                BorderBrush = GridBorderDefaultBrush,
                BorderThickness = new Thickness(1),
            };

            Children.Add(horzPath);
            Children.Add(vertPath);
            Children.Add(gridBorder);

            Background = Brushes.Transparent;
        }
        #endregion

        /************************************************************************/

        #region Brushes
        /// <summary>
        /// Gets or sets the brush for the axis grid lines.
        /// </summary>
        public Brush GridBrush
        {
            get => horzPath.Stroke;
            internal set
            {
                horzPath.Stroke = value;
                vertPath.Stroke = value;
            }
        }

        /// <summary>
        /// Gets or sets the brush for the axis grid lines.
        /// </summary>
        public Brush GridBorderBrush
        {
            get => gridBorder.BorderBrush;
            internal set
            {
                gridBorder.BorderBrush = value;
            }
        }
        #endregion

        /************************************************************************/

        #region Protected Methods
        ///// <summary>
        ///// Measures the size in layout required for child elements and determines a size for the Figure. 
        ///// </summary>
        ///// <param name="availableSize">
        ///// The available size that this element can give to child elements.
        ///// Infinity can be specified as a value to indicate that the element will size to whatever content is available.
        ///// </param>
        ///// <returns>The size that this element determines it needs during layout, based on its calculations of child element sizes.</returns>
        //protected override Size MeasureOverride(Size availableSize)
        //{
        //    Size desiredSize = new Size
        //        (
        //            availableSize.Width.IsFinite() ? availableSize.Width : 1,
        //            availableSize.Height.IsFinite() ? availableSize.Height : 1
        //        );

        //    foreach (UIElement child in Children)
        //    {
        //        child.Measure(availableSize);
        //    }

        //    Debug.WriteLine($"AxisGrid Desired Size {desiredSize}");
        //    return desiredSize;
        //}

        /// <summary>
        /// Positions child elements and determines a size for a AxisGrid
        /// </summary>
        /// <param name="finalSize">The final area within the parent that AxisGrid should use to arrange itself and its children</param>
        /// <returns>The actual size used</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement child in Children)
            {
                child.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            }
            return finalSize;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            if (sizeInfo.WidthChanged)
            {
                CreateHorizontalGeometry(horzGeometryGroup);
            }

            if (sizeInfo.HeightChanged)
            {
                CreateVerticalGeometry(vertGeometryGroup);
            }
            base.OnRenderSizeChanged(sizeInfo);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            double factor = e.Delta < 0 ? 1.2 : 1 / 1.2;
            if (xAxisRange == null) xAxisRange = xAxis.Range;
            if (yAxisRange == null) yAxisRange = yAxis.Range;

            xAxis.Range = xAxis.Range.Zoom(factor);
            yAxis.Range = yAxis.Range.Zoom(factor);
            e.Handled = true;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
            {
                if (xAxisRange != null && yAxisRange != null)
                {
                    xAxis.Range = xAxisRange;
                    yAxis.Range = yAxisRange;
                }
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void XAxisMajorTickGeometryCreated(object sender, GeometryGroup group)
        {
            vertGeometryGroup = group;
            CreateVerticalGeometry(group);
        }

        private void YAxisMajorTickGeometryCreated(object sender, GeometryGroup group)
        {
            horzGeometryGroup = group;
            CreateHorizontalGeometry(group);
        }

        private void CreateVerticalGeometry(GeometryGroup group)
        {
            if (group != null)
            {
                GeometryGroup vertGroup = new GeometryGroup();

                foreach (var item in group.Children.OfType<LineGeometry>())
                {
                    LineGeometry line = new LineGeometry
                    {
                        StartPoint = new Point(item.StartPoint.X, 0),
                        EndPoint = new Point(item.StartPoint.X, ActualHeight)
                    };
                    vertGroup.Children.Add(line);
                }
                vertPath.Data = vertGroup;
            }
        }

        private void CreateHorizontalGeometry(GeometryGroup group)
        {
            if (group != null)
            {
                GeometryGroup horzGroup = new GeometryGroup();

                foreach (var item in group.Children.OfType<LineGeometry>())
                {
                    LineGeometry line = new LineGeometry
                    {
                        StartPoint = new Point(0, item.StartPoint.Y),
                        EndPoint = new Point(ActualWidth, item.StartPoint.Y)
                    };
                    horzGroup.Children.Add(line);
                }
                horzPath.Data = horzGroup;
            }
        }
        #endregion
    }
}
