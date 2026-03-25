using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace RTE.Scripts.Cards;

public class Cooking : CardModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<PlatingPower>(4),
        new ForgeVar(4)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromForge()
        .Append(HoverTipFactory.FromKeyword(CardKeyword.Exhaust))
        .Append(HoverTipFactory.FromPower<PlatingPower>());
    public Cooking() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel cardModel = (await CardSelectCmd.FromHand(choiceContext, base.Owner, new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1), null, this)).FirstOrDefault();
        if (cardModel != null)
        {
            await CardCmd.Exhaust(choiceContext, cardModel);
            await PowerCmd.Apply<PlatingPower>(base.Owner.Creature, base.DynamicVars["PlatingPower"].BaseValue, base.Owner.Creature, this);
            await ForgeCmd.Forge(base.DynamicVars.Forge.BaseValue, base.Owner, this);
        }
    }
    
    protected override void OnUpgrade()
    {
        base.DynamicVars["PlatingPower"].UpgradeValueBy(1);
        base.DynamicVars.Forge.UpgradeValueBy(2);
    }
}