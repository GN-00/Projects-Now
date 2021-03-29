using System.Windows;
using System.Printing;
using System.Windows.Xps;
using System.Windows.Markup;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows.Xps.Packaging;

namespace Core.Print
{
    class Preview
    {
        public static void Show(FrameworkElement Page, string documentName)
        {
            if (System.IO.File.Exists("PrintPreview.xps"))
            { System.IO.File.Delete("PrintPreview.xps"); }

            XpsDocument xpsDocument = new XpsDocument("PrintPreview.xps", System.IO.FileAccess.ReadWrite);
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDocument);

            FixedDocument fixedDocument = new FixedDocument();
            PageContent pageContent;
            FixedPage fixedPage;

            fixedPage = new FixedPage
            { Width = Page.Width, Height = Page.Height };
            fixedPage.Children.Add(Page);

            pageContent = new PageContent();
            ((IAddChild)pageContent).AddChild(fixedPage);

            fixedDocument.Pages.Add(pageContent);

            writer.Write(fixedDocument);

            var printWindow = new PrintWindow(fixedDocument, documentName);
            printWindow.ShowDialog();
            xpsDocument.Close();
        }

        public static void Show(FrameworkElement Page, string documentName, PageOrientation pageOrientation)
        {
            if (System.IO.File.Exists("PrintPreview.xps"))
            { System.IO.File.Delete("PrintPreview.xps"); }

            XpsDocument xpsDocument = new XpsDocument("PrintPreview.xps", System.IO.FileAccess.ReadWrite);
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDocument);

            FixedDocument fixedDocument = new FixedDocument();
            PageContent pageContent;
            FixedPage fixedPage;

            fixedPage = new FixedPage
            { Width = Page.Width, Height = Page.Height };
            fixedPage.Children.Add(Page);

            pageContent = new PageContent();
            ((IAddChild)pageContent).AddChild(fixedPage);

            fixedDocument.Pages.Add(pageContent);

            writer.Write(fixedDocument);

            var printWindow = new PrintWindow(fixedDocument, documentName, pageOrientation);
            printWindow.ShowDialog();
            xpsDocument.Close();
        }

        public static void Show(List<FrameworkElement> Pages, string documentName)
        {
            if (System.IO.File.Exists("PrintPreview.xps"))
            { System.IO.File.Delete("PrintPreview.xps"); }

            XpsDocument xpsDocument = new XpsDocument("PrintPreview.xps", System.IO.FileAccess.ReadWrite);
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDocument);

            FixedDocument fixedDocument = new FixedDocument();
            PageContent pageContent;
            FixedPage fixedPage;

            for (int i = 0; i < Pages.Count; i++)
            {
                fixedPage = new FixedPage
                { Width = Pages[i].Width, Height = Pages[i].Height, Margin = new Thickness(-10) };
                fixedPage.Children.Add(Pages[i]);
                pageContent = new PageContent();
                ((IAddChild)pageContent).AddChild(fixedPage);
                fixedDocument.Pages.Add(pageContent);
            }

            writer.Write(fixedDocument);

            var printWindow = new PrintWindow(fixedDocument, documentName);
            printWindow.Show();
            xpsDocument.Close();
        }

        public static void Show(List<FrameworkElement> Pages, string documentName, PageOrientation pageOrientation)
        {
            if (System.IO.File.Exists("PrintPreview.xps"))
            { System.IO.File.Delete("PrintPreview.xps"); }

            XpsDocument xpsDocument = new XpsDocument("PrintPreview.xps", System.IO.FileAccess.ReadWrite);
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDocument);

            FixedDocument fixedDocument = new FixedDocument();
            PageContent pageContent;
            FixedPage fixedPage;

            for (int i = 0; i < Pages.Count; i++)
            {
                fixedPage = new FixedPage
                { Width = Pages[i].Width, Height = Pages[i].Height, Margin = new Thickness(-10) };
                fixedPage.Children.Add(Pages[i]);
                pageContent = new PageContent();
                ((IAddChild)pageContent).AddChild(fixedPage);
                fixedDocument.Pages.Add(pageContent);
            }

            writer.Write(fixedDocument);

            var printWindow = new PrintWindow(fixedDocument, documentName, pageOrientation);
            printWindow.Show();
            xpsDocument.Close();
        }
    }
}
