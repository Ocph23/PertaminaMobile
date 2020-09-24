using System.ComponentModel;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Widget;
using MobileApp.Controls;
using MobileApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PickerCustom), typeof(SearchBarCustomeRenderer))]
namespace MobileApp.Droid.Renderers
{
    class SearchBarCustomeRenderer : SearchBarRenderer
    {

        public SearchBarCustom ElementV2 => Element as SearchBarCustom;

        public SearchBarCustomeRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            var plateId = Resources.GetIdentifier("android:id/search_plate", null, null);
            var plate = Control.FindViewById(plateId);
            plate.SetBackgroundColor(Android.Graphics.Color.Transparent);
        }
    }
}