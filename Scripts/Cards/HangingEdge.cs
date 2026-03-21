using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace RTE.Scripts.Cards;

public class HangingEdge : CardModel
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<SovereignBlade>(), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    public HangingEdge() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, true)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        IEnumerable<CardModel> blades = PileType.Hand.GetPile(base.Owner).Cards.Where((CardModel c) => c is SovereignBlade).ToList();
        foreach (CardModel blade in blades)
        {
            blade.AddKeyword(CardKeyword.Exhaust);
            if (!CombatManager.Instance.IsOverOrEnding)
            {
                await CardCmd.AutoPlay(choiceContext, blade, cardPlay.Target, AutoPlayType.Default, false, false);
                continue;
            }
            break;
        }
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}