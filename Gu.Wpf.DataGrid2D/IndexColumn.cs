﻿namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class IndexColumn : DataGridTemplateColumn
    {
        private static readonly Dictionary<int, PropertyPath> IndexPaths = new Dictionary<int, PropertyPath>();
        private static readonly Dictionary<DependencyProperty, PropertyPath> PropertyPaths = new Dictionary<DependencyProperty, PropertyPath>();

        public IndexColumn(DataGrid dataGrid, int index)
        {
            Index = index;
            HeaderStringFormat = dataGrid.GetHeaderStringFormat();
            HeaderTemplate = dataGrid.GetHeaderTemplate();
            HeaderTemplateSelector = dataGrid.GetHeaderTemplateSelector();

            Width = dataGrid.GetWidth();
            MinWidth = dataGrid.GetMinWidth();
            MaxWidth = dataGrid.GetMaxWidth();
            //Bind(this, CellTemplateProperty, dataGrid, GetPath(CellTemplateProperty));
            //Bind(this, CellTemplateSelectorProperty, dataGrid, GetPath(CellTemplateSelectorProperty));
            //Bind(this, CellEditingTemplateProperty, dataGrid, GetPath(CellEditingTemplateProperty));
            //Bind(this, CellEditingTemplateSelectorProperty, dataGrid, GetPath(CellEditingTemplateSelectorProperty));
            CellTemplate = dataGrid.GetCellTemplate();
            CellTemplateSelector = dataGrid.GetCellTemplateSelector();
            CellEditingTemplate = dataGrid.GetCellEditingTemplate();
            CellEditingTemplateSelector = dataGrid.GetCellEditingTemplateSelector();
        }

        public IndexColumn(DataGrid dataGrid, object[] headers, int index)
            : this(dataGrid, index)
        {
            BindHeader(headers, index);
        }

        public void BindHeader(object[] headers, int index)
        {
            Bind(this, HeaderProperty, headers, GetPath(index));            
        }

        public int Index { get; private set; }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            var array = dataItem as Array;
            if (array != null)
            {
                Bind(cell, FrameworkElement.DataContextProperty, dataItem, GetPath(Index));
            }
            else
            {
                cell.DataContext = dataItem;
            }
            var frameworkElement = base.GenerateElement(cell, dataItem);
            if (frameworkElement == null)
            {
                ContentPresenter contentPresenter = new ContentPresenter
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                BindingOperations.SetBinding(contentPresenter, ContentPresenter.ContentProperty, new Binding());
                return contentPresenter;
            }
            return frameworkElement;
        }

        private static void Bind(DependencyObject target, DependencyProperty targetProperty, object source, PropertyPath path)
        {
            var binding = new Binding
            {
                Path = path,
                Source = source,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(target, targetProperty, binding);
        }

        private static PropertyPath GetPath(int index)
        {
            PropertyPath path;
            if (!IndexPaths.TryGetValue(index, out path))
            {
                path = new PropertyPath(string.Format("[{0}]", index));
                IndexPaths[index] = path;
            }
            return path;
        }

        private static PropertyPath GetPath(DependencyProperty property)
        {
            PropertyPath path;
            if (!PropertyPaths.TryGetValue(property, out path))
            {
                path = new PropertyPath(string.Format("({0}.{1})", typeof(Source2D).Name, property.Name));
                path = new PropertyPath(property);
                PropertyPaths[property] = path;
            }
            return path;
        }
    }
}