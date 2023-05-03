public class Value{
    public double Data{get;set;} = 0;
    public double Grad{get;set;} = 0;
    public HashSet<Value> From {get;set;} = new HashSet<Value>();
    public String Opeartor{get;set;} = "";
    private Action _backward{get;set;} = ()=>{};
    public string Label{get;set;} = "";
    public Value(double data,string label=""){
        this.Data = data;
        this.Label = label;
    }

    public static Value operator +(Value a, Value b){
        var ret = new Value(0);
        ret.From.Add(a);
        ret.From.Add(b);
        ret.Opeartor = "+";
        ret.Data = a.Data+b.Data;
        ret._backward = ()=>{
            a.Grad += ret.Grad;
            b.Grad += ret.Grad;
        };
        return ret;
    }
    //unary negation
    public static Value operator -(Value a){
        return a*(-1);
    }

    //subtraction

     public static Value operator -(Value a,Value b){
        return a+(-b);
    }

    public static Value operator *(Value a, Value b){
        var ret = new Value(0);
        ret.From.Add(a);
        ret.From.Add(b);
        ret.Opeartor = "*";
        ret.Data = a.Data*b.Data;
        ret._backward = ()=>{
            a.Grad += b.Data * ret.Grad;
            b.Grad += a.Data * ret.Grad;
        };
        return ret;
    }

    public static Value Exp(Value a){
        var ret = new Value(Math.Exp(a.Data));
        ret.From.Add(a);
        ret.Opeartor = "e^x";
        ret._backward = () =>{
            a.Grad += Math.Exp(a.Data) * ret.Grad;
        };
        return ret;
    }

    public Value tanh(){
        return new Value(0);
    }

    public static Value Pow (Value a,double by){
        var ret = new Value(Math.Pow(a.Data,by));
        ret.From.Add(a);
        ret._backward = () =>{
            a.Grad += Math.Pow(a.Data,by-1) * ret.Grad;
        };
        ret.Opeartor = "^";
        return ret;
    }

    public static Value operator /(Value a, Value b){
        var ret = a * Pow(b,-1);
        ret.Opeartor = "/";
        return ret;
    }

    public static void TopoSort(Value root,List<Value> visited, List<Value> topo){
        if(!visited.Contains(root)){
            visited.Add(root);
        }
        foreach(var child in root.From){
            TopoSort(child,visited,topo);
        }
        topo.Add(root);
    }

    public static implicit operator Value(double value)
    {
        return new Value(value);
    }

    public void Backward(){
        List<Value> visited = new List<Value>();
        List<Value> topo = new List<Value>();
        TopoSort(this,visited,topo);
        this.Grad = 1.0;
        foreach(var val in topo.Reverse<Value>()){
            val._backward();
        }
    }

    public override string ToString()
    {
        return $"{this.Label}:{this.Data}:[{this.Grad}]" + (this.Opeartor!=""?$"- op: {this.Opeartor}":"") + (this.From.Count>0? "from - " : "");
    }
}