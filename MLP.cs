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

    public List<Value> Call(List<Value> x) {
        List<Value> ret = x;
        foreach (var layer in Layers) {
            ret = layer.Call(ret);
        }
        return ret;
    }

    public override string ToString() {
        var ret = new System.Text.StringBuilder("MLP\n---\n");
        foreach (var l in Layers) {
            ret.Append(l.ToString() + "\n");
        }
        return ret.ToString();
    }
}

