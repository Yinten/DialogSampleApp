using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Dialog;
using System.Linq;
using Android.Graphics;
using Java.IO;

namespace DialogSampleApp
{
    //
    // NOTE: with the new update you will have to add all the dialog_* prefixes to your main application.
    //       This is because the current version of Mono for Android will not add resources from assemblies
    //       to the main application like it does for libraries in Android/Java/Eclipse...  This could
    //       change in a future version (it's slated for 1.0 post release) but for now, just add them 
    //       as in this sample...
    //
    [Activity(Label = "Android.Dialog Sample", MainLauncher = true, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class MainDialogActivity : DialogActivity
    {

        public static string DRAWING_SAVE_LOCATION = Android.OS.Environment.ExternalStorageDirectory + File.Separator + "drawing_save_location.png";
        public static string DRAWING_SAVE_LOCATION_ONE = Android.OS.Environment.ExternalStorageDirectory + File.Separator + "drawing_save_location_one.png";
        public static string DRAWING_SAVE_LOCATION_TWO = Android.OS.Environment.ExternalStorageDirectory + File.Separator + "drawing_save_location_two.png";
        public static string DRAWING_SAVE_LOCATION_THREE = Android.OS.Environment.ExternalStorageDirectory + File.Separator + "drawing_save_location_three.png";
        protected void StartNew()
        {
            StartActivity(typeof(DialogListViewActivity));
        }

        protected void ClickList()
        {
            StartActivity(typeof(NameActivity));
        }

        protected void ClickElementTest()
        {
            StartActivity(typeof(EntryActivity));
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Root = new RootElement("Test Root Elem")
            {
                new Section
                {
                    new StringElement("Label", "Only Element in a Blank Section"),
                },
                new Section("Test Header", "Test Footer")
                {
                    new ButtonElement("DialogActivity", (o, e) => StartNew()),
                    new StringElement("DialogListView Activity", (int)DroidResources.ElementLayout.dialog_labelfieldright)
                    {
                        Click = (o, e) => ClickList(),
                    },
                    new BooleanElement("Push my button", true),
                    new BooleanElement("Push this too", false),
                    new StringElement("Text label", "Click for EntryElement Test")
                    {
                        Click = (o, e) => ClickElementTest(),
                    },
                },
                new Section("Images")
                {
                    new DrawingElement("Signature Line", BitmapFactory.DecodeResource(Resources, Resource.Drawable.sigline), DRAWING_SAVE_LOCATION),
                    new DrawingElement("Signature Way Too Big of Image", 
                                       BitmapFactory.DecodeResource(Resources, Resource.Drawable.siglineverybig), DRAWING_SAVE_LOCATION_ONE),
                    new DrawingElement("MDPI MAX Drawing Field", 
                                       BitmapFactory.DecodeResource(Resources, Resource.Drawable.oddlyshapeddrawingfieldmdpimax), DRAWING_SAVE_LOCATION_TWO),
                    new DrawingElement("Very Oddly Shaped Drawing Field With A lot of Chars in Description", 
                                       BitmapFactory.DecodeResource(Resources, Resource.Drawable.veryoddlyshapeddrawingfield), DRAWING_SAVE_LOCATION_THREE)

                },
                new Section("Part II")
                {
                    new StringElement("This is the String Element", "The Value"),
                    new CheckboxElement("Check this out", true),
                    new EntryElement("Username", string.Empty){ Hint = "Enter Login", },
                    new EntryElement("Password", string.Empty) {
                        Hint = "Enter Password",
                        Password = true,
                    },
                },
                new Section("Group", new ViewElement(Android.Resource.Layout.SimpleListItem1)
                    { Populate = view => { view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = "Custom footer view"; }, })
                {
                    new RootElement("Radio Group", new Android.Dialog.RadioGroup("dessert", 2))
                    {
                        new Section
                        {
                            new RadioElement("Ice Cream Sandwich", "dessert"),
                            new RadioElement("Honeycomb", "dessert"),
                            new RadioElement("Gingerbread", "dessert"),
                        },
                    } as Element,
                }
            };

            ValueChanged += root_ValueChanged;
        }

        void root_ValueChanged(object sender, System.EventArgs e)
        {
            Toast.MakeText(this, "Changed", ToastLength.Short).Show();
        }
    }
}