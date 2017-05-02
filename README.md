# Tiled plugin for TShock 
Provides alternate tile implementations which improve performance and memory usage


Simply put Tiled.dll into your ServerPlugins folder. By default it will run the 1d tile provider, but you can use the **ONE** of the following command line arguments to switch to another provider:

```
-tiled 1d     # uses the 1d provider
-tiled 2d     # uses the 2d provider
-tiled struct # uses the struct provider
-tiled tsapi  # uses tshocks default implementation
```