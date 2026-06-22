using UnityEditor;
using UnityEngine;

namespace F8Framework.Core.Editor
{
	[CustomEditor(typeof(TextLocalizer))]
	public class TextLocalizerInspector : UnityEditor.Editor
	{
		TextLocalizer localizer;

		void OnEnable()
		{
			localizer = target as TextLocalizer;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			Localization.EditorInstance.LoadInEditor();
			var keys = Localization.EditorInstance.GetAllIds();
			var limit = LocalizationEditorSettings.current.maxSuggestion;

			LocalizerInspectorHelper.DrawLocalizationInfo(localizer.textId, "请输入 Text ID", keys, limit);
		}
	}
}
