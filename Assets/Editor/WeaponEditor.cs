using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using Assets.Modules;

namespace Assets
{

    [CustomEditor(typeof(Weapon))]
    public class WeaponEditor : DynamicChoiceEditor
    {
        public override string[] getChoices()
        {
            ModuleManager.LoadWeaponsJson();
            return ModuleManager.weaponDefs.Values.Select(w => w.fullName).ToArray();
        }
        public override void setSelected(string selected)
        {
            var someClass = (Weapon)target;
            // Update the selected choice in the underlying object
            someClass.selectedWeapon = selected;
        }
    }

}
