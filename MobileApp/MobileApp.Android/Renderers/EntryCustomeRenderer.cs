﻿using System.ComponentModel;
using Android.Content;
using Android.Graphics.Drawables;
using MobileApp.Controls;
using MobileApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(EntryCustom), typeof(EntryCustomeRenderer))]
namespace MobileApp.Droid.Renderers
{
    class EntryCustomeRenderer : EntryRenderer
    {

        public EntryCustom ElementV2 => Element as EntryCustom;

        public EntryCustomeRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundColor(global::Android.Graphics.Color.Transparent);


            }
        }

        protected override FormsEditText CreateNativeControl()
        {
            var control = base.CreateNativeControl();
            Update(control);
            return control;
        }

        private void Update(FormsEditText control)
        {
            if (control == null) return;

            var gd = new GradientDrawable();
            gd.SetCornerRadius(Context.ToPixels(ElementV2.CornerRadius));
            gd.SetStroke((int)Context.ToPixels(ElementV2.BorderThickness), ElementV2.BorderColor.ToAndroid());
            control.SetBackground(gd);
            control.SetPadding((int)Context.ToPixels(ElementV2.Padding.Left), (int)Context.ToPixels(ElementV2.Padding.Top),
                (int)Context.ToPixels(ElementV2.Padding.Right), (int)Context.ToPixels(ElementV2.Padding.Bottom));
        }
    }
}