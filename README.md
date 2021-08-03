# Driver San Francisco Online backend

This work is based on [**Ghost Recon Online Backend by Warranty Voider**](https://github.com/zeroKilo/GROBackendWV)

this is an experimental implementation of the quazal packet protocol to emulate a backend for GRO

to make the game use this backend make sure following lines are set in Orbit.json (which comes with DriverNGHook)

> OnlineConfigServiceUrl localhost
> OnlineConfigKey 23ad683803a0457cabce83f905811dbc
> OnlineAccessKey 8dtRv2oj

- GROBackendWV : experimental backend for DSF
- DareDebuggerWV : tool to interface the daredebug port of the game
- DareParserWV : Extracts custom RTTI information found in different exe and dll
- DTBReaderWV : converts .dtb files to .csv
- GROMemoryToolWV : tool to browse various stuctures like lists and trees in memory

# Helpful information

[Protocol/Service information](https://github.com/kinnay/NintendoClients/wiki/NEX-Protocols)