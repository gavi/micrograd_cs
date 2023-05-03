namespace dn_mnist;
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

        e.Backward();


        Value x = e/10;
        x.Label = "x";
        Value y = x + 15;
        y.Label = "y";


        add_test1();
        topo_test(y);
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
}
