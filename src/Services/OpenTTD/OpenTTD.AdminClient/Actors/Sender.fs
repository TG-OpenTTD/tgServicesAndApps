﻿module OpenTTD.AdminClient.Actors.Sender


open System.Net.Sockets
open Akka.FSharp

open Microsoft.Extensions.Logging
open OpenTTD.AdminClient.Networking.MessageTransformer
open OpenTTD.AdminClient.Networking.Packet
 
 
let init (logger : ILogger) (tcpClient : TcpClient) (mailbox : Actor<AdminMessage>) =
    
    logger.LogInformation "[Sender:init]"
    
    let stream = tcpClient.GetStream()
    
    mailbox.Defer (fun _ ->
        logger.LogInformation "[Sender:stopping] Taking pill instance"
        stream.Dispose())

    let rec loop () =
        actor {      
            let! msg = mailbox.Receive ()

            logger.LogDebug $"[Sender:send] msg: %A{msg}"            
            
            let { Buffer = buf; Size = size; } = msg |> msgToPacket |> prepareToSend
            stream.Write (buf, 0, int size)
            return! loop ()
        }
        
    loop ()
    