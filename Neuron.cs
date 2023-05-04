public class Neuron{
    public int NumInputs {get;set;}
    public List<Value> Weights{get;set;}
    public Value Bias{get;set;}

    public Neuron(int numInputs){
        NumInputs = numInputs;
        Weights = new List<Value>();
        for(int i=0;i<numInputs;i++){
            var weight = new Value(Random.Shared.NextDouble() * 2 -1);
            Weights.Add(weight);
        }
        Bias = Random.Shared.NextDouble() * 2 -1; //To get a number between -1 and 1
    }

    public Value Call(List<Value>x){
        Value sum = 0;
        var i = 0;
        foreach(var weight in Weights){
             sum += weight * x[i];
             i= i+1;
        }
        sum += Bias;
        var ret = sum.Tanh();//squishing function
        return ret;
    }

    public override string ToString()
    {
        return $"Neuron: {NumInputs}";
    }
}