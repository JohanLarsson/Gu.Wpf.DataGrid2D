﻿namespace Gu.Wpf.DataGrid2D
{
    using System.Windows;
    using System.Windows.Controls;

    public static partial class ItemsSource
    {
        public static readonly DependencyProperty CellEditingTemplateProperty = DependencyProperty.RegisterAttached(
            "CellEditingTemplate",
            typeof(DataTemplate),
            typeof(ItemsSource),
            new PropertyMetadata(null, OnCellEditingTemplateChanged),
            CellEditingTemplateValidateValue);

        public static void SetCellEditingTemplate(this DataGrid element, DataTemplate value)
        {
            element.SetValue(CellEditingTemplateProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(DataGrid))]
        public static DataTemplate GetCellEditingTemplate(this DataGrid element)
        {
            return (DataTemplate)element.GetValue(CellEditingTemplateProperty);
        }

        private static void OnCellEditingTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            var celltemplate = (DataTemplate)e.NewValue;

            dataGrid.SetCellEditingTemplate(celltemplate);
            dataGrid.RaiseEvent(new RoutedEventArgs(Events.ColumnsChanged));
        }

        private static bool CellEditingTemplateValidateValue(object value)
        {
            return true;
        }
    }
}
