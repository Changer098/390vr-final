using UnityEngine;
using UnityEditor;
using System.Collections;

public class detailScript : MonoBehaviour {
    //Just an editor script that explains what a game object does/why it's there
    public string comments;
}

[CustomEditor(typeof(detailScript))]
[CanEditMultipleObjects]

public class detailScriptEditor : Editor {
    string testString = "Hello, this is a test comment";
    //use this for initialization
    void onEnable() {

    }
    public override void OnInspectorGUI() {
        detailScript ds = (detailScript)target;
        //base.OnInspectorGUI();
        EditorGUILayout.LabelField("Comments");
        ds.comments = EditorGUILayout.TextArea(ds.comments, GUILayout.Height(100));
    }
}