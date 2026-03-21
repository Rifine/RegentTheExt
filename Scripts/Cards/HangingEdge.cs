using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace RTE.Scripts.Cards;

public class HangingEdge : CardModel
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => IsUpgraded? HoverTipFactory.FromForge().Append(HoverTipFactory.FromKeyword(CardKeyword.Exhaust)) : [HoverTipFactory.FromCard<SovereignBlade>(), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ForgeVar(4)];
    public HangingEdge() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, true)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (IsUpgraded)
        {
            await ForgeCmd.Forge(DynamicVars.Forge.IntValue, Owner, this);
        }
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
    }
}