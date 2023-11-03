using System.Diagnostics;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace Utilities.Mouse_and_Keyboard
{
	public interface IKeyInputHandler
	{
		/// <summary>
		/// Determines if the pressed key results in the preset keycombination.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		bool IsPresetKeyCombinationHit(Keys key);
		void KeyReleased(Keys keys);
		delegate void KeyCombinationHitEventHandler();
		event KeyCombinationHitEventHandler KeyCombinationHit;
	}

	public class KeyInputHandler : IKeyInputHandler
	{
		private HashSet<Keys> _pressedKeys = new HashSet<Keys>();

		public static bool _isRecordingNewCombo = false;
		HashSet<Keys> _oldCombo;
		public HashSet<Keys> PresetKeyCombination { get; set; }

		public static KeyInputHandler Instance { get; private set; } // todo: refactor. KeyINputHandler should not be concerned with KeyCombo preferences.
		public KeyInputHandler(IEnumerable<Keys> presetKeyCombination)
		{
			Instance = this;
			if (presetKeyCombination == null || !presetKeyCombination.Any())
			{
				throw new ArgumentNullException(nameof(presetKeyCombination), "Preset key combination must not be null or empty.");
			}

			PresetKeyCombination = new HashSet<Keys>(presetKeyCombination);
		}

		public event IKeyInputHandler.KeyCombinationHitEventHandler KeyCombinationHit;

		public void StartRecordingNewCombo()
		{
			_oldCombo = PresetKeyCombination;
			_isRecordingNewCombo = true;
		}

		public void CancelRecording()
		{
			_isRecordingNewCombo = false;
			_oldCombo.Clear();
			_oldCombo = null;
		}

		public void ApplyNewRecording()
		{
			_isRecordingNewCombo = false;
			PresetKeyCombination = _pressedKeys;
		}
		public bool IsPresetKeyCombinationHit(Keys key)
		{
			if(_isRecordingNewCombo)
			{
				if (key == Keys.LButton)
					return false; // left mouse button is prohibited.

				_pressedKeys.Add(key);
				return false;
			}
			if (PresetKeyCombination.Contains(key))
			{
				_pressedKeys.Add(key);  // If it's already there, it won't be added again.

				if (_pressedKeys.Count == PresetKeyCombination.Count)
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
			bool allPressed = PresetKeyCombination.All(_pressedKeys.Contains);

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
