//
// LineRasterization.cs
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
using System.Collections.Generic;

namespace RasterizationAlgorithms
{
    public static class LineRasterization
    {
     

        public static IEnumerable<Cairo.Point> Bresenham (int x0, int y0, int x1, int y1)
        {
            bool steep = Math.Abs (y1 - y0) > Math.Abs (x1 - x0);
            if (steep) {
                int t;
                t = x0; // swap x0 and y0
                x0 = y0;
                y0 = t;
                t = x1; // swap x1 and y1
                x1 = y1;
                y1 = t;
            }
            if (x0 > x1) {
                int t;
                t = x0; // swap x0 and x1
                x0 = x1;
                x1 = t;
                t = y0; // swap y0 and y1
                y0 = y1;
                y1 = t;
            }
            int dx = x1 - x0;
            int dy = Math.Abs (y1 - y0);
            int error = dx / 2;
            int ystep = (y0 < y1) ? 1 : -1;
            int y = y0;
            for (int x = x0; x <= x1; x++) {
                yield return new Cairo.Point ((steep ? y : x), (steep ? x : y));
                error -= dy;
                if (error < 0) {
                    y += ystep;
                    error += dx;
                }
            }
            yield break;
        }
    
        public static IEnumerable<Cairo.Point> DDA (int x1, int y1, int x2, int y2)
        {  
            double x = x1;
            double y = y1;

            var dx = x2 - x;
            var dy = y2 - y;

            int steps;
            if (Math.Abs (dx) >= Math.Abs (dy))
                steps = (int)Math.Abs (dx);
            else
                steps = (int)Math.Abs (dy);

            yield return new Cairo.Point ((int)x, (int)y);
 
            dx /= steps;
            dy /= steps;

            for (var i=1; i <= steps; i++) {
                x += dx;
                y += dy;
                yield return new Cairo.Point ((int)x, (int)y);
            }    
        }
    }
}

