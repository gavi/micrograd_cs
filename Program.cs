namespace dn_mnist;
using DotNetGraph;
using DotNetGraph.Extensions;
using DotNetGraph.Node;
class Program
{
    static void Main(string[] args)
    {
        
        Neuron n = new Neuron(5);
        var output = n.Call(new Value[]{1,2,3,4,5});
        output.Backward();
        DrawGraph(output);
        //NeuronTest();

        MLP x = new MLP(3,new List<(int, int)>(){(3,0),(5,3),(2,5)});
    }

    static void BasicTest(){
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
    }
    //Matching what karpahy had in the video
    static void NeuronTest(){
        //inputs
        var x1 = new Value(2.0){Label = "x1"};
        var x2 = new Value(0.0){Label = "x2"};
        //weights
        var w1 = new Value(-3.0){Label = "w1"};
        var w2 = new Value(1.0){Label = "w2"};
        //bias
        var b = new Value(6.8813753870195432){Label="b"};

        var x1w1 = x1*w1; x1w1.Label = "x1w1";
        var x2w2 = x2*w2; x2w2.Label = "x2w2";
        var x1w1x2w2 = x1w1 + x2w2; x1w1x2w2.Label = "x1w1 + x2w2";
        var n = x1w1x2w2 + b; n.Label = "n";

        // var e = Value.Exp(2*n);
        // var o = (e - 1) / (e + 1);
        var o = n.Tanh();o.Label = "o";
        o.Backward();
        // o.Grad = 1;
        // o._backward();
        // n._backward();
        DrawGraph(o);
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
        List<Value> visited = new List<Value>();
        FillGraph(root,graph,visited);
        var dot = graph.Compile(true);
        File.WriteAllText("graph.dot",dot);
    }

    static DotNode? FillGraph(Value root,DotGraph graph,List<Value> visited){
        if (visited.Contains(root)){
            return null;
        }
        visited.Add(root);
        var node = new DotNode(Guid.NewGuid().ToString());
        node.SetCustomAttribute("shape","record");
        node.Label = $"{root.Label}|{root.Data.ToString("F4")}|{root.Grad.ToString("F4")}|{root.Opeartor}";
        graph.Elements.Add(node);
        foreach(var child in root.From){
            var c = FillGraph(child,graph,visited);
            if(c!=null){            
                graph.AddEdge(node,c);
            }
        }
        return node;

    }
}
