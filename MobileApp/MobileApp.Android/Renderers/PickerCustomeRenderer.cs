using System.ComponentModel;
using Android.Content;
using Android.Graphics.Drawables;
using MobileApp.Controls;
using MobileApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PickerCustom), typeof(PickerCustomeRenderer))]
namespace MobileApp.Droid.Renderers
{
    class PickerCustomeRenderer : PickerRenderer
    {

        public PickerCustom ElementV2 => Element as PickerCustom;

        public PickerCustomeRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundColor(global::Android.Graphics.Color.Transparent);
                var gd = new GradientDrawable();
                gd.SetCornerRadius(Context.ToPixels(ElementV2.CornerRadius));
                gd.SetStroke((int)Context.ToPixels(ElementV2.BorderThickness), ElementV2.BorderColor.ToAndroid());
                Control.SetBackground(gd);
                Control.SetPadding((int)Context.ToPixels(ElementV2.Padding.Left), (int)Context.ToPixels(ElementV2.Padding.Top),
                    (int)Context.ToPixels(ElementV2.Padding.Right), (int)Context.ToPixels(ElementV2.Padding.Bottom));

            }
        }
    }
}