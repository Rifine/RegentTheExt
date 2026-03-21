using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Random;

namespace RTE.Scripts.Powers;

class CoalescePower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Exhaust), HoverTipFactory.Static(StaticHoverTip.Energy)];

    public override Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card.Owner != base.Owner.Player)
        {
            return Task.CompletedTask;
        }
        if (!CombatManager.Instance.IsInProgress)
        {
            return Task.CompletedTask;
        }
        if (card.Type == CardType.Status)
        {
            IEnumerable<CardModel> handCards = PileType.Hand.GetPile(base.Owner.Player).Cards;
            Rng rng = base.Owner.Player.RunState.Rng.CombatCardSelection;
            CardModel targetCard = rng.NextItem(handCards.Where((CardModel c) => !c.EnergyCost.CostsX && c.EnergyCost.GetWithModifiers(CostModifiers.All) > 0));
            if (targetCard != null)
            {
                targetCard.EnergyCost.AddThisTurn(-1);
            }
        }
        return Task.CompletedTask;
    }
}