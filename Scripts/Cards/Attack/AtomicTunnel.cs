using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace RTE.Scripts.Cards;

public class AtomicTunnel : CardModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(5, ValueProp.Move | ValueProp.SkipHurtAnim),
        new DynamicVar("EnemyScale", 2)
    ];

    public AtomicTunnel() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AllAllies) {}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(base.Owner.Creature)
            .WithNoAttackerAnim()
            .Execute(choiceContext);
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue * base.DynamicVars["EnemyScale"].BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(base.CombatState)
            .Execute(choiceContext);
        await Cmd.Wait(0.25f);
    }

    protected override PileType GetResultPileType()
    {
        PileType origin = base.GetResultPileType();
        if (origin != PileType.Discard)
        {
            return origin;
        }
        return PileType.Hand;
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["EnemyScale"].UpgradeValueBy(1);
    }
}