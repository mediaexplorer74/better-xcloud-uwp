using System;
using Windows.Gaming.Input;
using System.Collections.Generic;

namespace BetterXCloudUWP.Services
{
    public class GamepadService
    {
        public event Action<string> GamepadStatusChanged;
        public event Action<string> GamepadButtonPressed;
        private List<Gamepad> _gamepads = new List<Gamepad>();

        public GamepadService()
        {
            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;
        }

        private void Gamepad_GamepadAdded(object sender, Gamepad e)
        {
            _gamepads.Add(e);
            GamepadStatusChanged?.Invoke("Геймпад подключен");
        }

        private void Gamepad_GamepadRemoved(object sender, Gamepad e)
        {
            _gamepads.Remove(e);
            GamepadStatusChanged?.Invoke("Геймпад отключен");
        }

        public void PollGamepads()
        {
            foreach (var gamepad in Gamepad.Gamepads)
            {
                var reading = gamepad.GetCurrentReading();
                if ((reading.Buttons & GamepadButtons.A) != 0)
                    GamepadButtonPressed?.Invoke("A");
                if ((reading.Buttons & GamepadButtons.B) != 0)
                    GamepadButtonPressed?.Invoke("B");
                if ((reading.Buttons & GamepadButtons.X) != 0)
                    GamepadButtonPressed?.Invoke("X");
                if ((reading.Buttons & GamepadButtons.Y) != 0)
                    GamepadButtonPressed?.Invoke("Y");
            }
        }
    }
}
