using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UIElements;

public class PluginWindow : EditorWindow
{
    [MenuItem("Window/Script Finder")]

    static void ShowWindow()
    {
        GetWindow<PluginWindow>("Script Finder");
    }
// 
    public MonoScript scriptToSearch;
    public MonoScript lastScriptSearched;
    public List<GameObject> objectsWithScript = new List<GameObject>();
    public void OnGUI()
    {
        GUIStyle titleStyle = GUIStyle.none;
        titleStyle.normal.textColor = Color.white;
        titleStyle.fontSize = 18;
        titleStyle.fontStyle = FontStyle.Bold;
        
        GUILayout.Space(10);
        GUILayout.Label("Script Search", titleStyle);  
        GUILayout.Space(10);
        //This displays a slot where you can select a script (FINALLY, the issue was with it being MonoBehaviour and not MonoScript)
        scriptToSearch = (MonoScript)EditorGUILayout.ObjectField("Script", scriptToSearch, typeof(MonoScript), false);
        GUILayout.Space(15);
        
        //I had to do this to stop the foreach loop from running constantly, the lastScriptSearched is a little annoying to use but it works.
        if (scriptToSearch != lastScriptSearched)
        {
            lastScriptSearched = scriptToSearch;
            ScriptSearch();
        }
        
        //This displays the list of objects (but only their names), either way it works and my idea comes across
        if (scriptToSearch != null)
        {
            
            EditorGUILayout.LabelField("Objects Containing Script", titleStyle);

            foreach (var obj in objectsWithScript)
            {
                EditorGUILayout.LabelField(obj.name);
            }
            
        }
        else
        {
            //Added this purely for aesthetics 
            EditorGUILayout.HelpBox("Select Script to Begin Search", MessageType.Info);
        }
        
        EditorGUILayout.Space(15);
        // This button is a substitute for the list not updating in real time so it just calls the function again
        if (GUILayout.Button("Refresh"))
        {
            ScriptSearch();
            Debug.Log("List Refreshed");
        }

    }

    public void ScriptSearch()
    {
        objectsWithScript.Clear();
        if (scriptToSearch != null)
        {
            //it won't accept MonoScript as a type but because all scripts inherit from Monobehaviour this seems to work, it just checks the objects for the script as long as it inherits from MonoBehaviour. Not sure what'll happen if a script inherits from another class that inherits from Monobehaviour though
            MonoBehaviour[] objectsContainingScript = FindObjectsOfType<MonoBehaviour>();
            
                foreach (MonoBehaviour scriptSelect in objectsContainingScript)
                {
                    if (scriptSelect != null && scriptSelect.GetType() == scriptToSearch.GetClass())
                    {
                        objectsWithScript.Add(scriptSelect.gameObject);  
                        Debug.Log("Added Object: " + scriptSelect.gameObject.name);
                    }
                }
        }
        
    }
}
