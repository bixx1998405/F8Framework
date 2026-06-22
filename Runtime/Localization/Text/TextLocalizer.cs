#if LOCALIZER_TMP
using TMPro;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace F8Framework.Core
{
	[AddComponentMenu("F8Framework/Local/TextLocalizer")]
	public class TextLocalizer : LocalizerBase
	{
		public string textId;

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
				injector = new TextMeshInjector(textMesh);
			}
			else if (component is Text text)
			{
				injector = new UITextInjector(text);
			}
#if LOCALIZER_TMP
			else if (component is TMP_Text tmp)
			{
				injector = new TMPInjector(tmp);
			}
#endif
		}

		internal override void Localize()
		{
			if (injector == null)
			{
				return;
			}
			ChangeID(textId);
		}

		public bool ChangeID(string textId)
		{
			if (!ValidateAndInject(textId)) return false;
			this.textId = textId;
			return true;
		}

		public void Clear()
		{
			textId = null;
			ClearInjector();
		}
	}
}
