namespace dn_mnist;
using DotNetGraph;
using DotNetGraph.Extensions;
using DotNetGraph.Node;
using System.Drawing;
class Program
{
    static void Main(string[] args)
    {
        
        Value a = new Value(10,"a");
        Value b = new Value(20,"b");
        Value c = a + b;
        c.Label = "c";
        
        Value d = new Value(2);
        d.Label = "d";

        Value e = c*d;
        e.Label = "e";

        Value x = new Value(10);
        x.Label = "x";
        Value y = e / x;
        y.Label = "y";

        y.Backward();
        
        DrawGraph(y);
        NeuronTest();
    }

    static void NeuronTest(){
        //inputs
        var x1 = new Value(2.0){Label = "x1"};
        var x2 = new Value(0.0){Label = "x2"};
        //weights
        var w1 = new Value(2.0){Label = "x1"};
        var w2 = new Value(2.0){Label = "x1"};
        //bias
        var b = new Value(6.8813753870195432){Label="b"};

        var x1w1 = x1*w1; x1w1.Label = "x1w1";
        var x2w2 = x2*w2; x2w2.Label = "x2w2";
        var x1w1x2w2 = x1w1 + x2w2; x1w1x2w2.Label = "x1w1 + x2w2";
        var n = x1w1x2w2 + b; n.Label = "n";
        
    }

    static void add_test1(){
        Value a = new Value(10);
        Value b = new Value(20);
        Value c = a + b;
        c.Grad = 1;
        c.Backward();
    }

    static void topo_test(Value root){
        List<Value> topo = new List<Value>();
        List<Value> visited = new List<Value>();
        Value.TopoSort(root,visited,topo);
        foreach(var val in topo){
            Console.WriteLine(val);
        }
    }

    static void DFS(Value root){
        Console.Write(root);
        foreach(var child in root.From){
            Console.Write("(");
            DFS(child);
            Console.Write(")");
        }
    }

    static void DrawGraph(Value root){
        var graph = new DotGraph("Micrograd",true);
        FillGraph(root,graph);
        var dot = graph.Compile(true);
        File.WriteAllText("graph.dot",dot);
    }

    static DotNode FillGraph(Value root,DotGraph graph){
        var node = new DotNode(Guid.NewGuid().ToString());
        node.SetCustomAttribute("shape","record");
        node.Label = $"{root.Label}|{root.Data}|{root.Grad}|{root.Opeartor}";
        graph.Elements.Add(node);
        foreach(var child in root.From){
            var c = FillGraph(child,graph);            
            graph.AddEdge(node,c);
        }
        return node;

    }
}
