using Godot.Bridge;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using RTE.Scripts.Cards;

namespace RTE.Scripts;

[ModInitializer("Init")]
public class Entry
{
    public static void Init()
    {
        ScriptManagerBridge.LookupScriptsInAssembly(typeof(Entry).Assembly);

        Entry.AddCards();

        Log.Debug("[RTE] initialized");
    }

    static void AddCards()
    {
        AddCardToPool<Gimme>();
        AddCardToPool<Thronefall>();
        AddCardToPool<HangingEdge>();
        AddCardToPool<HeirsAgony>();
    }

    static void AddCardToPool<Card>() where Card: CardModel
    {
        ModHelper.AddModelToPool<RegentCardPool, Card>();
    }
}