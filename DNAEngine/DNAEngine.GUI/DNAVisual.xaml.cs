using DNAEngine.Machine;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DNAEngine.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DNAVisual : Window
    {
        public DNAVisual(DNAMachine DNAMachine)
        {
            InitializeComponent();
            //DNAData_Label.Content = DNAMachine.DNAData;
            //MRNAData_Label.Content = DNAMachine.MRNADATA;
            //TRNAData_Label.Content = DNAMachine.TRNADATA;


            //foreach (string aminoacid in DNAMachine.AminoAcids)
            //{
            //    ListBox aa = new ListBox();
            //    ListBoxItem aaName = new ListBoxItem() { Content = aminoacid };


            //    Image img = new Image();
            //    BitmapImage pic = new BitmapImage();
            //    pic.BeginInit();
            //    pic.UriSource = new Uri("pack://application:,,,/DNAEngine.GUI;component/AminoAcids/" + aminoacid + ".png");
            //    pic.EndInit();

            //    img.Source = pic;

            //    ListBoxItem aaPic = new ListBoxItem() { Content = img };

            //    aa.Items.Add(aaName);
            //    aa.Items.Add(aaPic);

            //    AminoAcids_ListBox.Items.Add(aa);
            //}

            //foreach (List<string> pb in DNAMachine.PeptineBonds)
            //{
            //    foreach (string aminoacid in pb)
            //    {
            //        ListBox aa = new ListBox();
            //        ListBoxItem aaName = new ListBoxItem() { Content = aminoacid };


            //        Image img = new Image();
            //        BitmapImage pic = new BitmapImage();
            //        pic.BeginInit();
            //        pic.UriSource = new Uri("pack://application:,,,/DNAEngine.GUI;component/AminoAcids/" + aminoacid + ".png");
            //        pic.EndInit();

            //        img.Source = pic;

            //        ListBoxItem aaPic = new ListBoxItem() { Content = img };

            //        aa.Items.Add(aaName);
            //        aa.Items.Add(aaPic);

            //        PeptineBonds_ListBox.Items.Add(aa);
            //    }
            //}
        }
    }
}
