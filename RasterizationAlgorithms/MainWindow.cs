//
// MainWindow.cs
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
using Gtk;
using Gdk;
using Cairo;
using System.Collections.Generic;
using System.Linq;
using RasterizationAlgorithms;

public partial class MainWindow: Gtk.Window
{
    public MainWindow (): base (Gtk.WindowType.Toplevel)
    {
        Build ();

        var rda = new RasterDrawingArea(canvas.GdkWindow);
        rda.GridSize = 20;

        btnReset.AddEvents ((int)EventMask.AllEventsMask);
        btnDrawLines.AddEvents ((int)EventMask.AllEventsMask);

        
        btnReset.Clicked += (sender, e) => {
            rda.DrawGrid ();
            rda.Points.Clear ();
        };

        btnDrawLines.Clicked += (sender, e) => {
            rda.DrawType = rbtVectorial.Active ? DrawType.Vectorial : DrawType.Rasterized;
            rda.DrawLines();
        };

        canvas.ModifyBg (StateType.Normal, new Gdk.Color (255, 255, 255));
        canvas.ModifyFg (StateType.Normal, new Gdk.Color (255, 255, 255));
        canvas.AddEvents ((int)EventMask.AllEventsMask);

        canvas.ExposeEvent += (o, args) => {
            rda.DrawGrid ();
        };

        canvas.MotionNotifyEvent += (o, args) => {
            var p = rda.ConvertPoint (args.Event.X, args.Event.Y);
            Title = string.Format ("{0} - {1}", p.X, p.Y);
        };


        canvas.ButtonPressEvent += (o, args) => {
            rda.Points.Add (new Cairo.Point ((int)args.Event.X, (int)args.Event.Y));
            using (Context g = CairoHelper.Create (canvas.GdkWindow)) {
                if (rbtVectorial.Active) {
                    g.Antialias = Antialias.Subpixel;
                    g.Color = new Cairo.Color (0, 1, 0);
                    g.DrawPoint ((int)args.Event.X, (int)args.Event.Y, rda.GridSize);
                    g.Color = new Cairo.Color (0, 0, 0);
                    g.LineWidth = 1;
                    g.Arc ((int)args.Event.X, (int)args.Event.Y, 10, 0, 360);
                    g.Stroke ();
                } else {
                    var p = rda.ConvertPoint ((int)args.Event.X, (int)args.Event.Y);
                    g.Color = new Cairo.Color (0, 1, 0);
                    g.Rectangle (new Cairo.Rectangle (p.X * rda.GridSize, p.Y * rda.GridSize, rda.GridSize, rda.GridSize));
                    g.Fill ();
                    g.Color = new Cairo.Color (0, 0, 0);
                    g.LineWidth = 1;
                    g.Rectangle (new Cairo.Rectangle (p.X * rda.GridSize, p.Y * rda.GridSize, rda.GridSize, rda.GridSize));
                    g.Stroke();
                }
            }
        };
    }
    
    protected void OnDeleteEvent (object sender, DeleteEventArgs a)
    {
        Application.Quit ();
        a.RetVal = true;
    }

  
}