using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using Xceed.Wpf.Toolkit.Primitives;

namespace _2026_WpfApp4
{
    /// <summary>
    /// MyDocument.xaml 的互動邏輯
    /// </summary>
    public partial class MyDocument : Window
    {
        Color fontColor = Colors.Black;
        
        public MyDocument()
        {
            InitializeComponent();
            FontColorPicker.SelectedColor = fontColor;

            foreach (FontFamily font in Fonts.SystemFontFamilies)
            {
                FontFamilyComboBox.Items.Add(font);
            }
            FontFamilyComboBox.SelectedIndex = 1;

            FontSizeComboBox.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            FontSizeComboBox.SelectedIndex = 4;
        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MyDocument newWindow = new MyDocument();
            newWindow.Show();
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*",
                DefaultExt = ".rtf"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                TextRange range = new TextRange(MainRichTextBox.Document.ContentStart, MainRichTextBox.Document.ContentEnd);
                FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open);
                range.Load(fileStream, DataFormats.Rtf);
                fileStream.Close();
            }
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*",
                DefaultExt = ".rtf",
                AddExtension = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                TextRange range = new TextRange(MainRichTextBox.Document.ContentStart, MainRichTextBox.Document.ContentEnd);
                FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create);
                range.Save(fileStream, DataFormats.Rtf);
                fileStream.Close();
            }
        }

        private void FontColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            fontColor = (Color)e.NewValue;
            SolidColorBrush fontBrush = new SolidColorBrush(fontColor);
            MainRichTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, fontBrush);
        }

        private void FontFamilyComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (FontFamilyComboBox.SelectedItem != null)
            {
                FontFamily selectedFontFamily = (FontFamily)FontFamilyComboBox.SelectedItem;
                MainRichTextBox.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, selectedFontFamily);
            }
        }

        private void FontSizeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (FontSizeComboBox.SelectedItem != null)
            {
                double selectedFontSize = (double)FontSizeComboBox.SelectedItem;
                MainRichTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, selectedFontSize);
            }
        }
        
        private void MainRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var property_bold = MainRichTextBox.Selection.GetPropertyValue(TextElement.FontWeightProperty);
            BoldButton.IsChecked = (property_bold != DependencyProperty.UnsetValue) && (property_bold.Equals(FontWeights.Bold));

            var property_italic = MainRichTextBox.Selection.GetPropertyValue(TextElement.FontStyleProperty);
            ItalicButton.IsChecked = (property_italic != DependencyProperty.UnsetValue) && (property_italic.Equals(FontStyles.Italic));

            var property_underline = MainRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            UnderlineButton.IsChecked = (property_underline != DependencyProperty.UnsetValue) && (property_underline.Equals(TextDecorations.Underline));

            var property_fontfamily = MainRichTextBox.Selection.GetPropertyValue(TextElement.FontFamilyProperty);
            FontFamilyComboBox.SelectedItem = property_fontfamily;

            var property_fontsize = MainRichTextBox.Selection.GetPropertyValue(TextElement.FontSizeProperty);
            FontSizeComboBox.SelectedItem = property_fontsize;

            var property_fontcolor = MainRichTextBox.Selection.GetPropertyValue(TextElement.ForegroundProperty);
            FontColorPicker.SelectedColor = ((SolidColorBrush)property_fontcolor).Color;
        }

        private void TrashButton_Click(object sender, RoutedEventArgs e)
        {
            MainRichTextBox.Document.Blocks.Clear();
        }
    }
}
