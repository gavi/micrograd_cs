public class MLP{
    int NumLayers {get;set;}
    List<(int,int)> LayerConfig{get;set;}

    List<Layer> Layers{get;set;}
    public MLP(int numLayers, List<(int,int)> layerConfig){
        this.LayerConfig = new List<(int, int)>();
        this.LayerConfig = layerConfig;
        this.Layers = new List<Layer>();
        for (int i=0;i<numLayers;i++){
            this.Layers.Add(new Layer(layerConfig[i].Item1,layerConfig[i].Item2));
        }
    }   
}

