using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace F8Framework.Core.Editor
{
	[CustomEditor(typeof(ImageLocalizer))]
	public class ImageLocalizerInspector : UnityEditor.Editor
	{
		ImageLocalizer localizer;
		SerializedProperty localizedTextID;
		SerializedProperty propertyName;

		void OnEnable()
		{
			localizer = (ImageLocalizer)target;
			propertyName = serializedObject.FindProperty("propertyName");
			localizedTextID = serializedObject.FindProperty("localizedTextID");
		}

		public override void OnInspectorGUI()
		{
			// base.OnInspectorGUI();

			Localization.EditorInstance.LoadInEditor();
			serializedObject.Update();

			var langCount = Localization.EditorInstance.LanguageList.Count;

			var component = ComponentFinder.Find<Image, RawImage, SpriteRenderer, Renderer>(localizer);
			
			if (component is Image)
			{
				UpdateSpriteInspector(langCount);
			}
			else if (component is RawImage)
			{
				UpdateTextureInspector(langCount);
			}
			else if (component is SpriteRenderer)
			{
				UpdateSpriteInspector(langCount);
			}
			else if (component is Renderer)
			{
				UpdateTexture2DInspector(langCount);
			}

			serializedObject.ApplyModifiedProperties();

			var keys = Localization.EditorInstance.GetAllIds();
			var limit = LocalizationEditorSettings.current.maxSuggestion;

			LocalizerInspectorHelper.DrawLocalizationInfo(localizer.localizedTextID, "输入 Text ID 或 拖拽图片 到上方", keys, limit);
		}

		void UpdateTexture2DInspector(int langCount)
		{
			EditorGUILayout.PropertyField(localizedTextID);
			EditorGUILayout.PropertyField(propertyName);
			
			if (!localizedTextID.stringValue.IsNullOrEmpty())
			{
				return;
			}
			
			if (localizer.texture2Ds == null)
			{
				localizer.texture2Ds = new Texture2D[langCount];
			}
			else if (localizer.texture2Ds.Length != langCount)
			{
				var oldTexture2Ds = localizer.texture2Ds;
				localizer.texture2Ds = new Texture2D[langCount];
				for (var i = 0; i < langCount; i++)
				{
					localizer.texture2Ds[i] = oldTexture2Ds[i];
				}
			}

			for (var i = 0; i < langCount; i++)
			{
				var tex = EditorGUILayout.ObjectField(Localization.EditorInstance.LanguageList[i], localizer.texture2Ds[i], typeof(Texture2D), false) as Texture2D;
				if (localizer.texture2Ds[i] != tex)
				{
					localizer.texture2Ds[i] = tex;
					EditorUtility.SetDirty(localizer);
				}
			}
		}

		void UpdateTextureInspector(int langCount)
		{
			EditorGUILayout.PropertyField(localizedTextID);
			
			if (!localizedTextID.stringValue.IsNullOrEmpty())
			{
				return;
			}
			
			if (localizer.textures == null)
			{
				localizer.textures = new Texture[langCount];
			}
			else if (localizer.textures.Length != langCount)
			{
				var oldTextures = localizer.textures;
				localizer.textures = new Texture[langCount];
				for (var i = 0; i < langCount; i++)
				{
					localizer.textures[i] = oldTextures[i];
				}
			}

			for (var i = 0; i < langCount; i++)
			{
				var tex = EditorGUILayout.ObjectField(Localization.EditorInstance.LanguageList[i], localizer.textures[i], typeof(Texture), false) as Texture;
				if (localizer.textures[i] != tex)
				{
					localizer.textures[i] = tex;
					EditorUtility.SetDirty(localizer);
				}
			}
		}

		void UpdateSpriteInspector(int langCount)
		{
			EditorGUILayout.PropertyField(localizedTextID);
			
			if (!localizedTextID.stringValue.IsNullOrEmpty())
			{
				return;
			}
			
			if (localizer.sprites == null)
			{
				localizer.sprites = new Sprite[langCount];
			}
			else if (localizer.sprites.Length != langCount)
			{
				var oldSprites = localizer.sprites;
				localizer.sprites = new Sprite[langCount];
				for (var i = 0; i < langCount; i++)
				{
					localizer.sprites[i] = oldSprites[i];
				}
			}

			for (var i = 0; i < langCount; i++)
			{
				var sprite = EditorGUILayout.ObjectField(Localization.EditorInstance.LanguageList[i], localizer.sprites[i], typeof(Sprite), false) as Sprite;
				if (localizer.sprites[i] != sprite)
				{
					localizer.sprites[i] = sprite;
					EditorUtility.SetDirty(localizer);
				}
			}
		}
	}
}
