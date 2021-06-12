# DarkRift Nat Helper
This plugin is a **rough** attempt to make it easier to port forward routers when using [DarkRift](https://www.darkriftnetworking.com). It will attempt to intelligently port forward the network's router with the correct ports for the server's listeners. The plugin relies on the router supporting [UPnP](https://en.wikipedia.org/wiki/Universal_Plug_and_Play) or [PMP](https://en.wikipedia.org/wiki/NAT_Port_Mapping_Protocol).

Inevitably, DarkRift isn't designed for peer-to-peer networking so really this is a solution to make development easier rather than for use in a production product.

This project is unmaintained and I probably won't accept PRs/respond to issues on it.

Requires Darkrift Pro.

## Usage
Probably built with something like this (I haven't checked recently):
```bash
dotnet publish src/
```

Then drop the generated DLLs in your plugins folder/somewhere on the DR server's plugin search path. It should just magically work from there.
