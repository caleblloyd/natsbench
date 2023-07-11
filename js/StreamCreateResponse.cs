// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using QuickType;
//
//    var streamCreateResponse = StreamCreateResponse.FromJson(jsonString);

namespace QuickType
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class StreamCreateResponse
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("config")]
        public Config Config { get; set; }

        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("state")]
        public State State { get; set; }

        [JsonProperty("did_create")]
        public bool DidCreate { get; set; }
    }

    public partial class Config
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("subjects")]
        public string[] Subjects { get; set; }

        [JsonProperty("retention")]
        public string Retention { get; set; }

        [JsonProperty("max_consumers")]
        public long MaxConsumers { get; set; }

        [JsonProperty("max_msgs")]
        public long MaxMsgs { get; set; }

        [JsonProperty("max_bytes")]
        public long MaxBytes { get; set; }

        [JsonProperty("max_age")]
        public long MaxAge { get; set; }

        [JsonProperty("max_msgs_per_subject")]
        public long MaxMsgsPerSubject { get; set; }

        [JsonProperty("max_msg_size")]
        public long MaxMsgSize { get; set; }

        [JsonProperty("discard")]
        public string Discard { get; set; }

        [JsonProperty("storage")]
        public string Storage { get; set; }

        [JsonProperty("num_replicas")]
        public long NumReplicas { get; set; }

        [JsonProperty("duplicate_window")]
        public long DuplicateWindow { get; set; }

        [JsonProperty("allow_direct")]
        public bool AllowDirect { get; set; }

        [JsonProperty("mirror_direct")]
        public bool MirrorDirect { get; set; }

        [JsonProperty("sealed")]
        public bool Sealed { get; set; }

        [JsonProperty("deny_delete")]
        public bool DenyDelete { get; set; }

        [JsonProperty("deny_purge")]
        public bool DenyPurge { get; set; }

        [JsonProperty("allow_rollup_hdrs")]
        public bool AllowRollupHdrs { get; set; }
    }

    public partial class State
    {
        [JsonProperty("messages")]
        public long Messages { get; set; }

        [JsonProperty("bytes")]
        public long Bytes { get; set; }

        [JsonProperty("first_seq")]
        public long FirstSeq { get; set; }

        [JsonProperty("first_ts")]
        public DateTimeOffset FirstTs { get; set; }

        [JsonProperty("last_seq")]
        public long LastSeq { get; set; }

        [JsonProperty("last_ts")]
        public DateTimeOffset LastTs { get; set; }

        [JsonProperty("consumer_count")]
        public long ConsumerCount { get; set; }
    }

    public partial class StreamCreateResponse
    {
        public static StreamCreateResponse FromJson(string json) => JsonConvert.DeserializeObject<StreamCreateResponse>(json, QuickType.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this StreamCreateResponse self) => JsonConvert.SerializeObject(self, QuickType.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}