namespace FSiDT_Lab;

public class ClustersCountEvaluationData
{
    public int ClustersCount { get; set; }

    public double Value { get; set; }

    public string ValueString => Value.ToString("#.##");

    public bool Result { get; set; }
}
