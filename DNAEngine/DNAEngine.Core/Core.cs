using DNAEngine.GUI;
using DNAEngine.Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNAEngine.Core
{
    class Core
    {
        [STAThread]
        static void Main(string[] args)
        {
            string filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string GeneratedDNASequenceFile = "GenderatedDNASequence.dna";
            string DNASequenceFile = "DNASequence.dna";
            string Molecule1 = "Molecule1.dna";
            string Molecule2 = "Molecule2.dna";
            string Molecule3 = "Molecule3.dna";
            string Molecule4 = "Molecule4.dna";
            string Molecule5 = "Molecule5.dna";
            string Molecule6 = "Molecule6.dna";
            string Molecule7 = "Molecule7.dna";
            string Molecule8 = "Molecule8.dna";
            string Molecule9 = "Molecule9.dna";
            string Molecule10 = "Molecule10.dna";


            Console.WriteLine("DNA Engine Starting up...");
            DNAMachine DNAMachine = new DNAMachine();
            Console.WriteLine("DNA Engine started.");

            //Console.WriteLine("Generating  and loading DNA Data...");
            //DNAMachine.GenerateAndLoadDNASequence(filepath + @"\" + GeneratedDNASequenceFile);

            //Console.WriteLine("Loading DNA Data...");
            //DNAMachine.LoadDNASequence(filepath + @"\" + DNASequenceFile);

            DNAMachine.DNAData = DNAMachine.StringToDNA("ATCGTTATACTT", filepath + @"\" + Molecule1);
            DNAMachine.DNAData = DNAMachine.StringToDNA("ATCGTTGGACTT", filepath + @"\" + Molecule2);
            DNAMachine.DNAData = DNAMachine.StringToDNA("ATCGTCCCCCTT", filepath + @"\" + Molecule3);
            DNAMachine.DNAData = DNAMachine.StringToDNA("TTTGTTGGACTT", filepath + @"\" + Molecule4);
            DNAMachine.DNAData = DNAMachine.StringToDNA("ATCAAAGGACTT", filepath + @"\" + Molecule5);
            DNAMachine.DNAData = DNAMachine.StringToDNA("ATCGTTGATCGT", filepath + @"\" + Molecule6);
            DNAMachine.DNAData = DNAMachine.StringToDNA("ATCGGGGGACTT", filepath + @"\" + Molecule7);
            DNAMachine.DNAData = DNAMachine.StringToDNA("ATCGTTTTATTT", filepath + @"\" + Molecule8);
            DNAMachine.DNAData = DNAMachine.StringToDNA("ACGCGTGGACTT", filepath + @"\" + Molecule9);
            DNAMachine.DNAData = DNAMachine.StringToDNA("ATCATCTCGATT", filepath + @"\" + Molecule10);


            Console.WriteLine("DNA Data:");
            Console.WriteLine(DNAMachine.Read(0, 20, Language.DNA) + "...");

            Console.WriteLine("MRNA Data:");
            Console.WriteLine(DNAMachine.Read(0, 20, Language.MRNA) + "...");

            Console.WriteLine("TRNA Data:");
            Console.WriteLine(DNAMachine.Read(0, 20, Language.TRNA) + "...");

            Console.WriteLine("Amino Acids:");
            Console.WriteLine(string.Join(" - ", DNAMachine.ReadAminoAcids(0, 20)));

            Console.WriteLine("Peptine Bonds:");
            foreach (List<string> pb in DNAMachine.ReadPeptineBonds(0, 20))
            {
                Console.WriteLine(string.Join(" - ", pb));
            }

            var app = new App();
            app.Run(new DNAVisual(DNAMachine));
        }
    }
}
