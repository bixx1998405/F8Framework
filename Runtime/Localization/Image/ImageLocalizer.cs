using UnityEngine;
using UnityEngine.UI;

namespace F8Framework.Core
{
	[AddComponentMenu("F8Framework/Local/ImageLocalizer")]
	public class ImageLocalizer : LocalizerBase
	{
		public string localizedTextID = "";
		public string propertyName = "_MainTex";
		public Texture2D[] texture2Ds;
		public Sprite[] sprites;
		public Texture[] textures;
		
		protected override void Prepare()
		{
			var component = ComponentFinder.Find<Image, RawImage, SpriteRenderer, Renderer>(this);
			if (component == null) return;
			
			if (component is Image image)
			{
				injector = new UIImageInjector(image, sprites);
			}
			else if (component is RawImage rawImage)
			{
				injector = new RawImageInjector(rawImage, textures);
			}
			else if (component is SpriteRenderer spriteRenderer)
			{
				injector = new SpriteRendererInjector(spriteRenderer, sprites);
			}
			else if (component is Renderer renderer)
			{
				injector = new TextureInjector(renderer, propertyName, texture2Ds);
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
			var index = Localization.Instance?.CurrentLanguageIndex;
			injector.Inject(index, this);
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
