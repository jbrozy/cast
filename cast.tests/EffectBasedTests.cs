using cast.core.parser;

namespace cast.tests;

public class EffectBasedTests
{
    private static bool Compiles(string source)
    {
        var parser = new GlslParser();
        parser.Parse(source);
        return parser.GetLogs().Count == 0;
    }
    
    private static List<string> GetLogs(string source)
    {
        var parser = new GlslParser();
        parser.Parse(source);
        return parser.GetLogs();
    }

    [Fact]
    public void AcceptsNdotL()
    {
        var source = @"
#version 460
vec3<World> normal;
vec3<World> lightDirection;

void main(){
    float diffuse = dot(normal, lightDirection);
}
                     ";
        bool result = Compiles(source);
        Assert.True(result);
    }
    
    [Fact]
    public void RejectsNdotL()
    {
        var source = @"
#version 460
vec3<Model> normal;
vec3<World> lightDirection;

void main(){
    float diffuse = dot(normal, lightDirection);
}
                     ";
        bool result = Compiles(source);
        Assert.False(result);
    }
    
    [Fact]
    public void RejectsWorldSpaceWindOffsetOnModelSpacePosition()
    {
        var source = @"
#version 460
point4<Model> localPosition;
vec4<World> windOffset;

void main() {
    point4<Model> movedPosition = localPosition + windOffset;
}
                     ";

        bool result = Compiles(source);

        Assert.False(result);
    } 
    
    [Fact]
    public void AcceptsWorldSpaceWindOffsetAfterWorldTransformation()
    {
        var source = @"
#version 460
point4<Model> localPosition;

mat4<Model, World> modelMatrix;
mat4<World, View> viewMatrix;
mat4<View, Clip> projectionMatrix;

vec4<World> windOffset;

void main() {
    point4<World> worldPosition = modelMatrix * localPosition;
    point4<World> movedWorldPosition = worldPosition + windOffset;

    point4<View> viewPosition = viewMatrix * movedWorldPosition;
    point4<Clip> clipPosition = projectionMatrix * viewPosition;
}
                     ";

        bool result = Compiles(source);

        Assert.True(result);
    }

    [Fact]
    public void RejectFogDistance()
    {
        var source = @"
#version 460
point3<World> cameraPosition;
point3<View> fragPosition;

void main() {
    vec3<World> distanceVector = fragPosition - cameraPosition;
    float dist = length(distanceVector);
}
                     ";
        var logs = GetLogs(source);
        Assert.NotEmpty(logs);
        Assert.Contains(logs, log =>
            log.Contains("World") && log.Contains("View"));
    }
    
    [Fact]
    public void AcceptFogDistance()
    {
        var source = @"
#version 460
point3<World> cameraPosition;
point3<World> fragPosition;

void main() {
    vec3<World> distanceVector = fragPosition - cameraPosition;
    float distance = length(distanceVector);
}
                     ";
        bool result = Compiles(source);
        Assert.True(result);
    }

    [Fact]
    public void RejectPhongSpecular()
    {
        var source = @"
#version 460
vec3<World> reflectDirection;
vec3<View> viewDirection;

void main() {
    float specular = dot(reflectDirection, viewDirection);
}
                     ";
        bool result = Compiles(source);
        Assert.False(result);       
    }
    
    [Fact]
    public void AcceptPhongSpecular()
    {
        var source = @"
#version 460
vec3<World> reflectDirection;
vec3<World> viewDirection;

void main() {
    float specular = dot(reflectDirection, viewDirection);
}
                     ";
        bool result = Compiles(source);
        Assert.True(result);       
    }

    [Fact]
    public void RejectMVPChain()
    {
        var source = @"
#version 460
mat4<Model, World> modelMatrix;
mat4<World, View> viewMatrix;
mat4<View, Clip> projectionMatrix;

void main() {
    mat4<Model, Clip> MVP = projectionMatrix * modelMatrix * viewMatrix;
}
                     ";
        bool result = Compiles(source);
        Assert.False(result);              
    }
    
    [Fact]
    public void AcceptMVPChain()
    {
        var source = @"
#version 460
mat4<Model, World> modelMatrix;
mat4<World, View> viewMatrix;
mat4<View, Clip> projectionMatrix;

void main() {
    mat4<Model, Clip> MVP = projectionMatrix * viewMatrix * modelMatrix;
}
                     ";
        bool result = Compiles(source);
        Assert.True(result);              
    }
}