# Alcatraz
![](https://i.ibb.co/gJFYt04/alcatraz-logo.jpg)

**Driver San Francisco PC Online backend**

This server mimics the behaviour of Quazal Rendez-Vous server and protocols used by Driver San Francisco.

To make the game use this backend make sure following lines are set in Orbit.json (which comes with DriverNGHook):

```
OnlineConfigServiceUrl "your_server_address"
OnlineConfigKey "23ad683803a0457cabce83f905811dbc"
OnlineAccessKey "8dtRv2oj"
```

Main projects
- AlcatrazGameServices : the Web API and game server
- - AlcatrazDbContext : entities and database context
- BackendDebugServer : local debug server application
- - QNetZ : implementation of Quazal Net-Z protocol and RMC (Remote Method Calls)
- - RDVServices : Rendez-Vous authentication services
- - DSFSErvices : Driver SF game servcies

The tools:
- DareDebuggerWV : tool to interface the daredebug port of the game
- DareParserWV : Extracts custom RTTI information found in different exe and dll
- DTBReaderWV : converts .dtb files to .csv
- GROMemoryToolWV : tool to browse various stuctures like lists and trees in memory

# Helpful information

[Protocol/Service information](https://github.com/kinnay/NintendoClients/wiki/NEX-Protocols)

# Credits

- SoapyMan - project lead
- VortexLeBelge - for providing his game account for research purpose
- Warranty Voider - [**original Ghost Recon Online Backend**](https://github.com/zeroKilo/GROBackendWV)
