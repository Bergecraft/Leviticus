using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using Assets.Modules;
using Assets.Entities.Modules.Weapons;
using Assets.Entities.Modules.Thrusters;
using Assets.Entities.Modules;

namespace Assets
{
    [CustomEditor(typeof(WeaponBehaviour))]
    [CanEditMultipleObjects]
    public class WeaponEditor : DynamicChoiceEditor<WeaponDef>
    {

    }

    [CustomEditor(typeof(MainThrusterBehaviour))]
    [CanEditMultipleObjects]
    public class ThrusterEditor : DynamicChoiceEditor<ThrusterDef>
    {

    }

    [CustomEditor(typeof(RCSBehaviour))]
    [CanEditMultipleObjects]
    public class RCSEditor : DynamicChoiceEditor<ThrusterDef>
    {

    }
    [CustomEditor(typeof(ModuleBehaviour))]
    [CanEditMultipleObjects]
    public class ModuleEditor : DynamicChoiceEditor<ModuleDef>
    {

    }
    [CustomEditor(typeof(TurretBehaviour))]
    [CanEditMultipleObjects]
    public class TurretEditor : DynamicChoiceEditor<ModuleDef>
    {

    }
    //[CustomEditor(typeof(DefinitionBehaviour<WeaponDef>))]
    //public class WeaponEditor : DynamicChoiceEditor
    //{
    //    public override string[] getChoices()
    //    {
    //        DefinitionManager.LoadWeaponsJson();
    //        return DefinitionManager.GetAllDefinitions<WeaponDef>().Select(w => w.definitionType).ToArray();
    //    }
    //    public override void setSelected(string selected)
    //    {
    //        var someClass = (DefinitionBehaviour)target;
    //        // Update the selected choice in the underlying object
    //        someClass.selectedWeapon = selected;
    //    }
    //}

}
