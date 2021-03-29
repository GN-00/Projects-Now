using System.Windows;
using Core.Data;
using System.Windows.Controls;
using System.Collections.Generic;
using Core.Data.QuotationsData;
using System.Windows.Media;

namespace Core.Print.QuotationPages
{
    public partial class PanelsInformation : UserControl
    {
        string LastArticle1;
        string LastArticle2;
        Border LastArticle1Border;
        Border LastArticle2Border;

        static readonly double cm = Constants.Cm;
        public  Data.QuotationsData.Panel PanelData { get; set; }

        List<Item> ItemsData;
        public PanelsInformation()
        {
            InitializeComponent();
        }

        public PanelsInformation(Data.QuotationsData.Panel panel)
        {
            InitializeComponent();
            PanelData = panel;
            DataContext = PanelData;
        }

        public Item AddItem
        {
            get { return (Item)GetValue(AddItemProperty); }
            set { SetValue(AddItemProperty, value); }
        }

        public static readonly DependencyProperty AddItemProperty =
            DependencyProperty.Register("AddItem", typeof(Item), typeof(PanelsInformation), new PropertyMetadata(null, SetAddItemValue));

        private static void SetAddItemValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Border border;
            Viewbox viewbox;
            TextBlock textBlock;
            var table = d as PanelsInformation;
            if (table.ItemsData == null)
                table.ItemsData = new List<Item>();

            var item = e.NewValue as Item;
            table.ItemsData.Add(item);
            table.Details.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0.6 * cm) });

            textBlock = new TextBlock()
            {
                Text = item.Article1,
                FontSize = 14,
                Foreground = Brushes.White,
                FontFamily = new FontFamily("Calibri (Body)"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            border = new Border()
            {
                Background = (Brush)(new BrushConverter().ConvertFromString("#4f81bd")),
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1, 0, 1, 1),
                Child = textBlock,

            };
            System.Windows.Controls.Panel.SetZIndex(border, 1000);
            
            if (table.LastArticle1 == null)
            {
                table.LastArticle1 = item.Article1;
                table.LastArticle1Border = border;

                table.Details.Children.Add(border);
                Grid.SetRow(border, table.Details.RowDefinitions.Count - 1);
                Grid.SetColumn(border, 0);

                if (string.IsNullOrWhiteSpace(item.Article2))
                    Grid.SetColumnSpan(border, 2);

                textBlock = new TextBlock()
                {
                    Text = item.Article2,
                    FontSize = 12,
                    FontFamily = new FontFamily("Calibri (Body)"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                border = new Border()
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0, 0, 1, 1),
                    Child = textBlock,
                };
                System.Windows.Controls.Panel.SetZIndex(border, 0);
                table.Details.Children.Add(border);
                Grid.SetRow(border, table.Details.RowDefinitions.Count - 1);
                Grid.SetColumn(border, 1);

                table.LastArticle2 = item.Article2;
                table.LastArticle2Border = border;

            }
            else
            {
                if (item.Article1 == table.LastArticle1)
                {
                    Grid.SetRowSpan(table.LastArticle1Border, Grid.GetRowSpan(table.LastArticle1Border) + 1);

                    if (Grid.GetColumnSpan(table.LastArticle1Border) == 2 && !string.IsNullOrWhiteSpace(item.Article2))
                        Grid.SetColumnSpan(table.LastArticle1Border, 1);


                    textBlock = new TextBlock()
                    {
                        Text = item.Article2,
                        FontSize = 12,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                    };
                    border = new Border()
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(0, 0, 1, 1),
                        Child = textBlock,
                    };

                    if (table.LastArticle2 == item.Article2)
                    {
                        Grid.SetRowSpan(table.LastArticle2Border, Grid.GetRowSpan(table.LastArticle2Border) + 1);
                    }
                    else
                    {
                        table.Details.Children.Add(border);
                        Grid.SetRow(border, table.Details.RowDefinitions.Count - 1);
                        Grid.SetColumn(border, 1);

                        table.LastArticle2 = item.Article2;
                        table.LastArticle2Border = border;
                    }

                }
                else
                {
                    table.LastArticle1 = item.Article1;
                    table.LastArticle1Border = border;

                    table.Details.Children.Add(border);
                    Grid.SetRow(border, table.Details.RowDefinitions.Count - 1);
                    Grid.SetColumn(border, 0);
                    if (string.IsNullOrWhiteSpace(item.Article2))
                        Grid.SetColumnSpan(border, 2);

                    textBlock = new TextBlock()
                    {
                        Text = item.Article2,
                        FontSize = 12,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                    };
                    border = new Border()
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(0, 0, 1, 1),
                        Child = textBlock,
                    };
                    System.Windows.Controls.Panel.SetZIndex(border, 0);

                    table.Details.Children.Add(border);
                    Grid.SetRow(border, table.Details.RowDefinitions.Count - 1);
                    Grid.SetColumn(border, 1);

                    table.LastArticle2 = item.Article2;
                    table.LastArticle2Border = border;
                }

            }

            textBlock = new TextBlock()
            {
                Text = item.Description,
                FontSize = 14,
                FontFamily = new FontFamily("Calibri (Body)"),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5, 0, 5, 0)
            };
            border = new Border()
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(0, 0, 1, 1),
                Child = textBlock,
            };
            table.Details.Children.Add(border);
            Grid.SetRow(border, table.Details.RowDefinitions.Count - 1);
            Grid.SetColumn(border, 2);

            if (item.Unit == "Lot")
            {
                textBlock = new TextBlock()
                {
                    Text = item.Unit,
                    FontSize = 14,
                    FontWeight = FontWeights.Bold,
                    Foreground = (Brush)(new BrushConverter().ConvertFromString("#4f81bd")),
                    FontFamily = new FontFamily("Calibri (Body)"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };
            }
            else
            {
                textBlock = new TextBlock()
                {
                    Text = item.Qty.ToString(),
                    FontSize = 14,
                    FontWeight = FontWeights.Bold,
                    Foreground = (Brush)(new BrushConverter().ConvertFromString("#4f81bd")),
                    FontFamily = new FontFamily("Calibri (Body)"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };
            }
            border = new Border()
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(0, 0, 1, 1),
                Child = textBlock,
            };
            table.Details.Children.Add(border);
            Grid.SetRow(border, table.Details.RowDefinitions.Count - 1);
            Grid.SetColumn(border, 3);


            textBlock = new TextBlock()
            {
                Text = item.Brand,
                FontSize = 14,
                FontFamily = new FontFamily("Calibri (Body)"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            viewbox = new Viewbox() { Child = textBlock, Margin = new Thickness(5, 0, 5, 0) };
            border = new Border()
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(0, 0, 1, 1),
                Child = viewbox,
            };
            table.Details.Children.Add(border);
            Grid.SetRow(border, table.Details.RowDefinitions.Count - 1);
            Grid.SetColumn(border, 4);

            textBlock = new TextBlock()
            {
                Text = item.Remarks,
                FontSize = 14,
                FontFamily = new FontFamily("Calibri (Body)"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            border = new Border()
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(0, 0, 1, 1),
                Child = textBlock,
            };
            table.Details.Children.Add(border);
            Grid.SetRow(border, table.Details.RowDefinitions.Count - 1);
            Grid.SetColumn(border, 5);

        }
    }
}
