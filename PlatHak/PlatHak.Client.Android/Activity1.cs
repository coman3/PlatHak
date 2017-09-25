using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using PlatHak.Client.Android.Managers;
using PlatHak.Client.Game;

namespace PlatHak.Client.Android
{
    [Activity(Label = "PlatHak", 
        MainLauncher = true, 
        Icon = "@drawable/icon", 
        Theme = "@style/Theme.Splash", 
        AlwaysRetainTaskState = true,
        Immersive = true,
        ScreenOrientation = ScreenOrientation.Landscape,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var g = new PlatHakGame(new AndroidInputManager());
            View vw = (View)g.Services.GetService(typeof(View));
            vw.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.HideNavigation | (StatusBarVisibility)SystemUiFlags.ImmersiveSticky;
            vw.SetOnSystemUiVisibilityChangeListener(new MyUiVisibilityChangeListener(vw));
            SetContentView((View) g.Services.GetService(typeof(View)));
            g.Run();
        }

        private class MyUiVisibilityChangeListener : Java.Lang.Object, View.IOnSystemUiVisibilityChangeListener
        {
            readonly View _targetView;

            public MyUiVisibilityChangeListener(View v)
            {
                _targetView = v;
            }

            public void OnSystemUiVisibilityChange(StatusBarVisibility v)
            {
                if (_targetView.SystemUiVisibility != ((StatusBarVisibility) SystemUiFlags.HideNavigation | (StatusBarVisibility) SystemUiFlags.Immersive))
                {
                    _targetView.SystemUiVisibility = (StatusBarVisibility) SystemUiFlags.HideNavigation | (StatusBarVisibility) SystemUiFlags.ImmersiveSticky;
                }
            }
        }
    }
}

