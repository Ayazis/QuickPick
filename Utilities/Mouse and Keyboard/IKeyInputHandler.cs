using System.Windows.Forms;

namespace Utilities.Mouse_and_Keyboard
{
	public interface IKeyInputHandler
	{
		bool IsPresetKeyCombinationHit(Keys key);
		void KeyReleased(Keys keys);
	}

	public class KeyInputHandler : IKeyInputHandler
	{
		private HashSet<Keys> _pressedKeys = new HashSet<Keys>();
		private HashSet<Keys> _presetKeyCombination;

		public KeyInputHandler(IEnumerable<Keys> presetKeyCombination)
		{
			if (presetKeyCombination == null || !presetKeyCombination.Any())
			{
				throw new ArgumentNullException(nameof(presetKeyCombination), "Preset key combination must not be null or empty.");
			}

			_presetKeyCombination = new HashSet<Keys>(presetKeyCombination);
		}

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
				return true;
			}

			return false;
		}
	}
}
