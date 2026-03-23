using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace RTE.Scripts.Cards;

public class MoonLight : CardModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new ForgeVar(3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromForge();
    public MoonLight() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self) {}

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel card = (await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner)).FirstOrDefault();
        if (card != null && card.Type == CardType.Skill)
        {
            await ForgeCmd.Forge(base.DynamicVars.Forge.BaseValue, base.Owner, this);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Forge.UpgradeValueBy(2);
    }
}