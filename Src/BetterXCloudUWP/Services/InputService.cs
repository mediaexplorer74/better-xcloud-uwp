using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace BetterXCloudUWP.Services
{
    public class InputService
    {
        public event Action<string> InputEvent;

        public void Attach(UIElement element)
        {
            element.PointerPressed += Element_PointerPressed;
            element.KeyDown += Element_KeyDown;
            element.KeyUp += Element_KeyUp;
            element.PointerMoved += Element_PointerMoved;
            element.PointerReleased += Element_PointerReleased;
        }

        private void Element_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var pt = e.GetCurrentPoint((UIElement)sender);
            if (pt.Properties.IsLeftButtonPressed)
                InputEvent?.Invoke($"Мышь: ЛКМ ({pt.Position.X}, {pt.Position.Y})");
            else if (pt.Properties.IsRightButtonPressed)
                InputEvent?.Invoke($"Мышь: ПКМ ({pt.Position.X}, {pt.Position.Y})");
            else if (pt.PointerDevice.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Touch)
                InputEvent?.Invoke($"Touch: Tap ({pt.Position.X}, {pt.Position.Y})");
        }

        private void Element_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var pt = e.GetCurrentPoint((UIElement)sender);
            if (pt.PointerDevice.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Touch)
                InputEvent?.Invoke($"Touch: Move ({pt.Position.X}, {pt.Position.Y})");
        }

        private void Element_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var pt = e.GetCurrentPoint((UIElement)sender);
            if (pt.PointerDevice.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Touch)
                InputEvent?.Invoke($"Touch: Release ({pt.Position.X}, {pt.Position.Y})");
        }

        private void Element_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            InputEvent?.Invoke($"Клавиша нажата: {e.Key}");
        }

        private void Element_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            InputEvent?.Invoke($"Клавиша отпущена: {e.Key}");
        }
    }
}
