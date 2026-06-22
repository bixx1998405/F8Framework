using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
#if LOCALIZER_TMP
using TMPro;
#endif

namespace F8Framework.Core.Editor
{
	[CustomEditor(typeof(FontLocalizer))]
	public class FontLocalizerInspector : UnityEditor.Editor
	{
		FontLocalizer localizer;
		SerializedProperty localizedTextID;
		bool hasText;
#if LOCALIZER_TMP
		bool hasTMP_Text;
#endif
		
		void OnEnable()
		{
			localizer = (FontLocalizer)target;
			localizedTextID = serializedObject.FindProperty("localizedTextID");
			hasText = ComponentFinder.Find<TextMesh, Text>(localizer);
#if LOCALIZER_TMP
			hasTMP_Text = ComponentFinder.Find<TMP_Text>(localizer);
#endif
		}

		public override void OnInspectorGUI()
		{
			// base.OnInspectorGUI();

			Localization.EditorInstance.LoadInEditor();
			serializedObject.Update();

			var langCount = Localization.EditorInstance.LanguageList.Count;

			UpdateFontInspector(langCount);

			serializedObject.ApplyModifiedProperties();

			var keys = Localization.EditorInstance.GetAllIds();
			var limit = LocalizationEditorSettings.current.maxSuggestion;

			LocalizerInspectorHelper.DrawLocalizationInfo(localizer.localizedTextID, "输入 Text ID 或 拖拽字体 到上方", keys, limit);
		}

		void UpdateFontInspector(int langCount)
		{
			EditorGUILayout.PropertyField(localizedTextID);
			
			if (!localizedTextID.stringValue.IsNullOrEmpty())
			{
				return;
			}

			if (hasText)
			{
				if (localizer.fonts == null)
				{
					localizer.fonts = new Font[langCount];
				}
				else if (localizer.fonts.Length != langCount)
				{
					var oldFonts = localizer.fonts;
					localizer.fonts = new Font[langCount];
					for (var i = 0; i < langCount; i++)
					{
						localizer.fonts[i] = oldFonts[i];
					}
				}

				for (var i = 0; i < langCount; i++)
				{
					var font = EditorGUILayout.ObjectField(Localization.EditorInstance.LanguageList[i], localizer.fonts[i], typeof(Font), false) as Font;
					if (localizer.fonts[i] != font)
					{
						localizer.fonts[i] = font;
						EditorUtility.SetDirty(localizer);
					}
				}
				return;
			}

#if LOCALIZER_TMP
			if (hasTMP_Text)
			{
				if (localizer.TMP_fontAsset == null)
				{
					localizer.TMP_fontAsset = new TMP_FontAsset[langCount];
				}
				else if (localizer.TMP_fontAsset.Length != langCount)
				{
					var oldFonts = localizer.TMP_fontAsset;
					localizer.TMP_fontAsset = new TMP_FontAsset[langCount];
					for (var i = 0; i < langCount; i++)
					{
						localizer.TMP_fontAsset[i] = oldFonts[i];
					}
				}

				for (var i = 0; i < langCount; i++)
				{
					var font = EditorGUILayout.ObjectField(Localization.EditorInstance.LanguageList[i], localizer.TMP_fontAsset[i], typeof(TMP_FontAsset), false) as TMP_FontAsset;
					if (localizer.TMP_fontAsset[i] != font)
					{
						localizer.TMP_fontAsset[i] = font;
						EditorUtility.SetDirty(localizer);
					}
				}
				return;
			}
#endif
		}
	}
}
