namespace dn_mnist;
using DotNetGraph;
using DotNetGraph.Extensions;
using DotNetGraph.Node;
class Program {
    static void Main(string[] args) {

        //BasicTest2();
        //NeuronTest();
        //MLPTest();
        MLPIrisTest();

    }

    static MLP MLPTest() {
        MLP m = new MLP(3, new List<int> { 4, 4, 1 });
        Console.Write(m);
        var xs = new List<List<Value>>{
            new List<Value>{2.0,3.0,-1.0},
            new List<Value>{3.0,-1.0,0.5},
            new List<Value>{0.5,1.0,1.0},
            new List<Value>{1.0,1.0,-1.0},
        };
        var ys = new Value[] { 1.0, -1.0, -1.0, 1.0 };

        Value loss = 0;
        //Training Loop
        for (int epoch = 0; epoch < 20; epoch++) {
            loss = 0;

            //Forward pass and calculate loss
            var yPred = new List<Value>();
            foreach (var row in xs) {
                yPred.Add(m.Call(row)[0]);
            }

            var losses = new List<Value>();

            for (int i = 0; i < yPred.Count; i++) {
                losses.Add(Value.Pow((yPred[i] - ys[i]), 2));
            }

            foreach (var l in losses) {
                loss += l;
            }
            //zero grad before backward
            foreach (var param in m.Parameters) {
                param.Grad = 0;
            }

            loss.Backward();


            //Update parameters
            foreach (var param in m.Parameters) {
                param.Data -= 0.05 * param.Grad; // we are using negative to reduce the loss
            }

            Console.WriteLine($"Loss: {loss.Data}");
        }
        //DrawGraph(loss);
        //DrawNetwork(m);
        Console.WriteLine($"Parameters: {m.Parameters.Count}");
        return m;
    }
    static double ToCategory(string category) {
        if (category == "Iris-setosa") {
            return -1;
        }
        else if (category == "Iris-versicolor") {
            return 0;
        }
        else if (category == "Iris-virginica") {
            return 1;
        }
        else {
            throw new Exception("Invalid category");
        }
    }
    static MLP MLPIrisTest() {
        MLP m = new MLP(4, new List<int> { 5,5, 1 });

        var data = System.IO.File.ReadAllLines("data/iris.data");
        var xs = new List<List<Value>>();

        var ys = new List<Value>();

        foreach (string line in data) {
            var lst = new List<Value>();
            var items = line.Split(",");
            if(items.Length==5){
                lst.Add(Double.Parse(items[0].Trim()));
                lst.Add(Double.Parse(items[1].Trim()));
                lst.Add(Double.Parse(items[2].Trim()));
                lst.Add(Double.Parse(items[3].Trim()));
                xs.Add(lst);
                ys.Add(ToCategory(items[4]));
            }
        }

        Value loss = 0;
        //Training Loop
        for (int epoch = 0; epoch < 200; epoch++) {
            loss = 0;

            //Forward pass and calculate loss
            var yPred = new List<Value>();
            foreach (var row in xs) {
                yPred.Add(m.Call(row)[0]);
            }

            var losses = new List<Value>();

            for (int i = 0; i < yPred.Count; i++) {
                losses.Add(Value.Pow((yPred[i] - ys[i]), 2));
            }
            // PrintValueArr(losses);
            foreach (var l in losses) {
                loss += l;
            }

            loss = loss / xs.Count;
            //zero grad before backward
            foreach (var param in m.Parameters) {
                param.Grad = 0;
            }

            loss.Backward();


            //Update parameters
            foreach (var param in m.Parameters) {
                param.Data -= .05 * param.Grad; // we are using negative to reduce the loss
            }

            Console.WriteLine($"Loss: {loss.Data}");
        }
        Console.WriteLine($"Parameters: {m.Parameters.Count}");
        return m;
    }

