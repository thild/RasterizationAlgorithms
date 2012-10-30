//
// RasterDrawingArea.cs
//
// Author:
//       Tony Alexander Hild <tony_hild@yahoo.com>
//
// Copyright (c) 2012 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using Cairo;
using Gdk;
using System.Collections.Generic;
using System.Linq;

namespace RasterizationAlgorithms
{
    public enum DrawType
    {
        Vectorial,
        Rasterized
    }

    public class RasterDrawingArea
    {

        public RasterDrawingArea (Gdk.Drawable drawable)
        {
            GridSize = 10;
            DrawType = DrawType.Vectorial;
            Drawable = drawable;
            Points = new List<Cairo.Point> ();
        }

        public int GridSize { get; set; }

        public DrawType DrawType { get; set; }

        public IList<Cairo.Point> Points { get; private set; }

        public Gdk.Drawable Drawable { get; set; }

        Size GetSize (Drawable window)
        {
            int w, h;
            window.GetSize (out w, out h);
            return new Size (w, h);
        }

        public Cairo.Point ConvertPoint (double x, double y)
        {
            var modX = x % GridSize;
            var modY = y % GridSize;
            x -= modX;
            x /= GridSize;
            y -= modY;
            y /= GridSize;
            return new Cairo.Point ((int)x, (int)y);
        }

        public void DrawGrid ()
        {
            using (Context g = CairoHelper.Create (Drawable)) {
                var size = GetSize (Drawable);
                g.DrawGrid (0, 0, size.Width, size.Height, GridSize);
            }
        }

        public void DrawLines ()
        {
            using (Context g = CairoHelper.Create (Drawable)) {
            
                g.Color = new Cairo.Color (0, 1, 0, 0.5);
                g.LineCap = LineCap.Round;
                g.LineWidth = GridSize;
                if (DrawType == RasterizationAlgorithms.DrawType.Vectorial) {
                    g.Antialias = Antialias.Subpixel;
                    for (int i = 0; i < Points.Count - 1; i++) {
                        var p = Points [i];
                        g.MoveTo (p.X, p.Y);
                        p = Points [i + 1];
                        g.LineTo (p.X, p.Y);
                    }
                    g.Stroke ();

                } else {
                    for (int i = 0; i < Points.Count - 1; i++) {
                        g.Color = new Cairo.Color (0, 1, 0, 0.05);
                        var rasterPoints = LineRasterization.Bresenham (Points [i].X, Points [i].Y, Points [i + 1].X, Points [i + 1].Y).ToList ();
                        for (int j = 1; j < rasterPoints.Count; j++) {
                            var prev = rasterPoints [j];
                            var p = ConvertPoint (prev.X, prev.Y);
                            g.Rectangle (new Cairo.Rectangle (p.X * GridSize, p.Y * GridSize, GridSize, GridSize));
                            g.Fill ();
                        }
                        var vecLine = Points [i];
                        g.LineCap = LineCap.Round;
                        g.MoveTo (vecLine.X, vecLine.Y);
                        vecLine = Points [i + 1];
                        g.LineTo (vecLine.X, vecLine.Y);
                        g.LineWidth = 2;
                        g.Color = new Cairo.Color (1, 0, 0);
                        g.Antialias = Antialias.Subpixel;
                        g.Stroke ();
                    }
                }
            }
        }
    }
}

