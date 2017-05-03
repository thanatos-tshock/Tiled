# Tiled plugin for TShock 
Provides alternate tile implementations which improve performance and memory usage


Simply put Tiled.dll into your ServerPlugins folder. By default it will run the 1d tile provider, but you can use the **ONE** of the following command line arguments to switch to another provider:

```
-tiled 1d     # uses the 1d provider
-tiled 2d     # uses the 2d provider
-tiled struct # uses the struct provider
-tiled tsapi  # uses tshocks default implementation
```

### How do these new providers differ from Re-Logic and TShock HeapTile implementations?
Currently, on Windows, if you are running a large world using Re-Logics tile mechanisms your server would be using over 600mb RAM.

The TShock team try and aid this with their HeapTile implementation which will reduce this large world to around 350mb (pretty neat huh?), however there is a trade-off using HeapTile - it's very slow.

This is where my versions step in, they aim to give you the best of both worlds. My 1d & 2d implementation run on-par with Re-Logics implementation AND with TShocks memory usage.

### What provider should I use?
Generally using the 1d or 2d will suffice as they are nearly the exact same thing, however you might want to trial each version to see which runs best on your machine.
Below is a short non-technical comparison of my providers:

- 1d 		- stores the data in one location and each 1d tile will access the data using **ONE** offset
- 2d 		- stores the data in one location and each 2d tile will access the data using **TWO** coordinates (x,y)
- struct	- the same setup as a 1d tile, but is an experimental version that 

For a slightly more technical explanation see the comments at the top of the file [here](https://github.com/thanatos-tshock/Tiled/blob/master/Tiled/TiledPlugin.cs).