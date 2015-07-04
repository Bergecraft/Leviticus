using Assets.Entities;
using Assets.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Assets
{
    public abstract class DynamicChoiceEditor<T> : Editor where T : EntityDef
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
            if (getChoices().Length > _choiceIndex && _choiceIndex>0)
            {
                setSelected(getChoices()[_choiceIndex]);
                // Save the changes back to the object
                foreach (var target in targets)
                {
                    EditorUtility.SetDirty(target);
                }
            }
        }

        public string[] getChoices()
        {
            DefinitionManager.LoadWeaponsJson();
            var choices = DefinitionManager.GetAllDefinitions<T>().Select(w => w.definitionType).ToList();
            choices.Insert(0, "");
            return choices.ToArray();
        }

        public void setSelected(string selected)
        {
            foreach (var target in targets)
            {
                var someClass = (DefinitionBehaviour<T>)target;
                // Update the selected choice in the underlying object
                someClass.selectedDefinition = selected;
            }
        }
    }
}
