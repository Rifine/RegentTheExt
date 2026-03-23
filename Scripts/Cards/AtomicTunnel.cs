using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace RTE.Scripts.Cards;

public class AtomicTunnel : CardModel
{
    public override int CanonicalStarCost => 2;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10, ValueProp.Move), new PowerVar<DisintegrationPower>(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DisintegrationPower>()];   
    public AtomicTunnel() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) {}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).Execute(choiceContext);
        await PowerCmd.Apply<DisintegrationPower>(cardPlay.Target, base.DynamicVars["DisintegrationPower"].IntValue, base.Owner.Creature, this);
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
        base.DynamicVars.Damage.UpgradeValueBy(4);
        base.DynamicVars["DisintegrationPower"].UpgradeValueBy(2);
    }
}