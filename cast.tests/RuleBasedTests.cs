using cast.cli.builder;
using cast.core.models;
using cast.core.models.symbols;
using cast.core.parser;
using cast.core.parser.programs;

namespace cast.tests;

public class RuleBasedTests
{
    private static GlslShaderProgram Compile(string source)
    {
        var parser = new GlslParser();
        return parser.Parse(source);
    }

    private static bool Compiles(string source)
    {
        var parser = new GlslParser();
        parser.Parse(source);
        return parser.GetLogs().Count == 0;
    }

    [Fact]
    public void T1_MatrixPointTransformation_Accept()
    {
        var source = @"
#version 460
void main() {
    point4<Model> p;
    mat4<Model, World> m;
    point4<World> r = m * p;
}";
        Assert.True(Compiles(source));
    }

    [Fact]
    public void T2_MatrixSourceSpaceMismatch_Reject()
    {
        var source = @"
#version 460
void main() {
    point4<Model> p;
    mat4<World, View> m;
    point4<View> r = m * p;
}";
        Assert.False(Compiles(source));
    }

    [Fact]
    public void T3_VectorAdditionDifferentSpaces_Reject()
    {
        var source = @"
#version 460
void main() {
    vec3<World> a;
    vec3<Model> b;
    vec3<World> c = a + b;
}";
        Assert.False(Compiles(source));
    }

    [Fact]
    public void T4_PointPlusVectorSameSpace_Accept()
    {
        var source = @"
#version 460
void main() {
    point3<World> p;
    vec3<World> v;
    point3<World> r = p + v;
}";
        Assert.True(Compiles(source));
    }

    [Fact]
    public void T5_AddTwoPoints_Reject()
    {
        var source = @"
#version 460
void main() {
    point3<World> a;
    point3<World> b;
    point3<World> r = a + b;
}";
        Assert.False(Compiles(source));
    }

    [Fact]
    public void T6_PipelineCompatible_Accept()
    {
        var vertexShader = @"
#version 460
out point4<World> color;
void main() {
    color = point4(1.0, 1.0, 1.0, 1.0);
}";
        var fragmentShader = @"
#version 460
in point4<World> color;
out point4<World> frag;
void main() {
    frag = color;
}";

        var vshProgram = new GlslParser().Parse(vertexShader);
        var fshProgram = new GlslParser().Parse(fragmentShader);

        var vshNode = new Node("stage", "test.vsh", null, StageType.GBuffer);
        vshNode.Outputs = vshProgram.Outputs;

        var fshNode = new Node("stage", "test.fsh", null, StageType.GBuffer);
        fshNode.Inputs = fshProgram.Inputs;

        var errors = GraphBuilder.Wire(new List<Node> { vshNode, fshNode });
        Assert.Empty(errors);
    }

    [Fact]
    public void T7_PipelineIncompatible_Reject()
    {
        var vertexShader = @"
#version 460
out point4<World> color;
void main() {
    color = point4(1.0, 1.0, 1.0, 1.0);
}";
        var fragmentShader = @"
#version 460
in point4<View> color;
out point4<View> frag;
void main() {
    frag = color;
}";

        var vshProgram = new GlslParser().Parse(vertexShader);
        var fshProgram = new GlslParser().Parse(fragmentShader);

        var vshNode = new Node("stage", "test.vsh", null, StageType.GBuffer);
        vshNode.Outputs = vshProgram.Outputs;

        var fshNode = new Node("stage", "test.fsh", null, StageType.GBuffer);
        fshNode.Inputs = fshProgram.Inputs;

        var errors = GraphBuilder.Wire(new List<Node> { vshNode, fshNode });
        Assert.NotEmpty(errors);
    }

    [Fact]
    public void T8_AnnotationErasure_NoAngleBracketsInOutput()
    {
        var source = @"
#version 460
in vec3<World> normal;
out vec4<Clip> color;
void main() {
    vec3<World> n = normalize(normal);
    color = vec4(n, 1.0);
}";
        var program = Compile(source);
        var output = program.GetShaderCode();

        Assert.DoesNotContain("<", output);
        Assert.DoesNotContain(">", output);
        Assert.DoesNotContain("point4", output);
    }
}
