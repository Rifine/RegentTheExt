using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace RTE.Scripts.Powers;

public sealed class HeirsAgonyPower : PowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromForge();

    public override async Task BeforeDamageReceived(PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
    {
        if (target == base.Owner && base.Owner.IsPlayer)
        {
            Player player = base.Owner.Player;
            if (player.Creature.Block >= amount)
            {
                Flash();
                await ForgeCmd.Forge(Amount, player, this);
                
            }
        }
    }
}