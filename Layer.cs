public class Layer{
    public int NumNeurons{get;set;}
    public int NumInputs{get;set;}
    List<Neuron> Neurons{get;set;}
    public Layer(int numNuerons,int numInputs){
        this.NumNeurons = numNuerons;
        this.NumInputs = numInputs;

        Neurons = new List<Neuron>();
        for(int i=0;i<numNuerons;i++){
            Neurons.Add(new Neuron(numInputs));
        }

    }
}