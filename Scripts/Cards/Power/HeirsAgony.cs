using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using RTE.Scripts.Powers;

namespace RTE.Scripts.Cards;

public class HeirsAgony : CardModel
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromForge().Append(HoverTipFactory.FromPower<DisintegrationPower>());

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new PowerVar<DisintegrationPower>(3), new ForgeVar(3)];
    public HeirsAgony() : base(0, CardType.Power, CardRarity.Uncommon, TargetType.Self, true) { }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.IntValue, base.Owner);
        await PowerCmd.Apply<DisintegrationPower>(base.Owner.Creature, base.DynamicVars["DisintegrationPower"].IntValue, base.Owner.Creature, this);
        await PowerCmd.Apply<HeirsAgonyPower>(base.Owner.Creature, base.DynamicVars.Forge.IntValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}