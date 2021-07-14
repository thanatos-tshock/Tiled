using ModFramework;
using System;
using System.Linq;
using Terraria;
using Tiled.OneDimension;
using Tiled.Struct;
using Tiled.TwoDimensions;

namespace Tiled
{
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
    public class Tiled
    {
        [Modification(ModType.Runtime, "Loading Runtime Example!")]
        public static void OnRunning()
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

        static ICollection<ITile> ParseProviderName(string name)
        {
            if (name == null)
                return null;

            name = name.ToLower().Replace("-", "").Replace("tile", "");
            switch (name)
            {
                case "tsapi":
                case "tshock":
                    var type = Type.GetType("TerrariaApi.Server.TileProvider");
                    if (type == null) throw new Exception("TShock is not installed, unable to use tsapi/tshock tile provider.");
                    return (ICollection<ITile>)type.GetConstructors().Single().Invoke(null);

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

        static bool SetProvider(ICollection<ITile> provider)
        {
            var previous = Terraria.Main.tile as IDisposable;
            Terraria.Main.tile = provider;

            if (previous != null)
                previous.Dispose();

            GC.Collect();

            return true;
        }
    }
}
