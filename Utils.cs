global using Rocket.API.Collections;
global using Rocket.API;
global using Rocket.API.Serialisation;
global using Rocket.Core;
global using Rocket.Core.Assets;
global using Rocket.Core.Logging;
global using Rocket.Core.Plugins;
global using Rocket.Core.Utils;
global using Rocket.Unturned.Chat;
global using Rocket.Unturned.Events;
global using Rocket.Unturned;
global using Rocket.Unturned.Player;
global using SDG.Unturned;
global using SDG.NetPak;
global using SDG.NetTransport;
global using SDG.Framework.Landscapes;
global using SDG.Framework.Utilities;
global using System;
global using System.Runtime.CompilerServices;
global using System.Runtime.InteropServices;
global using System.Collections;
global using System.Collections.Generic;
global using System.Diagnostics;
global using System.IO;
global using System.Linq;
global using System.Reflection;
global using System.Text;
global using System.Text.RegularExpressions;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Xml.Serialization;
global using Steamworks;
global using UnityEngine;
global using Color = UnityEngine.Color;
global using UnityComponent = UnityEngine.Component;
global using UnityObject = UnityEngine.Object;
global using UnityBehaviour = UnityEngine.MonoBehaviour;
global using Logger = Rocket.Core.Logging.Logger;
global using UP = Rocket.Unturned.Player.UnturnedPlayer;
global using UPlayer = Rocket.Unturned.Player.UnturnedPlayer;
global using IRP = Rocket.API.IRocketPlayer;
global using IRPlayer = Rocket.API.IRocketPlayer;
global using SP = SDG.Unturned.SteamPlayer;
global using SPlayer = SDG.Unturned.SteamPlayer;
global using P = SDG.Unturned.Player;
global using Player = SDG.Unturned.Player;
global using Vehicle = SDG.Unturned.InteractableVehicle;
global using Storage = SDG.Unturned.InteractableStorage;
global using InventoryPage = SDG.Unturned.Items;
global using Action = System.Action;
global using SmartUI.Elements;
global using SmartUI.Controllers;
global using static SmartUI.Utils;

namespace SmartUI;

public static partial class Utils
{
    public static bool SameAs(this string a, string b) => a.Equals(b, StringComparison.InvariantCulture);

    public static IEnumerable<int> TryGetIds(this string name, string format)
    {
        var regex = new Regex(Regex.Replace(format, @"\{\d+\}", @"(\d+)"));
        var match = regex.Match(name);
        if (!match.Success)
            return new int[0];
        return match.Groups.Cast<Group>().Skip(1).Select(x => int.Parse(x.Value));
    }

    public static async void ControllersAction<T>(this P player, Func<T, Task> func)
        where T : UIController => await Task.WhenAll(player.GetComponents<T>().Select(func));

    public static void ElementComponentAction<T>(this IEnumerable<Element> elements, string name, Action<T> action)
        where T : Element
    {
        if (action is null)
            return;
        var namedElements = elements.Where(x => x.Name.SameAs(name)).ToList();
        if (namedElements.Count <= 0)
            return;
        foreach (var element in namedElements)
            try
            {
                var decorator = element.Get<T>();
                action.Invoke(decorator!);
            }
            catch { }
    }
    public static T Get<T>(this Element element) where T : Element => element as T ?? (element?.Alias as T) ?? (element?.Alias?.Get<T>());

}