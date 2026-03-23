using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace RTE.Scripts.Cards;

public class KinglyAura : CardModel
{
    private const string _HitsKey = "TotalHits";
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(12, ValueProp.Move | ValueProp.SkipHurtAnim),
        new DynamicVar(_HitsKey, 0)    
    ];
    private decimal _hitsCount;
    private decimal HitsCount
    {
        get
        {
            return _hitsCount;
        }
        set
        {
            AssertMutable();
            _hitsCount = value;
        }
    }
    public KinglyAura() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.RandomEnemy) {}

    public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card != this)
        {
            return Task.CompletedTask;
        }
        base.DynamicVars[_HitsKey].BaseValue += 1;
        HitsCount += 1;
        return Task.CompletedTask;
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int hitsCount = base.DynamicVars[_HitsKey].IntValue;
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingRandomOpponents(base.CombatState)
            .WithHitCount(hitsCount)
            .WithAttackerFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(4);
    }

    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        base.DynamicVars[_HitsKey].BaseValue += HitsCount;
    }
}