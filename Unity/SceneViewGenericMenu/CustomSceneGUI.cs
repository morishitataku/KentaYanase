using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[InitializeOnLoad]
public class CustomSceneGUI{

	static CustomSceneGUI () {
		SceneView.onSceneGUIDelegate += OnSceneGUI;
		EditorApplication.update += Update;
	}

	static void OnSceneGUI (SceneView sceneView) {
		Event env = Event.current;
		if (env.type == EventType.MouseDown && env.button == 1) {
			Rect contextRect = sceneView.position;

			if (env.type == EventType.mouseDown && env.button == 1) {
				Vector2 mousePos = env.mousePosition;
				if (contextRect.Contains(mousePos))
				{
					GenericMenu menu = new GenericMenu();

					menu.AddItem(new GUIContent("MenuItem1"), false, Callback, "item 1");
					menu.AddItem(new GUIContent("MenuItem2"), false, Callback, "item 2");
					menu.AddSeparator("");
					menu.AddItem(new GUIContent("SubMenu/MenuItem3"), false, Callback, "item 3");

					menu.ShowAsContext();

					env.Use();
				}
			}
		}
	}

	static void Callback (object obj) {
		Debug.Log(obj);
	}

	static void Update () {
	}
}
