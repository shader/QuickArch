﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QuickArch
{
    public class MyThumb : Thumb
    {
        #region Properties
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(MyThumb), new UIPropertyMetadata(""));
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(string), typeof(MyThumb), new UIPropertyMetadata(""));

        // This property will handle the content of the textbox element taken from control template
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public Guid ID
        {
            get { return id; }
        }
        public Point Position
        {
            get { return position; }
        }

        // This property will handle the content of the image element taken from control template
        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
        #endregion

        #region Class Variables
        public List<LineGeometry> EndLines { get; private set; }
        public List<LineGeometry> StartLines { get; private set; }
        private Guid id;
        private Point position;
        #endregion

        #region Constructors
        public MyThumb() : base()
        {
            StartLines = new List<LineGeometry>();
            EndLines = new List<LineGeometry>();
            id = Guid.NewGuid();
            position = new Point();
        }

        public MyThumb(ControlTemplate template, string title, Point position) : this()
        {
            this.Template = template;
            this.Title = (title != null) ? title : string.Empty;
            this.SetPosition(position);
        }

        public MyThumb(ControlTemplate template, string title, string imageSource, Point position, DragDeltaEventHandler dragDelta) : this(template, title, position)
        {
            this.DragDelta += dragDelta;
        }
        #endregion

        // Helper method for setting the position of our thumb
        public void SetPosition(Point value)
        {
            Canvas.SetLeft(this, value.X);
            Canvas.SetTop(this, value.Y);
        }

        #region Linking logic
        // This method establishes a link between current thumb and specified thumb.
        // Returns a line geometry with updated positions to be processed outside.
        public LineGeometry LinkTo(MyThumb target)
        {
            // Create new line geometry
            LineGeometry line = new LineGeometry();
            // Save as starting line for current thumb
            this.StartLines.Add(line);
            // Save as ending line for target thumb
            target.EndLines.Add(line);
            // Ensure both thumbs the latest layout
            this.UpdateLayout();
            target.UpdateLayout();
            // Update line position
            line.StartPoint = new Point(Canvas.GetLeft(this) + this.ActualWidth / 2, Canvas.GetTop(this) + this.ActualHeight / 2);
            line.EndPoint = new Point(Canvas.GetLeft(target) + target.ActualWidth / 2, Canvas.GetTop(target) + target.ActualHeight / 2);
            // return line for further processing
            return line;
        }

        // This method establishes a link between current thumb and target thumb using a predefined line geometry
        // Note: this is commonly to be used for drawing links with mouse when the line object is predefined outside this class
        public bool LinkTo(MyThumb target, LineGeometry line)
        {
            // Save as starting line for current thumb
            this.StartLines.Add(line);
            // Save as ending line for target thumb
            target.EndLines.Add(line);
            // Ensure both tumbs the latest layout
            this.UpdateLayout();
            target.UpdateLayout();
            // Update line position
            line.StartPoint = new Point(Canvas.GetLeft(this) + this.ActualWidth / 2, Canvas.GetTop(this) + this.ActualHeight / 2);
            line.EndPoint = new Point(Canvas.GetLeft(target) + target.ActualWidth / 2, Canvas.GetTop(target) + target.ActualHeight / 2);
            return true;
        }
        #endregion

        // This method updates all the starting and ending lines assigned for the given thumb 
        // according to the latest known thumb position on the canvas
        public void UpdateLinks()
        {
            double left = Canvas.GetLeft(this);
            double top = Canvas.GetTop(this);

            for (int i = 0; i < this.StartLines.Count; i++)
                this.StartLines[i].StartPoint = new Point(left + this.ActualWidth / 2, top + this.ActualHeight / 2);

            for (int i = 0; i < this.EndLines.Count; i++)
                this.EndLines[i].EndPoint = new Point(left + this.ActualWidth / 2, top + this.ActualHeight / 2);
        }
    }
}
