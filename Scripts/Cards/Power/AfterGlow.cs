using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using RTE.Scripts.Powers;

namespace RTE.Scripts.Cards;

public class AfterGlow : CardModel
{
    public override int CanonicalStarCost => 2;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => IsUpgraded ? [HoverTipFactory.FromKeyword(CardKeyword.Retain), HoverTipFactory.FromPower<VigorPower>()] : [HoverTipFactory.FromPower<VigorPower>()];
    public AfterGlow() : base(0, CardType.Power, CardRarity.Common, TargetType.Self, true) { }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (IsUpgraded)
        {
            await PowerCmd.Apply<RetainHandPower>(base.Owner.Creature, 1, base.Owner.Creature, this);
        }
        await PowerCmd.Apply<AfterGlowPower>(base.Owner.Creature, 1, base.Owner.Creature, this);
    }
}