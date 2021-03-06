using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Event;
using Akka.Logger.Serilog;
using Akka.Util;
using OpenTTD.Actors.Server;
using Domain.Models;
using Domain.ValueObjects;

namespace OpenTTD.Actors.Coordinator;

public sealed class CoordinatorActor : ReceiveActor
{
    private readonly ILoggingAdapter _logger = Context.GetLogger<SerilogLoggingAdapter>();
    
    public CoordinatorActor()
    {
        Dictionary<ServerId, (ServerCredentials Credentials, State ServerState, IActorRef Ref)> servers = new();
        
        Receive<ServerAdd>(msg =>
        {
            if (servers.TryGetValue(msg.ServerId, out _))
            {
                _logger.Warning(
                    "[{ServerId}] Server already added", 
                    msg.ServerId.Value);
                
                Sender.Tell(Result.Success(new ServerAdded(msg.ServerId)));
            }
            else
            {
                var serverProps = DependencyResolver
                    .For(Context.System)
                    .Props<ServerActor>(msg.ServerId, msg.Credentials);
                var serverRef = Context.ActorOf(serverProps);

                servers.Add(msg.ServerId, (msg.Credentials, State.IDLE, serverRef));
                
                _logger.Info(
                    "[{ServerId}] Server was added", 
                    msg.ServerId.Value);
                
                Sender.Tell(Result.Success(new ServerAdded(msg.ServerId)));
            }
        });

        Receive<ServerConnect>(msg =>
        {
            if (servers.TryGetValue(msg.ServerId, out var data))
            {
                if (data.ServerState is State.CONNECTED or State.CONNECTING)
                {
                    _logger.Warning(
                        "[{ServerId}] Server is connected but connect called", 
                        msg.ServerId.Value);
                    return;
                }
                
                _logger.Info(
                    "[{ServerId}] Server will be connected", 
                    msg.ServerId.Value);
                data.Ref.Tell(new Connect());
            }
            else
            {
                _logger.Warning(
                    "[{ServerId}] Server was not found while connecting", 
                    msg.ServerId.Value);
            }
        });
        
        Receive<ServerDisconnect>(msg =>
        {
            if (servers.TryGetValue(msg.ServerId, out var data))
            {
                if (data.ServerState is not State.CONNECTED)
                {
                    _logger.Warning(
                        "[{ServerId}] Server is not connected but disconnect called", 
                        msg.ServerId.Value);
                    return;
                }
                
                data.Ref.Tell(new Disconnect());
                _logger.Info(
                    "[{ServerId}] Server will be disconnected", 
                    msg.ServerId.Value);
            }
            else
            {
                _logger.Warning(
                    "[{ServerId}] Server was not found while disconnecting", 
                    msg.ServerId.Value);
            }
        });
        
        Receive<ServerRemove>(msg =>
        {
            if (servers.TryGetValue(msg.ServerId, out var data))
            {
                data.Ref.Tell(PoisonPill.Instance);

                servers.Remove(msg.ServerId);
                
                _logger.Info(
                    "[{ServerId}] Server was added", 
                    msg.ServerId.Value);
            }
            else
            {
                _logger.Warning(
                    "[{ServerId}] Server was not found while remove", 
                    msg.ServerId.Value);
            }
        });

        Receive<ServerStateChanged>(msg =>
        {
            if (servers.TryGetValue(msg.ServerId, out var data))
            {
                servers[msg.ServerId] = (data.Credentials, msg.State, data.Ref);
                
                _logger.Info(
                    "[{ServerId}] Server state was modified to {State}", 
                    msg.ServerId.Value, msg.State);
            }
            else
            {
                _logger.Warning(
                    "[{ServerId}] Server was not found while changing state", 
                    msg.ServerId.Value);
            }
        });
    }
}