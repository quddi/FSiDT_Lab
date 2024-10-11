using ScottPlot.Plottables;

namespace FSiDT_Lab;

public class PlotsContext
{
    public List<Ellipse> TwoSignsPlotPoints { get; set; } = new();

    public List<Ellipse> TwoSignsPlotClustersCenters { get; set; } = new();

    public void SetPointsRadius(double radius)
    {
        foreach (var point in TwoSignsPlotPoints)
        {
            point.RadiusX = radius;
            point.RadiusY = radius;
        }
    }

    public void SetClustersCenters(double radius, float lineWidth)
    {
        foreach (var center in TwoSignsPlotClustersCenters)
        {
            center.RadiusX = radius;
            center.RadiusY = radius;
            center.LineWidth = lineWidth;
        }
    }
}
