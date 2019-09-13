using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IssueTapGestureTranslationX
{
    public partial class MainPage : ContentPage
    {
        private StackLayout ChildStackLayout;
        private StackLayout Child2StackLayout;
        public bool HasSwipe = false;
        bool _translatedLeftOrRight;
        private double _previousXTranslation;
        private double _previousXTranslationRight;
        public double SizeContent
        {
            get { return App.Current.MainPage.Width * 0.99; }
        }

        public double TranslationXForShops
        {
            get => App.Current.MainPage.Width + 5;
        }
        public MainPage()
        {
            InitializeComponent();
            Model model = new Model { FirstString = "First Stack", SecondString = "SecondStack" };
            List<Model> list = new List<Model>();
            list.Add(model);
            list.Add(model);
            list.Add(model);
            list.Add(model);
            ListElements.ItemsSource = list;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Console.WriteLine("okay");
        }


        private bool CanTriggerRightAction(double totalX)
        {
            return -1 * totalX > (Width / 3);
        }

        private bool CanTriggerLeftAction(double totalX)
        {
            return totalX > (Width / 3);
        }

        private async void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {

            var value = (ContentViewCustom)sender;
            Grid grid = (Grid)value.Content.FindByName("Grids");

            ChildStackLayout = (StackLayout)grid.Children.ElementAt(0);

            Child2StackLayout = (StackLayout)grid.Children.ElementAt(1);

            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        if (e.TotalX != 0)
                        {
                            ListElements.IsPullToRefreshEnabled = false;
                            ListElements.Unfocus();
                            HasSwipe = true;
                        }
                    }
                    else
                    {
                        ListElements.IsPullToRefreshEnabled = false;
                        ListElements.Unfocus();
                        HasSwipe = true;
                    }
                    break;

                case GestureStatus.Running:
                    
                    HandleTouch(e.TotalX, grid);
                    break;
                case GestureStatus.Completed:
                    
                    ListElements.IsPullToRefreshEnabled = true;
                    if (CanTriggerRightAction(_previousXTranslation))
                    {
                        await grid.TranslateTo(-TranslationXForShops, 0, length: 200, easing: Easing.CubicOut);

                    }
                    else if (CanTriggerLeftAction(_previousXTranslation))
                    {
                        await grid.TranslateTo(0, 0, length: 200, easing: Easing.CubicOut);
                        

                    }
                    else
                    {
                        if (grid.TranslationX != -TranslationXForShops && _translatedLeftOrRight)
                        {
                            await grid.TranslateTo(0, 0, length: 200, easing: Easing.CubicOut);
                        }
                        else if (!_translatedLeftOrRight && _previousXTranslation > 0)
                        {
                            await grid.TranslateTo(-TranslationXForShops, 0, length: 200, easing: Easing.CubicOut);
                        }

                    }
                    HasSwipe = false;
                    break;
                case GestureStatus.Canceled:
                    HasSwipe = false;
                    ListElements.IsPullToRefreshEnabled = true;
                    HandleTouch(0, grid, animated: false);
                    break;

            }

        }

        private void HandleTouch(double xTranslation, Grid stack, bool animated = true)
        {
            _previousXTranslation = xTranslation;
            Console.WriteLine(xTranslation + " !!" + stack.TranslationX);
            StackLayout ChildStackLayout;
            StackLayout Child2StackLayout;

            
            ChildStackLayout = (StackLayout)stack.Children.ElementAt(0);
            

            Child2StackLayout = (StackLayout)stack.Children.ElementAt(1);
           
            // scrolling to left 
            if (xTranslation < 0 && stack.TranslationX != -TranslationXForShops)
            {
               
                double totalX = Math.Max(xTranslation, -Width / 2);
                if (stack.TranslationX < -300)
                {
                    stack.TranslationX = -TranslationXForShops;
                }
                else
                {
                    stack.TranslationX = totalX;
                }

                _translatedLeftOrRight = true;
            }
            // scrolling to right 
            else if (xTranslation > 0 && stack.TranslationX != 0)
            {
                
                double totalX = Math.Min(xTranslation, Width / 2);
                if (xTranslation > 0 && stack.TranslationX > -50)
                {
                    stack.TranslationX = 0;
                    _translatedLeftOrRight = false;
                }
                else if (xTranslation <= 0.5)
                {
                    stack.TranslationX = -TranslationXForShops;
                    _translatedLeftOrRight = false;
                }
                else
                {
                    stack.TranslationX = totalX - TranslationXForShops;
                    _translatedLeftOrRight = false;

                    if (xTranslation <= 0.5)
                    {
                        stack.TranslationX = -TranslationXForShops;
                    }
                }

                if (xTranslation < _previousXTranslationRight)
                {
                    
                    _translatedLeftOrRight = false;
                    if (xTranslation <= 13)
                    {
                        stack.TranslationX = -TranslationXForShops;
                    }
                }
            }


            _previousXTranslationRight = xTranslation;
        }

        private void ListElements_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}
