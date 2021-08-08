using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows;
namespace CloudsMove.Controls
{
    public class MyTabControl : TabControl
    {

        Timer t = null;
        private int left = 0;
        private int AnimationIndex = 1;
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            Action a = () =>
            {
                try
                {
                    AnimationIndex++;
                    var control = e.Source as MyTabControl;
                    if (control == null) return;
                    base.OnSelectionChanged(e);
                    if (AnimationIndex % 2 == 0)
                    {
                        left = -150;
                    }
                    else
                    {
                        left = 150;
                    }

                    var selectItem = control.SelectedContent as ScrollViewer;
                    if (selectItem == null)
                    {

                        var selectItem_2 = control.SelectedContent as Grid;
                        if (selectItem_2 == null)
                        {
                            var selectItem_3 = control.SelectedContent as StackPanel;
                            if (selectItem_3 == null) return;
                            selectItem_3.Visibility = Visibility.Collapsed;
                            t = new Timer(Move, selectItem_3, 0, 5);
                            return;
                        }
                        selectItem_2.Visibility = Visibility.Collapsed;
                        t = new Timer(Move, selectItem_2, 0, 5);
                        return;
                    }
                    selectItem.Visibility = Visibility.Collapsed;

                    t = new Timer(Move, selectItem, 0, 2);

                }
                catch (Exception)
                {

                }
            };
            this.Dispatcher.BeginInvoke(a);
        }


        public void Move(object o)
        {

            Action a = () =>
            {
                try
                {
                    if (AnimationIndex % 2 == 0)
                    {
                        left += 6;
                        if (left > 5)
                        {
                            t.Dispose();

                            return;
                        }
                        AnimationIndex = 0;
                    }
                    else
                    {

                        left -= 6;

                        if (left <= 0)
                        {
                            t.Dispose();

                            return;
                        }
                        AnimationIndex = 1;
                    }

                    var selectItem = o as ScrollViewer;
                    if (selectItem == null)
                    {
                        var selectItem_2 = o as Grid;
                        if (selectItem_2 == null)
                        {
                            var selectItem_3 = o as StackPanel;
                            if (selectItem_3 == null) return;
                            selectItem_3.Visibility = Visibility.Visible;
                            selectItem_3.Margin = new Thickness(left, 0, 0, 0);

                            return;
                        }
                        selectItem_2.Visibility = Visibility.Visible;
                        selectItem_2.Margin = new Thickness(left, 0, 0, 0);
                        return;
                    }
                    selectItem.Visibility = Visibility.Visible;
                    selectItem.Margin = new Thickness(left, 0, 0, 0);
                }
                catch (Exception)
                {

                }

            };
            this.Dispatcher.BeginInvoke(a);
        }


    }
}
