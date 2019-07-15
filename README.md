# UDPSync
Console application that synchronizes in a fixed rate background thread an ExampleObject in one way from [Client] --> to [Server] via UDP protocol

* [Program.cs](/Program.cs) - The Example Client/Server Console app with one way object synchronization via UDP
* [GameExample.cs](/GameExample.cs) - Simple example in a game that sends the position one-way
* [UDPSync/ObjectSyncClient.cs](/UDPSync/ObjectSyncClient.cs) - Sends Object
* [UDPSync/ObjectSyncServer.cs](/UDPSync/ObjectSyncServer.cs) - Receives Object
* [UDPSync/UDP/UDPClient.cs](/UDPSync/UDP/UDPClient.cs) - UDP Client that sends a byte array with a fixed buffer size
* [UDPSync/UDP/UDPServer.cs](/UDPSync/UDP/UDPServer.cs) - UDP Server that listens for byte array packet and stores it in it's ReceivedData property
* [UDPSync/Serialization/Serializer.cs](/UDPSync/Serialization/Serializer.cs) - Serialization/Deserialization Methods

