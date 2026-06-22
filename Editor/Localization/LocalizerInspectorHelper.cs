using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace F8Framework.Core.Editor
{
	internal static class LocalizerInspectorHelper
	{
		internal static void DrawLocalizationInfo(string textId, string emptyHint, IReadOnlyCollection<string> keys, int maxSuggestion)
		{
			GUI.skin.GetStyle("HelpBox").richText = true;

			if (keys.Count == 0)
			{
				EditorGUILayout.HelpBox("没有可用的数据。\n请将本地化表放在Excel存放文件夹中。", MessageType.Info);
				return;
			}

			if (string.IsNullOrEmpty(textId))
			{
				EditorGUILayout.HelpBox(emptyHint, MessageType.Info);
				var postfix = keys.Count > 5 ? $"\n\n<i>还有更多（共 {keys.Count.ToString()} 个ID）</i>" : "";
				ShowSuggestion(keys.ToList(), textId, maxSuggestion, postfix);
				return;
			}

			var dict = Localization.EditorInstance.GetDictionaryFromId(textId);
			if (dict != null)
			{
				var helpText = dict.Aggregate("", (current, item) => current + $"{item.Key}: {item.Value}\n");
				helpText = helpText.TrimEnd('\n');
				EditorGUILayout.HelpBox($"{helpText}", MessageType.Info);
			}
			else
			{
				EditorGUILayout.HelpBox($"Text ID：{textId} 不可用。", MessageType.Error);
			}

			var suggestions = keys.Where(key => key.StartsWith(textId)).ToList();
			ShowSuggestion(suggestions, textId, maxSuggestion);
		}

		internal static void ShowSuggestion(IReadOnlyCollection<string> suggestions, string textId, int maxSuggestion, string postfix = "")
		{
			var noSuggestion = suggestions.Count == 0;
			var exactMatch = suggestions.Count == 1 && suggestions.First() == textId;
			if (noSuggestion || exactMatch) return;

			var text = suggestions.Take(maxSuggestion)
				.Aggregate("\n<b>ID 索引</b>\n", (current, item) => $"{current}\n- {GetMarkedIdRepresentation(item, textId)}");
			text += string.IsNullOrEmpty(postfix) ? "" : postfix;
			EditorGUILayout.HelpBox($"{text}\n", MessageType.Info);
		}

		private static string GetMarkedIdRepresentation(string id, string textId)
		{
			if (string.IsNullOrEmpty(textId))
			{
				return id;
			}
			else
			{
				return $"<color=green>{id.Insert(textId.Length, "</color>")}";
			}
		}
	}
}
