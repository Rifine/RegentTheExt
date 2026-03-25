using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using RTE.Scripts.Powers;

namespace RTE.Scripts.Cards;

public class Thronefall : CardModel
{
    private const string powerKey = "ThronefallDamage";

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar(powerKey, 6)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromCardWithCardHoverTips<SovereignBlade>();

    public Thronefall() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.AllAllies, true) { }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<ThronefallPower>(base.Owner.Creature, base.DynamicVars[powerKey].BaseValue, base.Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars[powerKey].UpgradeValueBy(2);
    }
}