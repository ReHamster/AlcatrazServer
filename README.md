# Driver San Francisco Online backend

This work is based on **Ghost Recon Online Backend by Warranty Voider**

this is an experimental implementation of the quazal packet protocol to emulate a backend for GRO

to make the game use this backend make sure following lines are set in Orbit.json (which comes with DriverNGHook)

> OnlineConfigServiceUrl localhost
> OnlineConfigKey 23ad683803a0457cabce83f905811dbc
> OnlineAccessKey 8dtRv2oj

- DareDebuggerWV : tool to interface the daredebug port of the game
- DareParserWV : Extracts custom RTTI information found in different exe and dll
- DTBReaderWV : converts .dtb files to .csv
- GROBackendWV : experimental backend for GRO
- GRODedicatedServerWV : experimental DS for GRO
- GROExplorerWV : tool to browse the yeti.big file for game content
- GROMemoryToolWV : tool to browse various stuctures like lists and trees in memory
- GRO_Hook : proxy dll for easy code injection, hooks currently fire script event functions
