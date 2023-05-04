public class MLP{
    List<int> LayerSizes {get;set;}
    List<Layer> Layers{get;set;}
    public MLP(List<int> layerSizes){
        this.LayerSizes = layerSizes;
        this.Layers = new List<Layer>();
        for (int i=0;i<LayerSizes.Count-1;i++){
            this.Layers.Add(new Layer(layerSizes[i],layerSizes[i+1]));
            
        }
    }   

    public List<Value> Call(List<Value> x){
        List<Value> ret = x;
        foreach(var layer in Layers){
            ret = layer.Call(ret);
        }
        return ret;
    }

    public override string ToString()
    {   
        var ret = new System.Text.StringBuilder("MLP\n---\n");
        foreach(var l in Layers){
            ret.Append(l.ToString()+"\n");
        }
        return ret.ToString();
    }
}

