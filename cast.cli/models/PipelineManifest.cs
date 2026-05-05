using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace cast.cli.models;

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
    [Required]
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [Required]
    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter<StageType>))]
    public StageType Type { get; set; }
    
    [Required]
    [JsonPropertyName("entry")]
    public string Entry { get; set; }
    
    [JsonPropertyName("depends_on")]
    public string[] DependsOn { get; set; }
}

public class PipelineManifest
{
    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [Required]
    [JsonPropertyName("stages")]
    public List<Stage> Stages { get; set; }
}