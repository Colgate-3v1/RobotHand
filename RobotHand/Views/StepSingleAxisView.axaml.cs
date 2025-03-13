using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using RobotHand.ViewModels;
using System.Collections.Generic;

namespace RobotHand.Views;

public partial class StepSingleAxisView : UserControl
{
    private Button j1;

    public StepSingleAxisView()
    {
        InitializeComponent();
        var c = GetAllButtons();
        for (int i = 0; i < c.Count; i++)
        {
            c[i].AddHandler(Button.PointerPressedEvent, PointerPressed, handledEventsToo: true);
            c[i].AddHandler(Button.PointerReleasedEvent, PointerReleased, handledEventsToo: true);
            //c[i].PointerPressed += PointerPressed;
            //c[i].PointerReleased += PointerReleased;
        }
    }

    private void PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (DataContext is StepSingleAxisViewModel vm)
        {
            vm.StopMove();
        }
    }

    public List<Button> GetAllButtons()
    {
        var buttons = new List<Button>();
        GetAllButtonsRecursive(this, buttons);
        return buttons;
    }

    private void GetAllButtonsRecursive(ILogical obj, List<Button> buttons)
    {
        if (obj is Button button)
        {
            if (button.Content.ToString() == "+" || button.Content.ToString() == "-")
                buttons.Add(button);
        }

        foreach (var child in obj.GetLogicalChildren())
        {
            GetAllButtonsRecursive(child, buttons);
        }
    }

    private void PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var button = (Button)sender;
        if (DataContext is StepSingleAxisViewModel vm)
        {
            if (button.Content.ToString() == "+")
                vm.PositiveStepCurrentJoint(button.CommandParameter.ToString());
            else
                vm.NegativeStepCurrentJoint(button.CommandParameter.ToString());
        }
    }
}