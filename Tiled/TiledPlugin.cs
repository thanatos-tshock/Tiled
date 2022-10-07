using System;
using Terraria;
using TerrariaApi.Server;
using Tiled.OneDimension;
using Tiled.Struct;
using Tiled.TwoDimensions;

namespace Tiled;

/// <summary>
/// The tiled plugin which will be loaded into a TSAPI server.
/// It serves to bootstrap one tile provider.
/// </summary>
/// <remarks>
/// Here's the short on the major differences between my versions and TSAPI's HeapTile.
/// ** based on the release version
/// ** excludes internal array initialisation
/// 
/// 1dTile:
/// 6 instructions used to get a tile from the provider
/// 8 instructions used to set a tile into the provider
/// 1d stores tiles in one flat array, using one int32 index.
/// 
/// 2dTile:
/// 6 instructions used to get a tile from the provider
/// 8 instructions used to set a tile into the provider
/// 2d stores tiles using two dimensions, using two int32 indexes
/// Using int32 instead of int16 because array indexers require int32, which also saves 2 instructions per access
/// 
/// Structured:
/// 7 instructions used to get a tile from the provider
/// 10 instructions used to set a tile into the provider
/// Structured stores tiles in one flat array, using one int32 index - similar to 1d
/// 
/// TSAPI's HeapTile provider:
/// 40+ instructions used to get a tile the provider (i lost count, twice - but you get the point)
/// 14 instructions to set a tile into the provider
/// HeapTiles are stored in a flat one dimensional array, using one int32 offset + constants to calculate data offsets
/// </remarks>
[ApiVersion(2, 1)]
public class TiledPlugin : TerrariaPlugin
{
    public override string Author => "thanatos";
    public override string Description => "Provides alternate memory usage and performance optimisations";
    public override string Name => "Tiled";
    public override Version Version => typeof(TiledPlugin).Assembly.GetName().Version;

    public bool AcceptedWarning { get; set; }

    public TiledPlugin(Main game) : base(game)
    {
    }

    public override void Initialize()
    {
        string tileImplementation = null;

        var args = Environment.GetCommandLineArgs();
        var argumentIndex = Array.FindIndex(args, x => x.ToLower() == "-tiled");
        if (argumentIndex > -1)
        {
            argumentIndex++;

            if (argumentIndex < args.Length)
            {
                tileImplementation = args[argumentIndex];
            }
            else
            {
                Console.WriteLine("Please provide a tile implementation after -tiled. eg -tiled 1d");
            }
        }

        var provider = ParseProviderName(tileImplementation);
        if (provider == null)
        {
            provider = new OneDimensionTileProvider();
        }
        SetProvider(provider);

        Console.WriteLine($"Using tile provider: {Terraria.Main.tile.GetType().Name}");
    }

    /*void AddCommands()
    {
        Commands.ChatCommands.Add(new Command((command_args) =>
        {
            if (command_args.Parameters.Count > 0)
            {
                switch (command_args.Parameters[0])
                {
                    case "set":
                    case "hs":
                    case "hotswap":
                        if (command_args.Parameters.Count <= 1)
                        {
                            command_args.Player.SendInfoMessage($"Usage: tiled hotswap <provider>");
                        }
                        else
                        {
                            var hotprovider = ParseProviderName(command_args.Parameters[1]);
                            if (hotprovider != null)
                            {
                                command_args.Player.SendInfoMessage($"Changing to tile provider: {hotprovider.GetType().Name}");
                                SetProvider(hotprovider);
                                command_args.Player.SendInfoMessage($"Using tile provider: {Terraria.Main.tile.GetType().Name}");
                            }
                            else
                            {
                                command_args.Player.SendInfoMessage($"No such provider named: {command_args.Parameters[1]}");
                            }
                        }
                        return;
                }
            }

            command_args.Player.SendInfoMessage($"Using tile provider: {Terraria.Main.tile.GetType().Name}");
        }, "tiled"));
    }*/

    public ModFramework.ICollection<ITile> ParseProviderName(string name)
    {
        if (name == null)
            return null;

        name = name.ToLower().Replace("-", "").Replace("tile", "");
        switch (name)
        {
            case "tsapi":
            case "tshock":
                return new TileProvider();

            case "2d":
                return new TwoDimensionTileProvider();

            case "1d":
                return new OneDimensionTileProvider();

            case "struct":
            case "structured":
                return new Structured1DTileProvider();
        }
        return null;
    }

    public bool SetProvider(ModFramework.ICollection<ITile> provider)
    {
        //if (!AcceptedWarning)
        //{
        //	Console.Write("This is an experimental command and is only intended for evaluation. Proceed? [Y/n]: ");

        //	var accepted = Console.ReadKey().Key == ConsoleKey.Y;
        //	if (accepted)
        //	{

        //	}

        //	return false;
        //}

        ////todo transfer + unload if during game
        //if (Netplay.IsListening)
        //{
        //    if (Terraria.Main.tile != null)
        //    {
        //        int x = 0;
        //        int y = 0;
        //        try
        //        {
        //            // trigger our internals to generate the data store
        //            provider[0, 0]?.ClearTile();

        //            x = 0;
        //            for (x = 0; x < Terraria.Main.maxTilesX; x++)
        //            {
        //                y = 0;
        //                for (y = 0; y < Terraria.Main.maxTilesY; y++)
        //                {
        //                    provider[x, y] = Terraria.Main.tile[x, y];
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Error @{x}x{y}\n{ex}");
        //            return false;
        //        }
        //    }
        //}

        var previous = Terraria.Main.tile as IDisposable;
        Terraria.Main.tile = provider;
        if (previous != null)
        {
            previous.Dispose();
        }

        GC.Collect();

        return true;
    }
}
