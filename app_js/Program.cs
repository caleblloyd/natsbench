﻿using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;




/*
 
 
 
 
 

            
              ██ ███████ ████████ ███████ ████████ ██████  ███████  █████  ███    ███ 
              ██ ██         ██    ██         ██    ██   ██ ██      ██   ██ ████  ████ 
              ██ █████      ██    ███████    ██    ██████  █████   ███████ ██ ████ ██ 
         ██   ██ ██         ██         ██    ██    ██   ██ ██      ██   ██ ██  ██  ██ 
          █████  ███████    ██    ███████    ██    ██   ██ ███████ ██   ██ ██      ██ 
                                                                                      
                        ███    ██ ███████ ████████              ██████       
                        ████   ██ ██         ██                      ██      
                        ██ ██  ██ █████      ██        ██    ██  █████       
                        ██  ██ ██ ██         ██         ██  ██  ██           
                     ██ ██   ████ ███████    ██          ████   ███████      
                                                                                             
                ██████  ██████  ███████ ██    ██ ██ ███████ ██     ██      ██                
                ██   ██ ██   ██ ██      ██    ██ ██ ██      ██     ██     ███                
                ██████  ██████  █████   ██    ██ ██ █████   ██  █  ██      ██                
                ██      ██   ██ ██       ██  ██  ██ ██      ██ ███ ██      ██                
                ██      ██   ██ ███████   ████   ██ ███████  ███ ███       ██                
                                                                                             
                                                                             




                                                                                               
*/








/*******************************************************************************************/
//
//  J E T S T R E A M   C O N T E X T
//

var nats = new NatsConnection();

var js = new NatsJSContext(nats);








/*******************************************************************************************/





















/*******************************************************************************************/
//
//  A C C O U N T   I N F O 
//

var account = await js.GetAccountInfoAsync();

Console.WriteLine($$"""
                    Account
                    Domain: {{account.Domain}}
                    Consumers: {{account.Consumers}}
                    Streams: {{account.Streams}}
                    Api.Errors: {{account.Api.Errors}}
                    Api.Total: {{account.Api.Total}}
                    Limits.MaxConsumers: {{account.Limits.MaxConsumers}}
                    Limits.MaxStreams: {{account.Limits.MaxStreams}}
                    Limits.MaxStorage: {{account.Limits.MaxStorage}}
                    """);



/*******************************************************************************************/








/*******************************************************************************************/
//
//  S T R E A M S
//


// CREATE STREAM

var stream1 = await js.CreateStreamAsync("stream1", subjects: new[] { "stream1.*" });

stream1 = await js.CreateStreamAsync(new StreamConfiguration
{
    Name = "stream1",
    Subjects = new[] { "stream1.*" },
    Retention = StreamConfigurationRetention.workqueue,
});








 // GET STREAM

stream1 = await js.GetStreamAsync("stream1");

Console.WriteLine($$"""
                    Stream Info:
                    Name: {{ stream1.Info.Config.Name }}
                    Subjects: {{ string.Join(",", stream1.Info.Config.Subjects) }}
                    Created: {{ stream1.Info.Created }}
                    """);











// LIST STREAMS
{
    var list = await js.ListStreamsAsync(new StreamListRequest { Subject = "stream1.*" });

    foreach (var stream in list.Streams)
        Console.WriteLine($"Stream: {stream.Config.Name}");

    Console.WriteLine($"Streams" +
                      $" offset:{list.Offset}," +
                      $" limit:{list.Limit}," +
                      $" total:{list.Total}");

}











// UPDATE STREAM

stream1 = await js.UpdateStreamAsync(new StreamUpdateRequest
{
    Name = "stream1",
    MaxMsgs = 1_000_000,
});

Console.WriteLine($"New stream max msgs: {stream1.Info.Config.MaxMsgs}");
















// DELETE STREAM

var isStreamDeleted = await stream1.DeleteAsync();

if (!isStreamDeleted)
    Console.WriteLine($"Error deleting stream {stream1.Info.Config.Name}");






/*******************************************************************************************/















/*******************************************************************************************/
//
//  C O N S U M E R S
//


// CREATE CONSUMER

var consumer1 = await js.CreateConsumerAsync("stream1", "consumer1");

consumer1 = await js.CreateConsumerAsync(new ConsumerCreateRequest
{
    StreamName = "stream1",
    Config = new ConsumerConfiguration
    {
        AckPolicy = ConsumerConfigurationAckPolicy.@explicit,
        DurableName = "consumer1",
        Name = "consumer1",
    },
});












