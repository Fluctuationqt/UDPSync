# UDPSync
Synchronize objects between clients via UDP.

* [Program.cs](/Program.cs)
* [UDPSync/ObjectSyncClient.cs](/UDPSync/ObjectSyncClient.cs) - Sends Object
* [UDPSync/ObjectSyncServer.cs](/UDPSync/ObjectSyncServer.cs) - Receives Object
* [UDPSync/UDP/UDPClient.cs](/UDPSync/UDP/UDPClient.cs) - UDP Client that sends a byte array with a fixed buffer size
* [UDPSync/UDP/UDPServer.cs](/UDPSync/UDP/UDPServer.cs) - UDP Server that listens for byte array packet and stores it in it's ReceivedData property
* [UDPSync/Serialization/Serializer.cs](/UDPSync/Serialization/Serializer.cs) - Serialization/Deserialization Methods
