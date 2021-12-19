using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using CraftMagicItems.NewComponents;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.Items;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftMagicItems.Fixes
{
    static class FixNecroticWeapons
    {
        //BloodthirstNegative3d6.f79f6fc9e5ab7da4daf93d665e4935bf.json - actually 2d6
        //DarkBiddingSlingstaffNecrotic2d8.c2229230ff9292048b07a8429d6536c6.json - 2d8
        //GlaiveOfUnfavorableEnchantment.a2eddcb2e50d41d42a657b19fe25e524.json 1d3
        public static string[] negativeDamageWeaponEnergyDamageDice = new string[] { };

        //Deteriorative.bbe55d6e76b973d41bf5abeed643861d.json 3d6
        //LongswordOfDetrimentEnchantment.1d6b29e16be14214b8ccfacfcf4abf58.json 1
        //Necrotic.bad4134798e182c4487819dce9b43003.json 1d6, redudnant
        //WoundBearerNegative1d6.7f727c7023be4854babc44d3ee756d31.json 1d6, redudnanc
        public static string[] negativeDamageConditional = new string[] { };





        //ElderToothEnchantment.83612e733df422742818a69bf94f57fc.json
        //Looks like a super shitty burst implementation plus regular 1d6


        //Sea of WTFER
        //GlowingScytheEnchantment.385847ab4a270a64fa00479711995e57.json

        //MarchingTerrorEnchantment.c1af79dcadc9c4648b7cdac512186df5.json

        //NightmareEnchantment.0b97533c96610c4469438befcc425819.json

        public static void FixNecroticItems()
        {
            //Main.Log("Fixing Necrotic Weapons");
            ItemWeaponConfigurator.For("bee1ef955c45961438c9972646ed2431").RemoveFromEnchantments("7f727c7023be4854babc44d3ee756d31").AddToEnchantments("bad4134798e182c4487819dce9b43003").Configure();


            BlueprintItemEnchantment necrotic1d6 = ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>("bad4134798e182c4487819dce9b43003");

            ConditionsChecker c = necrotic1d6.Components.OfType<WeaponConditionalDamageDice>().FirstOrDefault().Conditions;
            ConditionsBuilder negConditions = ConditionsBuilder.New().HasFact("734a29b693e9ec346ba2951b27987e33", true).HasFact("fd389783027d63343b4a5634bd81645f", true);

            BlueprintItemEnchantment glaiveNegative = ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>("a2eddcb2e50d41d42a657b19fe25e524");
            WeaponEnchantmentConfigurator.For("a2eddcb2e50d41d42a657b19fe25e524").RemoveComponents(x => x is WeaponEnergyDamageDice).AddWeaponConditionalDamageDice(new DamageDescription()
            {
                Dice = new DiceFormula()
                {
                    m_Dice = DiceType.D3,
                    m_Rolls = 1
                },
                TypeDescription = new DamageTypeDescription()
                {
                    Energy = DamageEnergyType.NegativeEnergy,
                }
            }, false, false, negConditions).Configure();
            WeaponEnchantmentConfigurator.For("f79f6fc9e5ab7da4daf93d665e4935bf").RemoveComponents(x => x is WeaponEnergyDamageDice).AddWeaponConditionalDamageDice(new DamageDescription()
            {
                Dice = new DiceFormula()
                {
                    m_Dice = DiceType.D6,
                    m_Rolls = 2
                }
            }, false, false, negConditions).Configure();
            WeaponEnchantmentConfigurator.For("c2229230ff9292048b07a8429d6536c6").RemoveComponents(x => x is WeaponEnergyDamageDice).AddWeaponConditionalDamageDice(new DamageDescription()
            {
                Dice = new DiceFormula()
                {
                    m_Dice = DiceType.D8,
                    m_Rolls = 2
                }
            }, false, false, negConditions).Configure();



            WeaponEnchantmentConfigurator.For("83612e733df422742818a69bf94f57fc").RemoveComponents(x => x is WeaponEnergyDamageDice).RemoveComponents(x => x is AddInitiatorAttackWithWeaponTrigger).AddComponent(new ConditionalBurstDamage()
            {
                Conditions = c,
                Dice = Kingmaker.RuleSystem.DiceType.D10,
                Element = Kingmaker.Enums.Damage.DamageEnergyType.NegativeEnergy


            }).Configure();
            ItemWeaponConfigurator.For("3e55ebd643d5df949a19a0e17a0d8380").AddToEnchantments("bad4134798e182c4487819dce9b43003").Configure();





        }
        [HarmonyPatch(typeof(ItemEntity), "OnPostLoad")]
        class ItemEntity__PostLoad__Patch
        {
            static void Postfix(ItemEntity __instance)
            {

                BlueprintItemEnchantment woundbearerEnchant = ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>("7f727c7023be4854babc44d3ee756d31");

                ItemEnchantment toKIll = __instance.Enchantments.FirstOrDefault(x => x.Blueprint.ToReference<BlueprintItemEnchantmentReference>() == woundbearerEnchant.ToReference<BlueprintItemEnchantmentReference>());
                if (toKIll != null)//Should patch existing woundbearers
                {
                    __instance.Enchantments.Remove(toKIll);

                }


            }
        }
    }
}
