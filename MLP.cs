public class MLP {
    int NumInputs { get; set; }
    List<int> NumOuts { get; set; }
    List<Layer> Layers { get; set; }
    public MLP(int numInputs, List<int> numOuts) {
        this.NumInputs = numInputs;
        this.NumOuts = numOuts;
        this.Layers = new List<Layer>();
        var sz = NumOuts.Prepend(numInputs).ToArray();
        for (int i = 0; i < NumOuts.Count; i++) {
            Layers.Add(new Layer(sz[i], sz[i + 1]));
        }
    }

    public List<Value> Parameters {
        get {
            List<Value> list = new List<Value>();
            foreach (var n in Layers) {
                list.AddRange(n.Parameters);
            }
            return list;
        }
    }

    public List<Value> Call(List<Value> x) {
        foreach (var layer in Layers) {
            x = layer.Call(x);
        }
        return x;
    }

    public override string ToString() {
        var ret = new System.Text.StringBuilder("MLP\n---\n");
        foreach (var l in Layers) {
            ret.Append(l.ToString() + "\n");
        }
        return ret.ToString();
    }
}

