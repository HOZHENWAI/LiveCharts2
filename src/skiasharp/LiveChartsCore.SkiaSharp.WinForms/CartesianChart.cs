﻿// The MIT License(MIT)

// Copyright(c) 2021 Alberto Rodriguez Orozco & LiveCharts Contributors

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using LiveChartsCore.Kernel;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.Drawing;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;

namespace LiveChartsCore.SkiaSharpView.WinForms
{
    public class CartesianChart : Chart, ICartesianChartView<SkiaSharpDrawingContext>
    {
        private readonly CollectionDeepObserver<ISeries> seriesObserver;
        private readonly CollectionDeepObserver<IAxis> xObserver;
        private readonly CollectionDeepObserver<IAxis> yObserver;
        private IEnumerable<ISeries> series = new List<ISeries>();
        private IEnumerable<IAxis> xAxes = new List<Axis> { new Axis() };
        private IEnumerable<IAxis> yAxes = new List<Axis> { new Axis() };

        public CartesianChart()
        {
            seriesObserver = new CollectionDeepObserver<ISeries>(OnDeepCollectionChanged, OnDeepCollectionPropertyChanged, true);
            xObserver = new CollectionDeepObserver<IAxis>(OnDeepCollectionChanged, OnDeepCollectionPropertyChanged, true);
            yObserver = new CollectionDeepObserver<IAxis>(OnDeepCollectionChanged, OnDeepCollectionPropertyChanged, true);
        }

        CartesianChart<SkiaSharpDrawingContext> ICartesianChartView<SkiaSharpDrawingContext>.Core => (CartesianChart<SkiaSharpDrawingContext>)core;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable<ISeries> Series
        {
            get => series;
            set
            {
                seriesObserver.Dispose(series);
                seriesObserver.Initialize(value);
                series = value;
                core?.Update();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable<IAxis> XAxes 
        { 
            get => xAxes;
            set 
            {
                xObserver.Dispose(xAxes);
                xObserver.Initialize(value);
                xAxes = value;
                core?.Update(); 
            } 
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable<IAxis> YAxes 
        {
            get => yAxes; 
            set 
            {
                yObserver.Dispose(yAxes);
                yObserver.Initialize(value);
                yAxes = value;
                core?.Update();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ZoomAndPanMode ZoomMode { get; set; } = LiveCharts.CurrentSettings.DefaultZoomMode;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double ZoomingSpeed { get; set; } = LiveCharts.CurrentSettings.DefaultZoomSpeed;

        protected override void InitializeCore()
        {
            core = new CartesianChart<SkiaSharpDrawingContext>(this, LiveChartsSkiaSharp.DefaultPlatformBuilder, motionCanvas.CanvasCore);
            //legend = Template.FindName("legend", this) as IChartLegend<SkiaSharpDrawingContext>;
            //tooltip = Template.FindName("tooltip", this) as IChartTooltip<SkiaSharpDrawingContext>;
            core.Update();
        }

        public PointF ScaleUIPoint(PointF point, int xAxisIndex = 0, int yAxisIndex = 0)
        {
            throw new System.NotImplementedException();
        }

        private void OnDeepCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (core == null) return;
            core.Update();
        }

        private void OnDeepCollectionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (core == null) return;
            core.Update();
        }
    }
}
