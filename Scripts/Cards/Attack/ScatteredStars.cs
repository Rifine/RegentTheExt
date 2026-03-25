using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace RTE.Scripts.Cards;

public class ScatteredStars : CardModel
{
    public override int CanonicalStarCost => 2;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromForge();
    public ScatteredStars() : base(0, CardType.Attack, CardRarity.Common, TargetType.AllEnemies) {}
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(4, ValueProp.Move),
        new DynamicVar("HitsCount", 3),
        new ForgeVar(3),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        AttackCommand attackCommand = await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingRandomOpponents(base.CombatState)
            .WithHitCount(base.DynamicVars["HitsCount"].IntValue)
            .Execute(choiceContext);
        int forgeCount = attackCommand.Results.Select((DamageResult r) => r.Receiver).Distinct().Count();
        for (int i = 0; i < forgeCount; i++)
        {
            await ForgeCmd.Forge(base.DynamicVars.Forge.IntValue, base.Owner, this);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["HitsCount"].UpgradeValueBy(1);
    }
}