using Kingmaker;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftMagicItems.NewComponents
{

    class ConditionalBurstDamage : WeaponEnchantmentLogic, IInitiatorRulebookHandler<RuleDealDamage>, IRulebookHandler<RuleDealDamage>, ISubscriber, IInitiatorRulebookSubscriber
    {
        // Token: 0x0600C2ED RID: 49901 RVA: 0x0030F560 File Offset: 0x0030D760
        public void OnEventAboutToTrigger(RuleDealDamage evt)
        {
            if (this.CheckWielder)
            {
                using (base.Enchantment.Context.GetDataScope(base.Owner.Wielder.Unit))
                {
                    if (evt.DamageBundle.Weapon == base.Owner && this.Conditions.Check())
                    {
                        RuleAttackRoll attackRoll = evt.AttackRoll;
                        if (base.Owner == null || attackRoll == null || !attackRoll.IsCriticalConfirmed || attackRoll.FortificationNegatesCriticalHit)
                        {
                            return;
                        }
                        RuleCalculateWeaponStats ruleCalculateWeaponStats = Rulebook.Trigger<RuleCalculateWeaponStats>(new RuleCalculateWeaponStats(Game.Instance.DefaultUnit, base.Owner, null, null));
                        DiceFormula dice = new DiceFormula(Math.Max(ruleCalculateWeaponStats.CriticalMultiplier - 1, 1), this.Dice);
                        evt.Add(new EnergyDamage(dice, this.Element));
                    }
                    return;
                }
            }
            using (base.Enchantment.Context.GetDataScope(evt.Target))
            {
                if (evt.DamageBundle.Weapon == base.Owner && this.Conditions.Check())
                {
                    RuleAttackRoll attackRoll = evt.AttackRoll;
                    if (base.Owner == null || attackRoll == null || !attackRoll.IsCriticalConfirmed || attackRoll.FortificationNegatesCriticalHit)
                    {
                        return;
                    }
                    RuleCalculateWeaponStats ruleCalculateWeaponStats = Rulebook.Trigger<RuleCalculateWeaponStats>(new RuleCalculateWeaponStats(Game.Instance.DefaultUnit, base.Owner, null, null));
                    DiceFormula dice = new DiceFormula(Math.Max(ruleCalculateWeaponStats.CriticalMultiplier - 1, 1), this.Dice);
                    evt.Add(new EnergyDamage(dice, this.Element));
                }
            }

        }

        // Token: 0x0600C2EE RID: 49902 RVA: 0x00003AE3 File Offset: 0x00001CE3
        public void OnEventDidTrigger(RuleDealDamage evt)
        {
        }

        public bool CheckWielder;

        // Token: 0x04007F6B RID: 32619
        public DamageEnergyType Element;

        // Token: 0x04007F6C RID: 32620
        public DiceType Dice = DiceType.D8;

        public ConditionsChecker Conditions;
    }

}
