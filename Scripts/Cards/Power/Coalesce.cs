using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using RTE.Scripts.Powers;

namespace RTE.Scripts.Cards;

public class Coalesce : CardModel
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromForge().Append(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CoalescePower>(3)];
    public Coalesce() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self) { }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<CoalescePower>(base.Owner.Creature, base.DynamicVars["CoalescePower"].IntValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        CardCmd.ApplyKeyword(this, CardKeyword.Innate);
    }
}