    static void BasicTest1() {
        Value x = new Value(10, "x");
        Value y = new Value(20, "y");
        Value z = 2 * Value.Pow(x, 2) + 3 * y; z.Label = "z";

        z.Backward();

        DrawGraph(z);
    }
    static void BasicTest2() {
        Value x = new Value(10, "x");
        Value y = new Value(20, "y");
        Value z = 2 * x + 3 * y; z.Label = "z";

        z.Backward();

        DrawGraph(z);
    }
    //Matching what karpahy had in the video
    static void NeuronTest() {
        //inputs
        var x1 = new Value(2.0) { Label = "x1" };
        var x2 = new Value(0.0) { Label = "x2" };
        //weights
        var w1 = new Value(-3.0) { Label = "w1" };
        var w2 = new Value(1.0) { Label = "w2" };
        //bias
        var b = new Value(6.8813753870195432) { Label = "b" };

        var x1w1 = x1 * w1; x1w1.Label = "x1w1";
        var x2w2 = x2 * w2; x2w2.Label = "x2w2";
        var x1w1x2w2 = x1w1 + x2w2; x1w1x2w2.Label = "x1w1 + x2w2";
        var n = x1w1x2w2 + b; n.Label = "n";

        // var e = Value.Exp(2*n);
        // var o = (e - 1) / (e + 1);
        var o = Value.Tanh(n); o.Label = "o";
        o.Backward();
        // o.Grad = 1;
        // o._backward();
        // n._backward();
        DrawGraph(o);
    }

    static void DFS(Value root) {
        Console.Write(root);
        foreach (var child in root.From) {
            Console.Write("(");
            DFS(child);
            Console.Write(")");
        }
    }

    static void PrintValueArr(List<Value> arr){
        foreach(var item in arr){
            Console.Write($"{item.Data}, ");
        }
        Console.WriteLine();
    }

    static void DrawGraph(Value root) {
        var graph = new DotGraph("Micrograd", true);
        List<Value> visited = new List<Value>();
        FillGraph(root, graph, visited);
        var dot = graph.Compile(true);
        File.WriteAllText("graph.dot", dot);
    }

    static DotNode? FillGraph(Value root, DotGraph graph, List<Value> visited) {
        if (visited.Contains(root)) {
            return null;
        }
        visited.Add(root);
        var node = new DotNode(Guid.NewGuid().ToString());
        node.SetCustomAttribute("shape", "record");
        node.Style = DotNodeStyle.Filled;
        if (root.Operator == "tanh" || root.Operator == "relu") {
            node.Color = (System.Drawing.Color.Red);
            node.FillColor = System.Drawing.Color.LightGoldenrodYellow;
        }
        if (root.From.Count == 0) {//leaf
            node.Color = (System.Drawing.Color.Green);
            node.FillColor = System.Drawing.Color.LightGreen;
        }
        node.Label = $"{root.Label}|{root.Data.ToString("F4")}|{root.Grad.ToString("F4")}|{root.Operator}";
        graph.Elements.Add(node);
        foreach (var child in root.From) {
            var c = FillGraph(child, graph, visited);
            if (c != null) {
                graph.AddEdge(c, node);
            }
        }
        return node;

    }

    static void DrawNetwork(MLP mlp) {
        var graph = new DotGraph("Network", true);
        graph.Elements.Add(new DotString("rankdir=LR;"));
        List<DotNode> prev = new List<DotNode>();
        List<DotNode> current = new List<DotNode>();

        for (int i = 0; i <= mlp.NumOuts.Count; i++) {
            if (i == 0) {
                //draw the first layer
                for (int n = 0; n < mlp.NumInputs; n++) {
                    var id = Guid.NewGuid().ToString();
                    var node = new DotNode(id);
                    node.Label = "i";
                    node.Style = DotNodeStyle.Filled;
                    node.FillColor = System.Drawing.Color.LightGoldenrodYellow;
                    graph.Elements.Add(node);
                    prev.Add(node);
                }
            }
            else {
                Layer l = mlp.Layers[i - 1];
                foreach (var neuron in l.Neurons) {
                    var id = Guid.NewGuid().ToString();
                    var node = new DotNode(id);
                    node.Label = i == mlp.NumOuts.Count ? "o" : "H";
                    node.Style = DotNodeStyle.Filled;
                    node.FillColor = i == mlp.NumOuts.Count ? System.Drawing.Color.LightSalmon : System.Drawing.Color.LightCyan;
                    current.Add(node);
                    graph.Elements.Add(node);
                    foreach (var pnode in prev) {
                        graph.AddEdge(pnode, node);
                    }
                }
                prev.Clear();
                prev.AddRange(current);
                current.Clear();
            }
        }
        var dot = graph.Compile(true);
        File.WriteAllText("network.dot", dot);
    }
}
