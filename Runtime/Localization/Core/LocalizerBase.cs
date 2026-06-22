using UnityEngine;

namespace F8Framework.Core
{
	public abstract class LocalizerBase : MonoBehaviour
	{
		protected IInjector injector;

		protected virtual void Awake()
		{
			Localization.Instance?.AddLocalizer(this);
			Prepare();
		}

		/// <summary>
		/// 准备对目标组件的引用。
		/// </summary>
		protected abstract void Prepare();

		protected virtual void Start()
		{
			Localize();
		}

		/// <summary>
		/// 本地化目标组件。
		/// </summary>
		internal abstract void Localize();

		protected virtual void OnDestroy()
		{
			if (injector is IUnloadableInjector unloadableInjector)
			{
				unloadableInjector.Unload();
			}

			Localization.Instance?.RemoveLocalizer(this);
		}

		protected bool ValidateAndInject(string textId)
		{
			if (string.IsNullOrEmpty(textId)) return false;

#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				Localization.Instance?.LoadInEditor();
				Prepare();
			}
#endif

			if (Localization.Instance?.Has(textId) == false)
			{
				if (Application.isPlaying) LogF8.LogError($"Text ID: {textId} 不可用。");
				return false;
			}

			var text = Localization.Instance?.GetTextFromId(textId);
			injector.Inject(text, this);
			return true;
		}

		protected void ClearInjector()
		{
			injector?.Inject("", this);
		}
	}
}
