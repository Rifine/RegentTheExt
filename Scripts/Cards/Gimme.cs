using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace RTE.Scripts.Cards;

public class Gimme : CardModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    public Gimme() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardPile drawPile = PileType.Draw.GetPile(base.Owner);
        IEnumerable<CardModel> cards = drawPile.Cards
            .Where((CardModel c) => c.VisualCardPool.IsColorless && c.Type == CardType.Attack)
            .ToList()
            .StableShuffle(base.Owner.RunState.Rng.Shuffle)
            .Take(base.DynamicVars.Cards.IntValue);  
        foreach (CardModel card in cards)
        {
            if (card != null)
            {
                card.EnergyCost.AddThisTurn(-1);
                await CardPileCmd.Add(card, PileType.Hand);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}