// LIST CONSUMERS
{
    var list = await js.ListConsumersAsync("stream1", new ConsumerListRequest { Offset = 0 });

    foreach (var consumer in list.Consumers)
    {
        Console.WriteLine($"Consumer: {consumer.Name}");
    }

    Console.WriteLine($$"""
                        List:
                            Offset: {{list.Offset}}
                            Limit: {{list.Limit}}
                            Total: {{list.Total}}
                        """);

}










// GET CONSUMER

consumer1 = await js.GetConsumerAsync("stream1", "consumer1");

Console.WriteLine($$"""
                  Consumer:
                      Name: {{ consumer1.Info.Name }}
                      Stream: {{ consumer1.Info.StreamName }}
                      Created: {{ consumer1.Info.Created }}
                  """);
















// CONSUMING MESSAGES






{ // NEXT
    
    var next = await consumer1.NextAsync<Data>(new NatsJSNextOpts
    {
        ErrorHandler = ErrorHandler,
        Expires = TimeSpan.FromSeconds(30),
    });

    if (next is { } msg)
    {
        Console.WriteLine($"{msg.Subject}: {msg.Data.Id}");
        await msg.AckAsync();
        
        // or
        // await msg.NackAsync();
        // await msg.AckProgressAsync();
        // await msg.AckTerminateAsync();
    }
    
    void ErrorHandler(INatsJSFetch consumer, NatsJSNotification notification)
    {
        Console.WriteLine($"Error: {notification.Code} {notification.Description}");
    }
}













{ // FETCH
    
    var opts = new NatsJSFetchOpts
    {
        MaxMsgs = 100,
        // MaxBytes = 1024, // Either bytes or msgs, throw exception otherwise
        Expires = TimeSpan.FromMinutes(1),
        IdleHeartbeat = TimeSpan.FromSeconds(10),
        ErrorHandler = ErrorHandler,
    };

    var fetch = await consumer1.FetchAsync<Data>(opts);
    
    await foreach (var msg in fetch.Msgs.ReadAllAsync())
    {
        Console.WriteLine($"{msg.Subject}: {msg.Data.Id}");
        await msg.AckAsync();
    }

    
    void ErrorHandler(INatsJSFetch consumer, NatsJSNotification notification)
    {
        Console.WriteLine($"Error: {notification.Code} {notification.Description}");
        consumer.Stop();
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    // Alternative fetch all
    
    await foreach (var msg in consumer1.FetchAllAsync<Data>(new NatsJSFetchOpts { MaxMsgs = 32 }))
    {
        Console.WriteLine($"{msg.Subject}: {msg.Data.Id}");
        await msg.AckAsync();
    }
    
    
}










{ // CONSUME
    
    var opts = new NatsJSConsumeOpts
    {
        MaxBytes = 1024,
        ThresholdBytes = 256, // default is half of max
        // MaxMsgs = 100, // only allow msgs or bytes, throw exception otherwise
        Expires = TimeSpan.FromMinutes(2),
        ErrorHandler = ErrorHandler,
    };
    
    var consume = await consumer1.ConsumeAsync<Data>(opts);

    await foreach (var msg in consume.Msgs.ReadAllAsync())
    {
        Console.WriteLine($"{msg.Subject}: {msg.Data.Id}");
        await msg.AckAsync();
    }


    void ErrorHandler(INatsJSConsume consumer, NatsJSNotification notification)
    {
        Console.WriteLine($"Error: {notification.Code} {notification.Description}");
        consumer.Stop();
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    // Alternative consume all using asynchronous enumerable
    
    await foreach (var msg in consumer1.ConsumeAllAsync<Data>(new NatsJSConsumeOpts { MaxMsgs = 64 }))
    {
        Console.WriteLine($"{msg.Subject}: {msg.Data.Id}");
        await msg.AckAsync();
    }
    
    
}















{ // DELETE
    
    var isConsumerDeleted = await consumer1.DeleteAsync();
    
    if (!isConsumerDeleted)
        Console.WriteLine("Error deleting consumer");
    
}














/*******************************************************************************************/
//
//  P U B L I S H
//

var ack = await js.PublishAsync("stream1.foo", new Data { Id = 1 });

ack = await js.PublishAsync("stream1.foo", new Data { Id = 2 }, new NatsPubOpts
{
    Headers = new NatsHeaders { { "Nats-Msg-Id", "2" } },
});

Console.WriteLine($$"""
                    ACK:
                    Domain: {{ ack.Domain }}
                    Stream: {{ ack.Stream }}
                    Duplicate: {{ ack.Duplicate }}
                    Seq: {{ ack.Seq }}
                    API Error: {{ ack.Error }}
                    """);

ack.EnsureSuccess();










/*******************************************************************************************/







public class Data
{
    public int Id { get; set; }
}
