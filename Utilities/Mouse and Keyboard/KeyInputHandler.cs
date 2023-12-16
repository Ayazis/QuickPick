using System.Windows.Forms;

namespace Utilities.Mouse_and_Keyboard
{
    public class KeyInputHandler
    {
        private static KeyInputHandler _instance;
        public static KeyInputHandler Instance => _instance ??= new KeyInputHandler();

		private HashSet<Keys> _pressedKeys = new HashSet<Keys>();
        private HashSet<Keys> _presetKeyCombination;


        private KeyInputHandler()
        {
            // Prevent construction outside this class.
        }

        public void SetKeycombination(IEnumerable<Keys> presetKeyCombination)
        {
            if (presetKeyCombination == null || !presetKeyCombination.Any())
            {
                throw new ArgumentNullException(nameof(presetKeyCombination), "Preset key combination must not be null or empty.");
            }

            _presetKeyCombination = new HashSet<Keys>(presetKeyCombination);
        }
        public delegate void KeyCombinationHitEventHandler();
        public event KeyCombinationHitEventHandler KeyCombinationHit;

        public bool IsPresetKeyCombinationHit(Keys key)
        {
            if (_presetKeyCombination.Contains(key))
            {
                _pressedKeys.Add(key);  // If it's already there, it won't be added again.

                if (_pressedKeys.Count == _presetKeyCombination.Count)
                {
                    return IsHotKeyCombinationHit();
                }
            }
            return false;
        }

        public void KeyReleased(Keys key)
        {
            _pressedKeys.Remove(key);  // If it's not there, nothing happens.
        }

        private bool IsHotKeyCombinationHit()
        {
            bool allPressed = _presetKeyCombination.All(_pressedKeys.Contains);

            if (allPressed)
            {
                _pressedKeys.Clear();
                KeyCombinationHit?.Invoke();
                return true;
            }

            return false;
        }
    }
}
