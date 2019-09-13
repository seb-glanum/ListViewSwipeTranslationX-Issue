using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace IssueTapGestureTranslationX
{
    public class ContentViewCustom : ContentView
    {
        //public bool AuthorizeDelete = true;
        public ContentViewCustom() : base()
        {
            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.iOS>().SetPanGestureRecognizerShouldRecognizeSimultaneously(true);

           
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }
    }
}
