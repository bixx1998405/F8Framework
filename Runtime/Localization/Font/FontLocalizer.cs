using UnityEngine;
using UnityEngine.UI;
#if LOCALIZER_TMP
using TMPro;
#endif

namespace F8Framework.Core
{
	[AddComponentMenu("F8Framework/Local/FontLocalizer")]
	public class FontLocalizer : LocalizerBase
	{
		public string localizedTextID = "";
		public Font[] fonts;
#if LOCALIZER_TMP
		public TMP_FontAsset[] TMP_fontAsset;
#endif
		private bool hasTMP_Text = false;
		
		protected override void Prepare()
		{
#if LOCALIZER_TMP
			var component = ComponentFinder.Find<TextMesh, Text, TMP_Text>(this);
#else
			var component = ComponentFinder.Find<TextMesh, Text>(this);
#endif
			if (component == null) return;

			if (component is TextMesh textMesh)
			{
				injector = new FontInjector(textMesh);
			}
			else if (component is Text text)
			{
				injector = new FontInjector(text);
			}
#if LOCALIZER_TMP
			else if (component is TMP_Text tmp)
			{
				injector = new FontInjector(tmp);
				hasTMP_Text = true;
			}
#endif
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
			if (!hasTMP_Text)
			{
				injector.Inject(GetFont(index), this);
			}
#if LOCALIZER_TMP
			else
			{
				injector.Inject(GetTMPFont(index), this);
			}
#endif
		}

		private Font GetFont(int index)
		{
			return ArrayHelper.GetSafeElement(fonts, index);
		}

#if LOCALIZER_TMP
		private TMP_FontAsset GetTMPFont(int index)
		{
			return ArrayHelper.GetSafeElement(TMP_fontAsset, index);
		}
#endif
		
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
