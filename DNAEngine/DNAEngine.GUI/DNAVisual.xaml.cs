using DNAEngine.Machine;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace DNAEngine.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DNAVisual : Window
    {
        private static int step = 20;
        private DNAMachine _DNAMachine;
        private int beginPosition = 0;
        private int endPosition = step;
        public DNAVisual(DNAMachine dnaMachine)
        {
            InitializeComponent();

            _DNAMachine = dnaMachine;
            ShowData(0,20);           
        }
        private void btnStepDown_Click(object sender, RoutedEventArgs e)
        {
            if (step > 0)
            {
                step--;
            }
            lblStep.Content = "STEP: " + step;
        }

        private void btnStepUp_Click(object sender, RoutedEventArgs e)
        {
            step++;
            lblStep.Content = "STEP: " + step;

        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if ((endPosition + step) <= (_DNAMachine.DNAData.Length - 1))
            {
                endPosition += step;
            }
            ShowData(beginPosition, endPosition);
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if ((endPosition - step) >= 0)
            {
                endPosition -= step;
            }
            ShowData(beginPosition, endPosition);
        }
        private void ShowData(int begin, int end)
        {
            DNAData_Label.Text = _DNAMachine.Read(begin, end, Machine.Language.DNA);
            MRNAData_Label.Text = _DNAMachine.Read(begin, end, Machine.Language.MRNA);
            TRNAData_Label.Text = _DNAMachine.Read(begin, end, Machine.Language.TRNA);

            AminoAcids_ListBox.Items.Clear();
            foreach (string aminoacid in _DNAMachine.ReadAminoAcids(begin, end))
            {
                ListBox aa = new ListBox();
                ListBoxItem aaName = new ListBoxItem() { Content = aminoacid };


                Image img = new Image();
                BitmapImage pic = new BitmapImage();
                pic.BeginInit();
                pic.UriSource = new Uri("pack://application:,,,/DNAEngine.GUI;component/AminoAcids/" + aminoacid + ".png");
                pic.EndInit();

                img.Source = pic;

                ListBoxItem aaPic = new ListBoxItem() { Content = img };

                aa.Items.Add(aaName);
                aa.Items.Add(aaPic);

                AminoAcids_ListBox.Items.Add(aa);
            }

            PeptineBonds_ListBox.Items.Clear();
            foreach (List<string> pb in _DNAMachine.ReadPeptineBonds(begin, end))
            {
                foreach (string aminoacid in pb)
                {
                    ListBox aa = new ListBox();
                    ListBoxItem aaName = new ListBoxItem() { Content = aminoacid };


                    Image img = new Image();
                    BitmapImage pic = new BitmapImage();
                    pic.BeginInit();
                    pic.UriSource = new Uri("pack://application:,,,/DNAEngine.GUI;component/AminoAcids/" + aminoacid + ".png");
                    pic.EndInit();

                    img.Source = pic;

                    ListBoxItem aaPic = new ListBoxItem() { Content = img };

                    aa.Items.Add(aaName);
                    aa.Items.Add(aaPic);

                    PeptineBonds_ListBox.Items.Add(aa);
                }
            }
        }
    }


}
