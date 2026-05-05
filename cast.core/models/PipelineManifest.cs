using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace cast.core.models
{
    public enum StageType
    {
        [JsonStringEnumMemberName("shadow")]
        Shadow,

        [JsonStringEnumMemberName("depth_only")]
        DepthPrepass,

        [JsonStringEnumMemberName("geometry")]
        GBuffer,

        [JsonStringEnumMemberName("fullscreen")]
        DeferredLighting,

        [JsonStringEnumMemberName("forward")]
        Forward,

        [JsonStringEnumMemberName("post_process")]
        PostProcessing
    }

    public class Stage
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    
        [JsonPropertyName("type")]
        [JsonConverter(typeof(JsonStringEnumConverter<StageType>))]
        public StageType Type { get; set; }
    
        [JsonPropertyName("entry")]
        public string Entry { get; set; }
    
        [JsonPropertyName("depends_on")]
        public string[] DependsOn { get; set; }
    }

    public class PipelineManifest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    
        [JsonPropertyName("stages")]
        public List<Stage> Stages { get; set; }
    }
}
