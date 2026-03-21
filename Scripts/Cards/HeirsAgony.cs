using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using RTE.Scripts.Powers;

namespace RTE.Scripts.Cards;

public class HeirsAgony : CardModel
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromForge();

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new DamageVar(2, ValueProp.Unpowered)];
    public HeirsAgony() : base(0, CardType.Power, CardRarity.Common, TargetType.Self, true)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.IntValue, base.Owner);
        await PowerCmd.Apply<DisintegrationPower>(base.Owner.Creature, base.DynamicVars.Damage.IntValue, base.Owner.Creature, this);
        await PowerCmd.Apply<HeirsAgonyPower>(base.Owner.Creature, 1, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}