using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace RTE.Scripts.Cards;

public class Hearken : CardModel
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    public Hearken()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
    { }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        IEnumerable<CardModel> forCombat = CardFactory.GetForCombat(base.Owner, AllMinionCards().ToList(), DynamicVars.Cards.IntValue, RunState.Rng.CombatCardGeneration);
        await CardPileCmd.AddGeneratedCardsToCombat(forCombat, PileType.Hand, true);
    }

    protected override void OnUpgrade()
    {
        CardCmd.RemoveKeyword(this, [CardKeyword.Exhaust]);
    }

    private static IEnumerable<CardModel> AllMinionCards() => ModelDb.CardPool<TokenCardPool>().AllCards.Where((CardModel c) => c.Tags.Contains(CardTag.Minion));
}