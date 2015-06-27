using Assets.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Assets
{
    public abstract class DynamicChoiceEditor : Editor
    {
        string[] _choices;
        int _choiceIndex = 0;

        public override void OnInspectorGUI()
        {
            // Draw the default inspector
            DrawDefaultInspector();
            //_choices = ModuleManager.weaponDefs.Select(w => w.ToString()).ToArray();
            _choiceIndex = EditorGUILayout.Popup(_choiceIndex, getChoices());
            //var someClass = (T) target ;
            // Update the selected choice in the underlying object
            //someClass.selection = getChoices()[_choiceIndex];
            if (getChoices().Length > _choiceIndex)
            {
                setSelected(getChoices()[_choiceIndex]);
                // Save the changes back to the object
                EditorUtility.SetDirty(target);
            }
        }

        public abstract string[] getChoices();

        public abstract void setSelected(string selected);
    }
}
