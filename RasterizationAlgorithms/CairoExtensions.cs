//
// CairoExtensions.cs
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

namespace RasterizationAlgorithms
{
    public static class CairoExtensions
    {
        public static void DrawPoint (this Context g, int x, int y, int width)
        {
            g.LineCap = LineCap.Round;
            g.LineWidth = width;
            g.MoveTo (x, y);
            g.LineTo (x, y);
            g.Stroke ();
        }

        public static void DrawGrid (this Context g, int x, int y, int width, int height, int cellSize)
        {
            var cellSizePlus2 = cellSize * 2;
            for (int yy = x; yy < height; yy += cellSize) {
                bool flag = false;
                for (int xx = yy % cellSizePlus2 == 0 ? 0 : cellSize; xx < width; xx += cellSize) {
                        g.MoveTo (xx, yy);
                        g.Color = flag ? new Cairo.Color (1, 1, 1) : new Cairo.Color (0.80, 0.80, 0.80);
                        flag = !flag;
                        g.Rectangle (xx, yy, cellSize, cellSize);
                        g.Fill ();
                }
            }
        }
    }
}

