using UnityEditor;
using UnityEngine;

namespace F8Framework.Core.Editor
{
	[CustomEditor(typeof(AudioLocalizer))]
	public class AudioLocalizerInspector : UnityEditor.Editor
	{
		AudioLocalizer localizer;
		SerializedProperty localizedTextID;
		SerializedProperty playFromSamePositionWhenInject;

		void OnEnable()
		{
			localizer = (AudioLocalizer)target;
			playFromSamePositionWhenInject = serializedObject.FindProperty("playFromSamePositionWhenInject");
			localizedTextID = serializedObject.FindProperty("localizedTextID");
		}

		public override void OnInspectorGUI()
		{
			// base.OnInspectorGUI();

			Localization.EditorInstance.LoadInEditor();
			serializedObject.Update();

			var langCount = Localization.EditorInstance.LanguageList.Count;

			UpdateAudioClipInspector(langCount);

			serializedObject.ApplyModifiedProperties();

			var keys = Localization.EditorInstance.GetAllIds();
			var limit = LocalizationEditorSettings.current.maxSuggestion;

			LocalizerInspectorHelper.DrawLocalizationInfo(localizer.localizedTextID, "输入 Text ID 或 拖拽音频 到上方", keys, limit);
		}

		void UpdateAudioClipInspector(int langCount)
		{
			EditorGUILayout.PropertyField(localizedTextID);
			EditorGUILayout.PropertyField(playFromSamePositionWhenInject, new GUIContent("从同一位置播放"));
			
			if (!localizedTextID.stringValue.IsNullOrEmpty())
			{
				return;
			}

			if (localizer.clips == null)
			{
				localizer.clips = new AudioClip[langCount];
			}
			else if (localizer.clips.Length != langCount)
			{
				var oldClips = localizer.clips;
				localizer.clips = new AudioClip[langCount];
				for (var i = 0; i < langCount; i++)
				{
					localizer.clips[i] = oldClips[i];
				}
			}

			for (var i = 0; i < langCount; i++)
			{
				var clip = EditorGUILayout.ObjectField(Localization.EditorInstance.LanguageList[i], localizer.clips[i], typeof(AudioClip), false) as AudioClip;
				if (localizer.clips[i] != clip)
				{
					localizer.clips[i] = clip;
					EditorUtility.SetDirty(localizer);
				}
			}
		}
	}
}
