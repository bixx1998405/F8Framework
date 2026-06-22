using UnityEngine;

namespace F8Framework.Core
{
	[AddComponentMenu("F8Framework/Local/AudioLocalizer")]
	public class AudioLocalizer : LocalizerBase
	{
		public string localizedTextID = "";
		public AudioClip[] clips;
		public bool playFromSamePositionWhenInject;

		protected override void Prepare()
		{
			var component = ComponentFinder.Find<AudioSource>(this);
			if (component == null) return;

			if (component is AudioSource audio)
			{
				injector = new AudioSourceInjector(audio);
			}
		}

		internal override void Localize()
		{
			if (injector == null)
			{
				return;
			}
			if (!localizedTextID.IsNullOrEmpty())
			{
				ChangeID(localizedTextID);
				return;
			}
			var index = Localization.Instance?.CurrentLanguageIndex ?? 0;
			injector.Inject(GetClip(index), this);
		}

		private AudioClip GetClip(int index)
		{
			return ArrayHelper.GetSafeElement(clips, index);
		}
		
		public bool ChangeID(string textId)
		{
			if (!ValidateAndInject(textId)) return false;
			this.localizedTextID = textId;
			return true;
		}

		public void Clear()
		{
			localizedTextID = null;
			ClearInjector();
		}
	}
}
