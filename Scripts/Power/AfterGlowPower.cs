using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace RTE.Scripts.Powers;

public sealed class AfterGlowPower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.None;
    public override bool IsInstanced => true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VigorPower>()];

    public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == base.Owner.Side && base.Owner.IsPlayer)
        {
            Flash();
            CardPile hand = PileType.Hand.GetPile(base.Owner.Player);
            int numberOfCardsInHand = hand.Cards.Count();
            await PowerCmd.Apply<VigorPower>(base.Owner, numberOfCardsInHand, base.Owner, null);
        }
    }
}