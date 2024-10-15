using System.Windows.Controls;

namespace FSiDT_Lab;

public class Context
{
    private ComboBox _firstSignComboBox;
    private ComboBox _secondSignComboBox;

    public List<DataRow>? CurrentData { get; set; }
    public List<ClusterData>? ClustersDatas { get; set; }
    public List<ClustersCountEvaluationData>? ClustersCountsEvaluation { get; set; }

    public int FirstSignIndex { get; set; }
    public int SecondSignIndex { get; set; }
    public int? ClustersCount { get; set; }

    public List<string> FirstSignComboBoxItemsSource { get; set; } = new();
    public List<string> SecondSignComboBoxItemsSource { get; set; } = new();

    public PlotsContext PlotsContext { get; set; } = new();

    public string FirstSelectedLabel => FirstSignComboBoxItemsSource[_firstSignComboBox.SelectedIndex];
    public string SecondSelectedLabel => SecondSignComboBoxItemsSource[_secondSignComboBox.SelectedIndex];

    public int FirstSelectedIndex => SignLabel.ToIndex(FirstSelectedLabel)!.Value;
    public int SecondSelectedIndex => SignLabel.ToIndex(SecondSelectedLabel)!.Value;

    public bool AnySignComboBoxInEmpty => _firstSignComboBox.SelectedIndex == Constants.NoElementComboBoxIndex ||
            _secondSignComboBox.SelectedIndex == Constants.NoElementComboBoxIndex;

    public bool SignComboBoxesHaveSameValue => _firstSignComboBox.SelectedIndex == _secondSignComboBox.SelectedIndex;

    public bool SignComboBoxesOk => !AnySignComboBoxInEmpty && !SignComboBoxesHaveSameValue;

    public bool IsClusterized => ClustersDatas != null && CurrentData!.All(row => row.ClusterIndex != null);

    public int? Dimensions => CurrentData?.FirstOrDefault()?.Coordinates?.Values.Count;

    public Context(ComboBox firstSignComboBox, ComboBox secondSignComboBox)
    {
        _firstSignComboBox = firstSignComboBox;
        _secondSignComboBox = secondSignComboBox;
    }
}
