public class Layer {
    public int NumOutputs { get; set; }
    public int NumInputs { get; set; }
    List<Neuron> Neurons { get; set; }
    public Layer(int numInputs, int numOutputs) {
        this.NumInputs = numInputs;
        this.NumOutputs = numOutputs;

        Neurons = new List<Neuron>();
        for (int i = 0; i < numOutputs; i++) {
            Neurons.Add(new Neuron(numInputs));
        }

    }

    public List<Value> Call(List<Value> x) {
        List<Value> ret = new List<Value>();
        foreach (var neuron in Neurons) {
            ret.Add(neuron.Call(x));
        }
        return ret;
    }

    public override string ToString() {
        var ret = new System.Text.StringBuilder($"Layer{NumInputs}-{NumOutputs}\n---\n");
        foreach (var n in Neurons) {
            ret.Append(n.ToString() + "\n");
        }
        return ret.ToString();
    }
}