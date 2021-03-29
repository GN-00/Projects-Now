using Core.Data;
using Core.Data.QuotationsData;
using Core.Data.UserData;
using Core.Enums;
using Core.Print.QuotationPages;

using Dapper;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Core.Windows.QuotationsWindows
{
    public partial class PrintWindow : Window
    {
        public User UserData { get; set; }
        //public string PanelIDs { get; set; }
        public Quotation QuotationData { get; set; }
        public Option QuotationOptionData { get; set; }

        bool showVAT;
        List<Term> Terms;
        List<Data.QuotationsData.Panel> Panels;
        List<Data.QuotationsData.Item> Items;
        List<FrameworkElement> PanelsBillPages;
        List<FrameworkElement> PanelsDetailsPages;
        readonly static double cm = Constants.Cm;
        readonly static double pageBodyHight = 760;

        public PrintWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string query;
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                query = "Select * From Quotation._Panels ";
                if (QuotationOptionData == null)
                    query += $"Where QuotationId  = {QuotationData.Id} Order By SN ";
                else
                    query = $"Where Id In (Select PanelId From Quotation._OptionsPanels Where OptionId = {QuotationOptionData.Id}) Order By SN";

                Panels = connection.Query<Data.QuotationsData.Panel>(query).ToList();
            }
            DataContext = QuotationData;
        }

        private void Cover_Click(object sender, RoutedEventArgs e)
        {
            CoverCheck.IsChecked = !CoverCheck.IsChecked;

            if (CoverCheck.IsChecked.Equals(true)) CoverPages.Text = "1";
            else CoverPages.Text = "0";
        }
        private void Terms_Click(object sender, RoutedEventArgs e)
        {
            TermsCheck.IsChecked = !TermsCheck.IsChecked;

            if (Terms == null)
            {
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    string query = $"Select * From [Quotation].[_Terms&Conditions] Where IsUsed = 'True' And QuotationId = {QuotationData.Id} Order By Sort ";
                    Terms = connection.Query<Term>(query).ToList();
                }
            }

            if (TermsCheck.IsChecked == true && Terms.Count != 0)
            {
                int topicNumber = 0;
                double totalHeight = 0;
                Line line;
                TextBlock textBlock;
                StackPanel stackPanel;

                var ScopeOfSupply = Terms.Where<Term>(item => item.ConditionType == TermTypes.ScopeOfSupply.ToString());
                var TotalPrice = Terms.Where<Term>(item => item.ConditionType == TermTypes.TotalPrice.ToString());
                var PaymentConditions = Terms.Where<Term>(item => item.ConditionType == TermTypes.PaymentConditions.ToString());
                var ValidityPeriod = Terms.Where<Term>(item => item.ConditionType == TermTypes.ValidityPeriod.ToString());
                var ShopDrawingSubmittals = Terms.Where<Term>(item => item.ConditionType == TermTypes.ShopDrawingSubmittals.ToString());
                var Delivery = Terms.Where<Term>(item => item.ConditionType == TermTypes.Delivery.ToString());
                var Guarantee = Terms.Where<Term>(item => item.ConditionType == TermTypes.Guarantee.ToString());

                textBlock = new TextBlock()
                {
                    Text = "COMMERCIAL TERMS & CONDITIONS",
                    FontSize = 29.5,
                    FontWeight = FontWeights.Bold,
                    FontFamily = new FontFamily("Calibri (Body)"),
                    TextWrapping = TextWrapping.Wrap,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top
                };
                textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                textBlock.Arrange(new Rect(textBlock.DesiredSize));
                totalHeight += textBlock.ActualHeight;

                line = new Line()
                {
                    X1 = 0,
                    X2 = 670,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                };
                line.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                line.Arrange(new Rect(line.DesiredSize));
                totalHeight += line.ActualHeight;


                if (ScopeOfSupply.Count() != 0)
                {
                    textBlock = new TextBlock()
                    {
                        Text = $"{++topicNumber}. Scope Of Supply:",
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        TextWrapping = TextWrapping.Wrap,
                        TextDecorations = TextDecorations.Underline,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    textBlock.Arrange(new Rect(textBlock.DesiredSize));
                    totalHeight += textBlock.ActualHeight;

                    stackPanel = new StackPanel() { Height = 0.5 * cm };
                    totalHeight += stackPanel.Height;

                    foreach (Term termData in ScopeOfSupply)
                    {
                        stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                        textBlock = new TextBlock()
                        {
                            Text = $"» ",
                            FontSize = 18,
                            FontWeight = FontWeights.Bold,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);
                        textBlock = new TextBlock()
                        {
                            Text = termData.Condition,
                            FontSize = 18,
                            Width = 670,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);

                        stackPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        stackPanel.Arrange(new Rect(stackPanel.DesiredSize));
                        totalHeight += stackPanel.ActualHeight;
                    }
                }

                if (TotalPrice.Count() != 0)
                {
                    textBlock = new TextBlock()
                    {
                        Text = $"{++topicNumber}. Total Price:",
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        TextWrapping = TextWrapping.Wrap,
                        TextDecorations = TextDecorations.Underline,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    textBlock.Arrange(new Rect(textBlock.DesiredSize));
                    totalHeight += textBlock.ActualHeight;

                    stackPanel = new StackPanel() { Height = 0.5 * cm };
                    totalHeight += stackPanel.Height;

                    textBlock = new TextBlock()
                    {
                        Text = $"Our Prices are meant:",
                        FontSize = 18,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        TextWrapping = TextWrapping.Wrap,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    textBlock.Arrange(new Rect(textBlock.DesiredSize));
                    totalHeight += textBlock.ActualHeight;

                    foreach (Term termData in TotalPrice)
                    {
                        stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                        textBlock = new TextBlock()
                        {
                            Text = $"» ",
                            FontSize = 18,
                            FontWeight = FontWeights.Bold,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);
                        textBlock = new TextBlock()
                        {
                            Text = termData.Condition,
                            FontSize = 18,
                            Width = 670,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);

                        stackPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        stackPanel.Arrange(new Rect(stackPanel.DesiredSize));
                        totalHeight += stackPanel.ActualHeight;
                    }
                }

                if (PaymentConditions.Count() != 0)
                {
                    textBlock = new TextBlock()
                    {
                        Text = $"{++topicNumber}. Payment Conditions:",
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        TextWrapping = TextWrapping.Wrap,
                        TextDecorations = TextDecorations.Underline,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    textBlock.Arrange(new Rect(textBlock.DesiredSize));
                    totalHeight += textBlock.ActualHeight;

                    stackPanel = new StackPanel() { Height = 0.5 * cm };
                    totalHeight += stackPanel.Height;

                    foreach (Term termData in PaymentConditions)
                    {
                        stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                        textBlock = new TextBlock()
                        {
                            Text = $"» ",
                            FontSize = 18,
                            FontWeight = FontWeights.Bold,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);
                        textBlock = new TextBlock()
                        {
                            Text = termData.Condition,
                            FontSize = 18,
                            Width = 670,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);

                        stackPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        stackPanel.Arrange(new Rect(stackPanel.DesiredSize));
                        totalHeight += stackPanel.ActualHeight;
                    }
                }

                if (ValidityPeriod.Count() != 0)
                {
                    textBlock = new TextBlock()
                    {
                        Text = $"{++topicNumber}. Validity Period:",
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        TextWrapping = TextWrapping.Wrap,
                        TextDecorations = TextDecorations.Underline,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    textBlock.Arrange(new Rect(textBlock.DesiredSize));
                    totalHeight += textBlock.ActualHeight;

                    stackPanel = new StackPanel() { Height = 0.5 * cm };
                    totalHeight += stackPanel.Height;

                    foreach (Term termData in ValidityPeriod)
                    {
                        stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                        textBlock = new TextBlock()
                        {
                            Text = $"» ",
                            FontSize = 18,
                            FontWeight = FontWeights.Bold,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);
                        textBlock = new TextBlock()
                        {
                            Text = termData.Condition,
                            FontSize = 18,
                            Width = 670,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);

                        stackPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        stackPanel.Arrange(new Rect(stackPanel.DesiredSize));
                        totalHeight += stackPanel.ActualHeight;
                    }
                }

                if (ShopDrawingSubmittals.Count() != 0)
                {
                    textBlock = new TextBlock()
                    {
                        Text = $"{++topicNumber}. Shop Drawing Submittals:",
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        TextWrapping = TextWrapping.Wrap,
                        TextDecorations = TextDecorations.Underline,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    textBlock.Arrange(new Rect(textBlock.DesiredSize));
                    totalHeight += textBlock.ActualHeight;

                    stackPanel = new StackPanel() { Height = 0.5 * cm };
                    totalHeight += stackPanel.Height;

                    foreach (Term termData in ShopDrawingSubmittals)
                    {
                        stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                        textBlock = new TextBlock()
                        {
                            Text = $"» ",
                            FontSize = 18,
                            FontWeight = FontWeights.Bold,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);
                        textBlock = new TextBlock()
                        {
                            Text = termData.Condition,
                            FontSize = 18,
                            Width = 670,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);

                        stackPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        stackPanel.Arrange(new Rect(stackPanel.DesiredSize));
                        totalHeight += stackPanel.ActualHeight;
                    }
                }

                if (Delivery.Count() != 0)
                {
                    textBlock = new TextBlock()
                    {
                        Text = $"{++topicNumber}. Delivery:",
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        TextWrapping = TextWrapping.Wrap,
                        TextDecorations = TextDecorations.Underline,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    textBlock.Arrange(new Rect(textBlock.DesiredSize));
                    totalHeight += textBlock.ActualHeight;

                    stackPanel = new StackPanel() { Height = 0.5 * cm };
                    totalHeight += stackPanel.Height;

                    foreach (Term termData in Delivery)
                    {
                        stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                        textBlock = new TextBlock()
                        {
                            Text = $"» ",
                            FontSize = 18,
                            FontWeight = FontWeights.Bold,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);
                        textBlock = new TextBlock()
                        {
                            Text = termData.Condition,
                            FontSize = 18,
                            Width = 670,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);

                        stackPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        stackPanel.Arrange(new Rect(stackPanel.DesiredSize));
                        totalHeight += stackPanel.ActualHeight;
                    }
                }

                if (Guarantee.Count() != 0)
                {
                    textBlock = new TextBlock()
                    {
                        Text = $"{++topicNumber}. Guarantee:",
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        TextWrapping = TextWrapping.Wrap,
                        TextDecorations = TextDecorations.Underline,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    textBlock.Arrange(new Rect(textBlock.DesiredSize));
                    totalHeight += textBlock.ActualHeight;

                    stackPanel = new StackPanel() { Height = 0.5 * cm };
                    totalHeight += stackPanel.Height;

                    foreach (Term termData in Guarantee)
                    {
                        stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                        textBlock = new TextBlock()
                        {
                            Text = $"» ",
                            FontSize = 18,
                            FontWeight = FontWeights.Bold,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);
                        textBlock = new TextBlock()
                        {
                            Text = termData.Condition,
                            FontSize = 18,
                            Width = 670,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);

                        stackPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        stackPanel.Arrange(new Rect(stackPanel.DesiredSize));
                        totalHeight += stackPanel.ActualHeight;
                    }
                }

                TermsPages.Text = Math.Ceiling(totalHeight / pageBodyHight).ToString();
            }
            else
            {
                TermsPages.Text = "0";
            }
        }
        private void Bill_Click(object sender, RoutedEventArgs e)
        {
            QuotationPage page;
            BillCheck.IsChecked = !BillCheck.IsChecked;
            if (BillCheck.IsChecked == true && Panels.Count != 0)
            {
                PanelsBillPages = new List<FrameworkElement>();
                page = new QuotationPage(QuotationData.Code);
                page.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                page.Arrange(new Rect(page.DesiredSize));

                double totalHeight = 0;
                double rowWidth = 0;

                Line line;
                Border border;
                TextBlock textBlock;
                StackPanel stackPanel;

                textBlock = new TextBlock()
                {
                    Text = "BILL OF PRICES",
                    FontSize = 29.5,
                    Foreground = (Brush)(new BrushConverter().ConvertFromString("#4f81bd")),
                    FontWeight = FontWeights.Bold,
                    FontFamily = new FontFamily("Calibri (Body)"),
                    TextWrapping = TextWrapping.Wrap,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top
                };
                textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                textBlock.Arrange(new Rect(textBlock.DesiredSize));
                page.Body.Children.Add(textBlock);
                totalHeight += textBlock.ActualHeight;

                line = new Line()
                {
                    X1 = 0,
                    X2 = page.Body.ActualWidth,
                    Stroke = (Brush)(new BrushConverter().ConvertFromString("#4f81bd")),
                    StrokeThickness = 2,
                };
                line.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                line.Arrange(new Rect(line.DesiredSize));
                page.Body.Children.Add(line);
                totalHeight += line.ActualHeight;

                if (QuotationOptionData != null)
                {
                    textBlock = new TextBlock()
                    {
                        Text = $"Option {QuotationOptionData.Code}: {QuotationOptionData.Name}",
                        FontSize = 18,
                        Width = 670,
                        TextWrapping = TextWrapping.Wrap,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(0, 0.5 * cm, 0, 0),
                    };
                    page.Body.Children.Add(textBlock);
                }

                #region Header
                stackPanel = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0.5 * cm, 0, 0) };
                if (QuotationOptionData != null) stackPanel.Margin = new Thickness(0);
                //1
                border = new Border()
                {
                    Width = 1.2 * cm,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1, 1, 1, 1),
                    Background = (Brush)(new BrushConverter().ConvertFromString("#4f81bd")),
                };
                textBlock = new TextBlock()
                {
                    Text = "Item",
                    FontSize = 18,
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    FontFamily = new FontFamily("Calibri (Body)"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                border.Child = textBlock;
                stackPanel.Children.Add(border);
                //2
                border = new Border()
                {
                    Width = 8.54 * cm,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0, 1, 1, 1),
                    Background = (Brush)(new BrushConverter().ConvertFromString("#4f81bd")),
                };
                textBlock = new TextBlock()
                {
                    Text = "Panel Name",
                    FontSize = 18,
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    FontFamily = new FontFamily("Calibri (Body)"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                border.Child = textBlock;
                stackPanel.Children.Add(border);
                //3
                border = new Border()
                {
                    Width = 1.2 * cm,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0, 1, 1, 1),
                    Background = (Brush)(new BrushConverter().ConvertFromString("#4f81bd")),
                };
                textBlock = new TextBlock()
                {
                    Text = "QTY",
                    FontSize = 18,
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    FontFamily = new FontFamily("Calibri (Body)"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                border.Child = textBlock;
                stackPanel.Children.Add(border);
                //4
                border = new Border()
                {
                    Width = 3.5 * cm,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0, 1, 1, 1),
                    Background = (Brush)(new BrushConverter().ConvertFromString("#4f81bd")),
                };
                textBlock = new TextBlock()
                {
                    Text = "U/Price (SR)",
                    FontSize = 18,
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    FontFamily = new FontFamily("Calibri (Body)"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                border.Child = textBlock;
                stackPanel.Children.Add(border);
                //5
                border = new Border()
                {
                    Width = 3.5 * cm,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0, 1, 1, 1),
                    Background = (Brush)(new BrushConverter().ConvertFromString("#4f81bd")),
                };
                textBlock = new TextBlock()
                {
                    Text = "T/Price (SR)",
                    FontSize = 18,
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    FontFamily = new FontFamily("Calibri (Body)"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                border.Child = textBlock;
                stackPanel.Children.Add(border);
                stackPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                stackPanel.Arrange(new Rect(stackPanel.DesiredSize));

                totalHeight += stackPanel.ActualHeight;
                page.Body.Children.Add(stackPanel);
                #endregion

                #region Grid
                for (int i = 0; i < Panels.Count; i++)
                {
                    stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                    //1
                    border = new Border()
                    {
                        Width = 1.2 * cm,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1, 0, 1, 1),
                        Background = (Brush)(new BrushConverter().ConvertFromString("#4f81bd")),
                    };
                    if (i == 0) rowWidth += border.Width;

                    textBlock = new TextBlock()
                    {
                        Text = Panels[i].SN.ToString(),
                        FontSize = 18,
                        Foreground = Brushes.White,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    border.Child = textBlock;
                    stackPanel.Children.Add(border);
                    //2
                    border = new Border()
                    {
                        Width = 8.54 * cm,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(0, 0, 1, 1),
                    };
                    if (i == 0) rowWidth += border.Width;
                    textBlock = new TextBlock()
                    {
                        Text = Panels[i].Name,
                        FontSize = 18,
                        TextWrapping = TextWrapping.Wrap,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        Margin = new Thickness(5, 0, 5, 0),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    border.Child = textBlock;
                    stackPanel.Children.Add(border);
                    //3
                    border = new Border()
                    {
                        Width = 1.2 * cm,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(0, 0, 1, 1),
                    };
                    if (i == 0) rowWidth += border.Width;

                    textBlock = new TextBlock()
                    {
                        Text = Panels[i].Qty.ToString(),
                        FontSize = 18,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    border.Child = textBlock;
                    stackPanel.Children.Add(border);
                    //4
                    border = new Border()
                    {
                        Width = 3.5 * cm,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(0, 0, 1, 1),
                    };
                    if (i == 0) rowWidth += border.Width;

                    textBlock = new TextBlock()
                    {
                        Text = Panels[i].UnitPrice.ToString("N2"),
                        FontSize = 18,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    border.Child = textBlock;
                    stackPanel.Children.Add(border);
                    //5
                    border = new Border()
                    {
                        Width = 3.5 * cm,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(0, 0, 1, 1),
                    };

                    textBlock = new TextBlock()
                    {
                        Text = Panels[i].TotalPrice.ToString("N2"),
                        FontSize = 18,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    border.Child = textBlock;
                    stackPanel.Children.Add(border);
                    stackPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    stackPanel.Arrange(new Rect(stackPanel.DesiredSize));
                    totalHeight += stackPanel.ActualHeight;

                    if (totalHeight > (pageBodyHight - (0.6 * cm)))
                    {
                        PanelsBillPages.Add(page);
                        totalHeight = 0;
                        page = new QuotationPage(QuotationData.Code);
                        StackPanel subStackPanel = stackPanel;

                        stackPanel = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0.5 * cm, 0, 0) };
                        if (QuotationOptionData != null) stackPanel.Margin = new Thickness(0);
                        //1
                        border = new Border()
                        {
                            Width = 1.2 * cm,
                            BorderBrush = Brushes.Black,
                            BorderThickness = new Thickness(1, 1, 1, 1),
                            Background = (Brush)(new BrushConverter().ConvertFromString("#4f81bd")),
                        };
                        textBlock = new TextBlock()
                        {
                            Text = "Item",
                            FontSize = 18,
                            Foreground = Brushes.White,
                            FontWeight = FontWeights.Bold,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        border.Child = textBlock;
                        stackPanel.Children.Add(border);
                        //2
                        border = new Border()
                        {
                            Width = 8.54 * cm,
                            BorderBrush = Brushes.Black,
                            BorderThickness = new Thickness(0, 1, 1, 1),
                            Background = (Brush)(new BrushConverter().ConvertFromString("#4f81bd")),
                        };
                        textBlock = new TextBlock()
                        {
                            Text = "Panel Name",
                            FontSize = 18,
                            Foreground = Brushes.White,
                            FontWeight = FontWeights.Bold,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        border.Child = textBlock;
                        stackPanel.Children.Add(border);
                        //3
                        border = new Border()
                        {
                            Width = 1.2 * cm,
                            BorderBrush = Brushes.Black,
                            BorderThickness = new Thickness(0, 1, 1, 1),
                            Background = (Brush)(new BrushConverter().ConvertFromString("#4f81bd")),
                        };
                        textBlock = new TextBlock()
                        {
                            Text = "QTY",
                            FontSize = 18,
                            Foreground = Brushes.White,
                            FontWeight = FontWeights.Bold,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        border.Child = textBlock;
                        stackPanel.Children.Add(border);
                        //4
                        border = new Border()
                        {
                            Width = 3.5 * cm,
                            BorderBrush = Brushes.Black,
                            BorderThickness = new Thickness(0, 1, 1, 1),
                            Background = (Brush)(new BrushConverter().ConvertFromString("#4f81bd")),
                        };
                        textBlock = new TextBlock()
                        {
                            Text = "U/Price (SR)",
                            FontSize = 18,
                            Foreground = Brushes.White,
                            FontWeight = FontWeights.Bold,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        border.Child = textBlock;
                        stackPanel.Children.Add(border);
                        //5
                        border = new Border()
                        {
                            Width = 3.5 * cm,
                            BorderBrush = Brushes.Black,
                            BorderThickness = new Thickness(0, 1, 1, 1),
                            Background = (Brush)(new BrushConverter().ConvertFromString("#4f81bd")),
                        };
                        textBlock = new TextBlock()
                        {
                            Text = "T/Price (SR)",
                            FontSize = 18,
                            Foreground = Brushes.White,
                            FontWeight = FontWeights.Bold,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        border.Child = textBlock;
                        stackPanel.Children.Add(border);
                        stackPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        stackPanel.Arrange(new Rect(stackPanel.DesiredSize));

                        totalHeight += stackPanel.ActualHeight;
                        page.Body.Children.Add(stackPanel);

                        page.Body.Children.Add(subStackPanel);
                        totalHeight += subStackPanel.ActualHeight;
                    }
                    else
                    {
                        page.Body.Children.Add(stackPanel);
                    }
                }
                #endregion

                #region Totals
                var mainStackPanel = new StackPanel() { Orientation = Orientation.Vertical };

                if (showVAT || QuotationData.Discount != 0)
                {
                    stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                    //1
                    border = new Border()
                    {
                        Width = rowWidth,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1, 0, 1, 1),
                    };
                    textBlock = new TextBlock()
                    {
                        Text = "Total",
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 5, 0),
                        FontFamily = new FontFamily("Calibri (Body)"),
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    border.Child = textBlock;
                    stackPanel.Children.Add(border);
                    //2
                    border = new Border()
                    {
                        Width = 3.5 * cm,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(0, 0, 1, 1),
                    };
                    textBlock = new TextBlock()
                    {
                        Text = Math.Ceiling(QuotationData.Cost).ToString("N2"),
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    border.Child = textBlock;
                    stackPanel.Children.Add(border);
                    mainStackPanel.Children.Add(stackPanel);
                }

                if (QuotationData.Discount != 0)
                {
                    stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                    //1
                    border = new Border()
                    {
                        Width = rowWidth,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1, 0, 1, 1),
                    };
                    textBlock = new TextBlock()
                    {
                        Text = $"Discount {QuotationData.Discount:N2}%",
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 5, 0),
                        FontFamily = new FontFamily("Calibri (Body)"),
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    border.Child = textBlock;
                    stackPanel.Children.Add(border);
                    //2
                    border = new Border()
                    {
                        Width = 3.5 * cm,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(0, 0, 1, 1),
                    };
                    textBlock = new TextBlock()
                    {
                        Text = Math.Ceiling(QuotationData.DiscountValue).ToString("N2"),
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    border.Child = textBlock;
                    stackPanel.Children.Add(border);
                    mainStackPanel.Children.Add(stackPanel);
                }

                if (showVAT)
                {
                    stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                    //1
                    border = new Border()
                    {
                        Width = rowWidth,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1, 0, 1, 1),
                    };
                    textBlock = new TextBlock()
                    {
                        Text = $"VAT {QuotationData.VAT:N2}%",
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 5, 0),
                        FontFamily = new FontFamily("Calibri (Body)"),
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    border.Child = textBlock;
                    stackPanel.Children.Add(border);
                    //2
                    border = new Border()
                    {
                        Width = 3.5 * cm,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(0, 0, 1, 1),
                    };
                    textBlock = new TextBlock()
                    {
                        Text = Math.Ceiling(QuotationData.VAT_Value).ToString("N2"),
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    border.Child = textBlock;
                    stackPanel.Children.Add(border);
                    mainStackPanel.Children.Add(stackPanel);
                }

                stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                //1
                border = new Border()
                {
                    Width = rowWidth,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1, 0, 1, 1),
                };
                textBlock = new TextBlock()
                {
                    Text = "Total (Net Price)",
                    FontSize = 18,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 5, 0),
                    FontFamily = new FontFamily("Calibri (Body)"),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center
                };
                border.Child = textBlock;
                stackPanel.Children.Add(border);
                //2
                border = new Border()
                {
                    Width = 3.5 * cm,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0, 0, 1, 1),
                };

                if (showVAT)
                {
                    textBlock = new TextBlock()
                    {
                        Text = Math.Ceiling(QuotationData.PriceWithVAT).ToString("N2"),
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                }
                else
                {
                    textBlock = new TextBlock()
                    {
                        Text = Math.Ceiling(QuotationData.Price).ToString("N2"),
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                }

                border.Child = textBlock;
                stackPanel.Children.Add(border);
                mainStackPanel.Children.Add(stackPanel);

                textBlock = new TextBlock()
                {
                    Text = $"Only {Input.NumberToWords((int)Math.Ceiling(QuotationData.Price))} Saudi Riyals ",
                    FontSize = 18,
                    Width = 670,
                    TextWrapping = TextWrapping.Wrap,
                    FontFamily = new FontFamily("Calibri (Body)"),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };
                mainStackPanel.Children.Add(textBlock);

                if (!showVAT)
                {
                    textBlock = new TextBlock()
                    {
                        Text = $"\nNote: \nVAT excluded from price.",
                        FontSize = 18,
                        Width = 670,
                        TextWrapping = TextWrapping.Wrap,
                        Foreground = Brushes.Red,
                        FontWeight = FontWeights.Bold,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    mainStackPanel.Children.Add(textBlock);
                }

                mainStackPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                mainStackPanel.Arrange(new Rect(mainStackPanel.DesiredSize));
                totalHeight += mainStackPanel.ActualHeight;

                if (totalHeight > pageBodyHight)
                {
                    PanelsBillPages.Add(page);
                    page = new QuotationPage(QuotationData.Code);

                    line = new Line()
                    {
                        X1 = 0,
                        X2 = stackPanel.ActualWidth,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1,
                    };
                    page.Body.Children.Add(line);
                    page.Body.Children.Add(mainStackPanel);
                }
                else
                {
                    page.Body.Children.Add(mainStackPanel);
                }
                #endregion

                PanelsBillPages.Add(page);
                BillPages.Text = PanelsBillPages.Count.ToString();
            }
            else
            {
                PanelsBillPages = null;
                BillPages.Text = "0";
            }
        }
        private void Technical_Click(object sender, RoutedEventArgs e)
        {
            TechnicalCheck.IsChecked = !TechnicalCheck.IsChecked;
            if (Items == null)
            {
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    string query = $"Select * From Quotation._Items Where Table = '{Tables.Details}' ";
                    if (QuotationOptionData == null)
                        query += $"And PanelId In (Select Id From Quotation._Panels Where QuotationId = {QuotationData.Id}) Order By Sort";
                    else
                        query = $"Where PanelId In (Select PanelId From Quotation._OptionsPanels Where OptionId = {QuotationOptionData.Id}) Order By Sort";

                    Items = connection.Query<Item>(query).ToList();
                }
            }

            if (TechnicalCheck.IsChecked == true && Panels.Count != 0 && Items.Count != 0)
            {
                PanelsDetailsPages = new List<FrameworkElement>();
                var page = new QuotationPage(QuotationData.Code);
                page.Body.Margin = new Thickness(0, 5 * cm, 0, 0);

                Line line;
                TextBlock textBlock;
                double totalHeight = 0;

                textBlock = new TextBlock()
                {
                    Text = "TECHNICAL DETAILS",
                    FontSize = 29.5,
                    Foreground = (Brush)(new BrushConverter().ConvertFromString("#4f81bd")),
                    FontWeight = FontWeights.Bold,
                    FontFamily = new FontFamily("Calibri (Body)"),
                    TextWrapping = TextWrapping.Wrap,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top
                };
                textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                textBlock.Arrange(new Rect(textBlock.DesiredSize));
                totalHeight += textBlock.ActualHeight;
                page.Body.Children.Add(textBlock);

                line = new Line()
                {
                    X1 = 0,
                    X2 = 678,
                    Stroke = (Brush)(new BrushConverter().ConvertFromString("#4f81bd")),
                    StrokeThickness = 2,
                    HorizontalAlignment = HorizontalAlignment.Center,
                };
                line.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                line.Arrange(new Rect(line.DesiredSize));
                totalHeight += line.ActualHeight;
                page.Body.Children.Add(line);

                foreach (Data.QuotationsData.Panel panel in Panels)
                {
                    int ii = Panels.IndexOf(panel);
                    PanelsInformation quotationPanelInfo = new PanelsInformation(panel);
                    var panelItems = Items.Where(item => item.PanelId == panel.Id).ToList();

                    quotationPanelInfo.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    quotationPanelInfo.Arrange(new Rect(quotationPanelInfo.DesiredSize));

                    totalHeight += quotationPanelInfo.ActualHeight;

                    if (Panels.IndexOf(panel) == 0)
                    {
                        for (int i = 0; i < panelItems.Count(); i++)
                        {
                            if (totalHeight + 0.6 * cm >= pageBodyHight)
                            {
                                PanelsDetailsPages.Add(page);
                                page.Continue.Visibility = Visibility.Visible;
                                page = new QuotationPage(QuotationData.Code);
                                page.Body.Margin = new Thickness(0, 5 * cm, 0, 0);

                                totalHeight = 0;
                                quotationPanelInfo = new PanelsInformation(panel);
                                quotationPanelInfo.Continue.Visibility = Visibility.Visible;
                                page.Body.Children.Add(quotationPanelInfo);
                                quotationPanelInfo.AddItem = panelItems[i];

                                quotationPanelInfo.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                                quotationPanelInfo.Arrange(new Rect(quotationPanelInfo.DesiredSize));
                                totalHeight += quotationPanelInfo.ActualHeight;
                            }

                            else
                            {
                                if (i == 0)
                                    page.Body.Children.Add(quotationPanelInfo);

                                quotationPanelInfo.AddItem = panelItems[i];
                                totalHeight += 0.6 * cm;
                            }
                        }
                    }

                    else
                    {
                        double panelInformationHeight;
                        quotationPanelInfo.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        quotationPanelInfo.Arrange(new Rect(quotationPanelInfo.DesiredSize));
                        panelInformationHeight = quotationPanelInfo.ActualHeight + panelItems.Count * 0.6 * cm;

                        if (panelInformationHeight < (750 - totalHeight))
                        {
                            for (int i = 0; i < panelItems.Count(); i++)
                            {
                                if (i == 0)
                                    page.Body.Children.Add(quotationPanelInfo);

                                quotationPanelInfo.AddItem = panelItems[i];
                                totalHeight += 0.6 * cm;
                            }
                        }

                        else if (750 > panelInformationHeight && panelInformationHeight > 750 * 0.6)
                        {
                            PanelsDetailsPages.Add(page);
                            page = new QuotationPage(QuotationData.Code);
                            page.Body.Margin = new Thickness(0, 5 * cm, 0, 0);

                            totalHeight = 0;
                            quotationPanelInfo = new PanelsInformation(panel);
                            page.Body.Children.Add(quotationPanelInfo);

                            quotationPanelInfo.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                            quotationPanelInfo.Arrange(new Rect(quotationPanelInfo.DesiredSize));
                            totalHeight += quotationPanelInfo.ActualHeight;

                            for (int i = 0; i < panelItems.Count(); i++)
                            {
                                quotationPanelInfo.AddItem = panelItems[i];
                                totalHeight += 0.6 * cm;
                            }
                        }

                        else
                        {
                            for (int i = 0; i < panelItems.Count(); i++)
                            {
                                if (i != 0 && totalHeight + 0.6 * cm >= pageBodyHight)
                                {
                                    PanelsDetailsPages.Add(page);
                                    page.Continue.Visibility = Visibility.Visible;
                                    page = new QuotationPage(QuotationData.Code);
                                    page.Body.Margin = new Thickness(0, 5 * cm, 0, 0);

                                    totalHeight = 0;
                                    quotationPanelInfo = new PanelsInformation(panel);
                                    quotationPanelInfo.Continue.Visibility = Visibility.Visible;
                                    page.Body.Children.Add(quotationPanelInfo);
                                    quotationPanelInfo.AddItem = panelItems[i];

                                    quotationPanelInfo.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                                    quotationPanelInfo.Arrange(new Rect(quotationPanelInfo.DesiredSize));
                                    totalHeight += quotationPanelInfo.ActualHeight;
                                }

                                else if (i == 0 && totalHeight + 0.6 * cm >= pageBodyHight)
                                {
                                    PanelsDetailsPages.Add(page);
                                    page = new QuotationPage(QuotationData.Code);
                                    page.Body.Margin = new Thickness(0, 5 * cm, 0, 0);

                                    totalHeight = 0;
                                    quotationPanelInfo = new PanelsInformation(panel);
                                    page.Body.Children.Add(quotationPanelInfo);
                                    quotationPanelInfo.AddItem = panelItems[i];

                                    quotationPanelInfo.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                                    quotationPanelInfo.Arrange(new Rect(quotationPanelInfo.DesiredSize));
                                    totalHeight += quotationPanelInfo.ActualHeight;
                                }

                                else if (i == 0 && totalHeight + (5 * 0.6 * cm) >= pageBodyHight)
                                {
                                    PanelsDetailsPages.Add(page);
                                    page = new QuotationPage(QuotationData.Code);
                                    page.Body.Margin = new Thickness(0, 5 * cm, 0, 0);

                                    totalHeight = 0;
                                    quotationPanelInfo = new PanelsInformation(panel);
                                    page.Body.Children.Add(quotationPanelInfo);
                                    quotationPanelInfo.AddItem = panelItems[i];

                                    quotationPanelInfo.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                                    quotationPanelInfo.Arrange(new Rect(quotationPanelInfo.DesiredSize));
                                    totalHeight += quotationPanelInfo.ActualHeight;
                                }

                                else
                                {
                                    if (i == 0)
                                        page.Body.Children.Add(quotationPanelInfo);

                                    quotationPanelInfo.AddItem = panelItems[i];
                                    totalHeight += 0.6 * cm;
                                }
                            }
                        }
                    }

                }

                PanelsDetailsPages.Add(page);
                TechnicalPages.Text = PanelsDetailsPages.Count.ToString();
            }
            else
            {
                PanelsDetailsPages = null;
                TechnicalPages.Text = "0";
            }
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            var quotationPages = new List<FrameworkElement>();

            QuotationPage page;

            var totalPages = int.Parse(CoverPages.Text) +
                             int.Parse(TermsPages.Text) +
                             int.Parse(BillPages.Text) +
                             int.Parse(TechnicalPages.Text);


            if (CoverCheck.IsChecked == true)
            {
                int startPage = 1;
                Content content;
                List<Content> contents = new List<Content>();
                if (TermsCheck.IsChecked == true)
                {
                    content = new Content();
                    content.Description = "Commercial Terms & Conditions.";
                    if (int.Parse(TermsPages.Text) == 1)
                        content.Pages = (++startPage).ToString();
                    else
                        content.Pages = $"{++startPage} - {startPage + int.Parse(TermsPages.Text) - 1}";

                    startPage = startPage + int.Parse(TermsPages.Text) - 1;
                    contents.Add(content);
                }
                if (BillCheck.IsChecked == true && Panels.Count != 0)
                {
                    content = new Content();
                    content.Description = "Bill of Quantity & Price.";
                    if (int.Parse(BillPages.Text) == 1)
                        content.Pages = (++startPage).ToString();
                    else
                        content.Pages = $"{++startPage} - {startPage + int.Parse(BillPages.Text) - 1}";

                    startPage = startPage + int.Parse(BillPages.Text) - 1;
                    contents.Add(content);
                }
                if (TechnicalCheck.IsChecked == true && Panels.Count != 0 && Items.Count != 0)
                {
                    content = new Content();
                    content.Description = "Technical Details.";
                    if (int.Parse(TechnicalPages.Text) == 1)
                        content.Pages = (++startPage).ToString();
                    else
                        content.Pages = $"{++startPage} - {startPage + int.Parse(TechnicalPages.Text) - 1}";

                    contents.Add(content);
                }


                var quotationCoverPage = new Cover(UserData, QuotationData, contents);
                quotationCoverPage.TotalPages.Text = totalPages.ToString();
                quotationPages.Add(quotationCoverPage);
                quotationCoverPage.PageNumber.Text = quotationPages.Count.ToString();
            }

            if (TermsCheck.IsChecked == true && Terms.Count != 0)
            {
                page = new QuotationPage(QuotationData.Code);
                page.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                page.Arrange(new Rect(page.DesiredSize));

                int topicNumber = 0;
                double totalHeight = 0;
                Line line;
                TextBlock textBlock;
                StackPanel stackPanel;

                var ScopeOfSupply = Terms.Where<Term>(item => item.ConditionType == TermTypes.ScopeOfSupply.ToString());
                var TotalPrice = Terms.Where<Term>(item => item.ConditionType == TermTypes.TotalPrice.ToString());
                var PaymentConditions = Terms.Where<Term>(item => item.ConditionType == TermTypes.PaymentConditions.ToString());
                var ValidityPeriod = Terms.Where<Term>(item => item.ConditionType == TermTypes.ValidityPeriod.ToString());
                var ShopDrawingSubmittals = Terms.Where<Term>(item => item.ConditionType == TermTypes.ShopDrawingSubmittals.ToString());
                var Delivery = Terms.Where<Term>(item => item.ConditionType == TermTypes.Delivery.ToString());
                var Guarantee = Terms.Where<Term>(item => item.ConditionType == TermTypes.Guarantee.ToString());

                textBlock = new TextBlock()
                {
                    Text = "COMMERCIAL TERMS & CONDITIONS",
                    FontSize = 29.5,
                    Foreground = (Brush)(new BrushConverter().ConvertFromString("#4f81bd")),
                    FontWeight = FontWeights.Bold,
                    FontFamily = new FontFamily("Calibri (Body)"),
                    TextWrapping = TextWrapping.Wrap,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top
                };
                textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                textBlock.Arrange(new Rect(textBlock.DesiredSize));
                page.Body.Children.Add(textBlock);
                totalHeight += textBlock.ActualHeight;

                line = new Line()
                {
                    X1 = 0,
                    X2 = page.Body.ActualWidth,
                    Stroke = (Brush)(new BrushConverter().ConvertFromString("#4f81bd")),
                    StrokeThickness = 2,
                };
                line.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                line.Arrange(new Rect(line.DesiredSize));
                page.Body.Children.Add(line);
                totalHeight += line.ActualHeight;

                if (ScopeOfSupply.Count() != 0)
                {
                    textBlock = new TextBlock()
                    {
                        Text = $"{++topicNumber}. Scope Of Supply:",
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        TextWrapping = TextWrapping.Wrap,
                        TextDecorations = TextDecorations.Underline,
                        Margin = new Thickness(0, 0.5 * cm, 0, 0),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    textBlock.Arrange(new Rect(textBlock.DesiredSize));
                    totalHeight += textBlock.ActualHeight + 0.5 * cm;

                    if (totalHeight > pageBodyHight)
                    {
                        page.TotalPages.Text = totalPages.ToString();
                        quotationPages.Add(page);
                        page.PageNumber.Text = quotationPages.Count.ToString();
                        totalHeight = 0;
                        page = new QuotationPage(QuotationData.Code);
                        page.Body.Children.Add(textBlock);
                        totalHeight += textBlock.ActualHeight;
                    }
                    else
                    {
                        page.Body.Children.Add(textBlock);
                    }

                    foreach (Term termData in ScopeOfSupply)
                    {
                        stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                        textBlock = new TextBlock()
                        {
                            Text = $"» ",
                            FontSize = 18,
                            FontWeight = FontWeights.Bold,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);
                        textBlock = new TextBlock()
                        {
                            Text = termData.Condition,
                            FontSize = 18,
                            Width = 670,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);

                        stackPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        stackPanel.Arrange(new Rect(stackPanel.DesiredSize));
                        totalHeight += stackPanel.ActualHeight;

                        if (totalHeight > pageBodyHight)
                        {
                            page.TotalPages.Text = totalPages.ToString();
                            quotationPages.Add(page);
                            page.PageNumber.Text = quotationPages.Count.ToString();
                            totalHeight = 0;
                            page = new QuotationPage(QuotationData.Code);
                            page.Body.Children.Add(stackPanel);
                            totalHeight += stackPanel.ActualHeight;
                        }
                        else
                        {
                            page.Body.Children.Add(stackPanel);
                        }

                    }
                }

                if (TotalPrice.Count() != 0)
                {
                    textBlock = new TextBlock()
                    {
                        Text = $"{++topicNumber}. Total Price:",
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        TextWrapping = TextWrapping.Wrap,
                        TextDecorations = TextDecorations.Underline,
                        Margin = new Thickness(0, 0.5 * cm, 0, 0),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    textBlock.Arrange(new Rect(textBlock.DesiredSize));
                    totalHeight += textBlock.ActualHeight + 0.5 * cm;

                    if (totalHeight > pageBodyHight)
                    {
                        page.TotalPages.Text = totalPages.ToString();
                        quotationPages.Add(page);
                        page.PageNumber.Text = quotationPages.Count.ToString();
                        totalHeight = 0;
                        page = new QuotationPage(QuotationData.Code);
                        page.Body.Children.Add(textBlock);
                        totalHeight += textBlock.ActualHeight;
                    }
                    else
                    {
                        page.Body.Children.Add(textBlock);
                    }

                    textBlock = new TextBlock()
                    {
                        Text = $"Our Prices are meant:",
                        FontSize = 18,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        TextWrapping = TextWrapping.Wrap,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    textBlock.Arrange(new Rect(textBlock.DesiredSize));
                    totalHeight += textBlock.ActualHeight;

                    if (totalHeight > pageBodyHight)
                    {
                        page.TotalPages.Text = totalPages.ToString();
                        quotationPages.Add(page);
                        page.PageNumber.Text = quotationPages.Count.ToString();
                        totalHeight = 0;
                        page = new QuotationPage(QuotationData.Code);
                        page.Body.Children.Add(textBlock);
                        totalHeight += textBlock.ActualHeight;
                    }
                    else
                    {
                        page.Body.Children.Add(textBlock);
                    }

                    foreach (Term termData in TotalPrice)
                    {
                        stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                        textBlock = new TextBlock()
                        {
                            Text = $"» ",
                            FontSize = 18,
                            FontWeight = FontWeights.Bold,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);
                        textBlock = new TextBlock()
                        {
                            Text = termData.Condition,
                            FontSize = 18,
                            Width = 670,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);

                        stackPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        stackPanel.Arrange(new Rect(stackPanel.DesiredSize));
                        totalHeight += stackPanel.ActualHeight;

                        if (totalHeight > pageBodyHight)
                        {
                            page.TotalPages.Text = totalPages.ToString();
                            quotationPages.Add(page);
                            page.PageNumber.Text = quotationPages.Count.ToString();
                            totalHeight = 0;
                            page = new QuotationPage(QuotationData.Code);
                            page.Body.Children.Add(stackPanel);
                            totalHeight += stackPanel.ActualHeight;
                        }
                        else
                        {
                            page.Body.Children.Add(stackPanel);
                        }

                    }
                }

                if (PaymentConditions.Count() != 0)
                {
                    textBlock = new TextBlock()
                    {
                        Text = $"{++topicNumber}. Payment Conditions:",
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        TextWrapping = TextWrapping.Wrap,
                        TextDecorations = TextDecorations.Underline,
                        Margin = new Thickness(0, 0.5 * cm, 0, 0),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    textBlock.Arrange(new Rect(textBlock.DesiredSize));
                    totalHeight += textBlock.ActualHeight + 0.5 * cm;

                    if (totalHeight > pageBodyHight)
                    {
                        page.TotalPages.Text = totalPages.ToString();
                        quotationPages.Add(page);
                        page.PageNumber.Text = quotationPages.Count.ToString();
                        totalHeight = 0;
                        page = new QuotationPage(QuotationData.Code);
                        page.Body.Children.Add(textBlock);
                        totalHeight += textBlock.ActualHeight;
                    }
                    else
                    {
                        page.Body.Children.Add(textBlock);
                    }

                    foreach (Term termData in PaymentConditions)
                    {
                        stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                        textBlock = new TextBlock()
                        {
                            Text = $"» ",
                            FontSize = 18,
                            FontWeight = FontWeights.Bold,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);
                        textBlock = new TextBlock()
                        {
                            Text = termData.Condition,
                            FontSize = 18,
                            Width = 670,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);

                        stackPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        stackPanel.Arrange(new Rect(stackPanel.DesiredSize));
                        totalHeight += stackPanel.ActualHeight;

                        if (totalHeight > pageBodyHight)
                        {
                            page.TotalPages.Text = totalPages.ToString();
                            quotationPages.Add(page);
                            page.PageNumber.Text = quotationPages.Count.ToString();
                            totalHeight = 0;
                            page = new QuotationPage(QuotationData.Code);
                            page.Body.Children.Add(stackPanel);
                            totalHeight += stackPanel.ActualHeight;
                        }
                        else
                        {
                            page.Body.Children.Add(stackPanel);
                        }

                    }
                }

                if (ValidityPeriod.Count() != 0)
                {
                    textBlock = new TextBlock()
                    {
                        Text = $"{++topicNumber}. Validity Period:",
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        TextWrapping = TextWrapping.Wrap,
                        TextDecorations = TextDecorations.Underline,
                        Margin = new Thickness(0, 0.5 * cm, 0, 0),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    textBlock.Arrange(new Rect(textBlock.DesiredSize));
                    totalHeight += textBlock.ActualHeight + 0.5 * cm;

                    if (totalHeight > pageBodyHight)
                    {
                        page.TotalPages.Text = totalPages.ToString();
                        quotationPages.Add(page);
                        page.PageNumber.Text = quotationPages.Count.ToString();
                        totalHeight = 0;
                        page = new QuotationPage(QuotationData.Code);
                        page.Body.Children.Add(textBlock);
                        totalHeight += textBlock.ActualHeight;
                    }
                    else
                    {
                        page.Body.Children.Add(textBlock);
                    }

                    foreach (Term termData in ValidityPeriod)
                    {
                        stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                        textBlock = new TextBlock()
                        {
                            Text = $"» ",
                            FontSize = 18,
                            FontWeight = FontWeights.Bold,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);
                        textBlock = new TextBlock()
                        {
                            Text = termData.Condition,
                            FontSize = 18,
                            Width = 670,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);

                        stackPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        stackPanel.Arrange(new Rect(stackPanel.DesiredSize));
                        totalHeight += stackPanel.ActualHeight;

                        if (totalHeight > pageBodyHight)
                        {
                            page.TotalPages.Text = totalPages.ToString();
                            quotationPages.Add(page);
                            page.PageNumber.Text = quotationPages.Count.ToString();
                            totalHeight = 0;
                            page = new QuotationPage(QuotationData.Code);
                            page.Body.Children.Add(stackPanel);
                            totalHeight += stackPanel.ActualHeight;
                        }
                        else
                        {
                            page.Body.Children.Add(stackPanel);
                        }

                    }
                }

                if (ShopDrawingSubmittals.Count() != 0)
                {
                    textBlock = new TextBlock()
                    {
                        Text = $"{++topicNumber}. Shop Drawing Submittals:",
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        TextWrapping = TextWrapping.Wrap,
                        TextDecorations = TextDecorations.Underline,
                        Margin = new Thickness(0, 0.5 * cm, 0, 0),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    textBlock.Arrange(new Rect(textBlock.DesiredSize));
                    totalHeight += textBlock.ActualHeight + 0.5 * cm;

                    if (totalHeight > pageBodyHight)
                    {
                        page.TotalPages.Text = totalPages.ToString();
                        quotationPages.Add(page);
                        page.PageNumber.Text = quotationPages.Count.ToString();
                        totalHeight = 0;
                        page = new QuotationPage(QuotationData.Code);
                        page.Body.Children.Add(textBlock);
                        totalHeight += textBlock.ActualHeight;
                    }
                    else
                    {
                        page.Body.Children.Add(textBlock);
                    }

                    foreach (Term termData in ShopDrawingSubmittals)
                    {
                        stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                        textBlock = new TextBlock()
                        {
                            Text = $"» ",
                            FontSize = 18,
                            FontWeight = FontWeights.Bold,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);
                        textBlock = new TextBlock()
                        {
                            Text = termData.Condition,
                            FontSize = 18,
                            Width = 670,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);

                        stackPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        stackPanel.Arrange(new Rect(stackPanel.DesiredSize));
                        totalHeight += stackPanel.ActualHeight;

                        if (totalHeight > pageBodyHight)
                        {
                            page.TotalPages.Text = totalPages.ToString();
                            quotationPages.Add(page);
                            page.PageNumber.Text = quotationPages.Count.ToString();
                            totalHeight = 0;
                            page = new QuotationPage(QuotationData.Code);
                            page.Body.Children.Add(stackPanel);
                            totalHeight += stackPanel.ActualHeight;
                        }
                        else
                        {
                            page.Body.Children.Add(stackPanel);
                        }

                    }
                }

                if (Delivery.Count() != 0)
                {
                    textBlock = new TextBlock()
                    {
                        Text = $"{++topicNumber}. Delivery:",
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        TextWrapping = TextWrapping.Wrap,
                        TextDecorations = TextDecorations.Underline,
                        Margin = new Thickness(0, 0.5 * cm, 0, 0),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    textBlock.Arrange(new Rect(textBlock.DesiredSize));
                    totalHeight += textBlock.ActualHeight + 0.5 * cm;

                    if (totalHeight > pageBodyHight)
                    {
                        page.TotalPages.Text = totalPages.ToString();
                        quotationPages.Add(page);
                        page.PageNumber.Text = quotationPages.Count.ToString();
                        totalHeight = 0;
                        page = new QuotationPage(QuotationData.Code);
                        page.Body.Children.Add(textBlock);
                        totalHeight += textBlock.ActualHeight;
                    }
                    else
                    {
                        page.Body.Children.Add(textBlock);
                    }

                    foreach (Term termData in Delivery)
                    {
                        stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                        textBlock = new TextBlock()
                        {
                            Text = $"» ",
                            FontSize = 18,
                            FontWeight = FontWeights.Bold,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);
                        textBlock = new TextBlock()
                        {
                            Text = termData.Condition,
                            FontSize = 18,
                            Width = 670,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);

                        stackPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        stackPanel.Arrange(new Rect(stackPanel.DesiredSize));
                        totalHeight += stackPanel.ActualHeight;

                        if (totalHeight > pageBodyHight)
                        {
                            page.TotalPages.Text = totalPages.ToString();
                            quotationPages.Add(page);
                            page.PageNumber.Text = quotationPages.Count.ToString();
                            totalHeight = 0;
                            page = new QuotationPage(QuotationData.Code);
                            page.Body.Children.Add(stackPanel);
                            totalHeight += stackPanel.ActualHeight;
                        }
                        else
                        {
                            page.Body.Children.Add(stackPanel);
                        }

                    }
                }

                if (Guarantee.Count() != 0)
                {
                    textBlock = new TextBlock()
                    {
                        Text = $"{++topicNumber}. Guarantee:",
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        FontFamily = new FontFamily("Calibri (Body)"),
                        TextWrapping = TextWrapping.Wrap,
                        TextDecorations = TextDecorations.Underline,
                        Margin = new Thickness(0, 0.5 * cm, 0, 0),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    textBlock.Arrange(new Rect(textBlock.DesiredSize));
                    totalHeight += textBlock.ActualHeight + 0.5 * cm;

                    if (totalHeight > pageBodyHight)
                    {
                        page.TotalPages.Text = totalPages.ToString();
                        quotationPages.Add(page);
                        page.PageNumber.Text = quotationPages.Count.ToString();
                        totalHeight = 0;
                        page = new QuotationPage(QuotationData.Code);
                        page.Body.Children.Add(textBlock);
                        totalHeight += textBlock.ActualHeight;
                    }
                    else
                    {
                        page.Body.Children.Add(textBlock);
                    }

                    foreach (Term termData in Guarantee)
                    {
                        stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                        textBlock = new TextBlock()
                        {
                            Text = $"» ",
                            FontSize = 18,
                            FontWeight = FontWeights.Bold,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);
                        textBlock = new TextBlock()
                        {
                            Text = termData.Condition,
                            FontSize = 18,
                            Width = 670,
                            FontFamily = new FontFamily("Calibri (Body)"),
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        stackPanel.Children.Add(textBlock);

                        stackPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        stackPanel.Arrange(new Rect(stackPanel.DesiredSize));
                        totalHeight += stackPanel.ActualHeight;

                        if (totalHeight > pageBodyHight)
                        {
                            page.TotalPages.Text = totalPages.ToString();
                            quotationPages.Add(page);
                            page.PageNumber.Text = quotationPages.Count.ToString();
                            totalHeight = 0;
                            page = new QuotationPage(QuotationData.Code);
                            page.Body.Children.Add(stackPanel);
                            totalHeight += stackPanel.ActualHeight;
                        }
                        else
                        {
                            page.Body.Children.Add(stackPanel);
                        }
                    }
                }

                page.TotalPages.Text = totalPages.ToString();
                quotationPages.Add(page);
                page.PageNumber.Text = quotationPages.Count.ToString();
            }

            if (BillCheck.IsChecked == true && Panels.Count != 0)
            {
                foreach (FrameworkElement element in PanelsBillPages)
                {
                    ((QuotationPage)element).TotalPages.Text = totalPages.ToString();
                    quotationPages.Add(element);
                    ((QuotationPage)element).PageNumber.Text = quotationPages.Count.ToString();
                }
            }

            if (TechnicalCheck.IsChecked == true && Panels.Count != 0 && Items.Count != 0)
            {
                foreach (FrameworkElement element in PanelsDetailsPages)
                {
                    ((QuotationPage)element).TotalPages.Text = totalPages.ToString();
                    quotationPages.Add(element);
                    ((QuotationPage)element).PageNumber.Text = quotationPages.Count.ToString();
                }
            }

            if (quotationPages.Count != 0)
                Print.Preview.Show(quotationPages, $"Quotation {QuotationData.Code}");

            this.Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ShowVATToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            showVAT = true;
            if (BillCheck.IsChecked == true)
            {
                BillCheck.IsChecked = false;
                Bill_Click(sender, e);
            }
        }
        private void ShowVATToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            showVAT = false;
            if (BillCheck.IsChecked == true)
            {
                BillCheck.IsChecked = false;
                Bill_Click(sender, e);
            }
        }
    }
}
