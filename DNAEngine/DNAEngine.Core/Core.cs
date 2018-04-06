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
            string Mouse1Chromosome1 = "Chromosome1.dna";
            string Mouse1Chromosome2 = "Chromosome2.dna";
            string Mouse1Chromosome3 = "Chromosome3.dna";
            string Mouse1Chromosome4 = "Chromosome4.dna";
            string Mouse1Chromosome5 = "Chromosome5.dna";

            string Mouse2Chromosome1 = "Chromosome1.dna";
            string Mouse2Chromosome2 = "Chromosome2.dna";
            string Mouse2Chromosome3 = "Chromosome3.dna";
            string Mouse2Chromosome4 = "Chromosome4.dna";
            string Mouse2Chromosome5 = "Chromosome5.dna";
            Console.WriteLine("DNA Engine Starting up...");
            DNAMachine DNAMachine = new DNAMachine();
            Console.WriteLine("DNA Engine started.");

            //Console.WriteLine("Generating  and loading DNA Data...");
            DNAMachine.GenerateAndLoadDNASequence(filepath + @"\" + GeneratedDNASequenceFile);

            //Console.WriteLine("Loading DNA Data...");
            //DNAMachine.LoadDNASequence(filepath + @"\" + DNASequenceFile);

            //MAKE SURE YOU HAVE Mouse1 and Mouse2 folder in my documents
            DNAMachine.StringToDNA("AAAATTTTCCCC", filepath + @"\Mouse1\" + Mouse1Chromosome1);
            DNAMachine.StringToDNA("GGGGATATCGCG", filepath + @"\Mouse1\" + Mouse1Chromosome2);
            DNAMachine.StringToDNA("CCAATTGGCCGG", filepath + @"\Mouse1\" + Mouse1Chromosome3);
            DNAMachine.StringToDNA("AAAAAAAAAAAA", filepath + @"\Mouse1\" + Mouse1Chromosome4);
            DNAMachine.StringToDNA("TTCCAAAAGGCC", filepath + @"\Mouse1\" + Mouse1Chromosome5);

            DNAMachine.StringToDNA("AAAATTTTCCCC", filepath + @"\Mouse2\" + Mouse1Chromosome1);
            DNAMachine.StringToDNA("GGGGATATCGCG", filepath + @"\Mouse2\" + Mouse1Chromosome2);
            DNAMachine.StringToDNA("CCAATTGGCCGG", filepath + @"\Mouse2\" + Mouse1Chromosome3);
            DNAMachine.StringToDNA("AAAATTTTAAAA", filepath + @"\Mouse2\" + Mouse1Chromosome4);
            DNAMachine.StringToDNA("TTCCAAAAGGCC", filepath + @"\Mouse2\" + Mouse1Chromosome5);

            //DNAMachine.DNAData = DNAMachine.StringToDNA("ATCGTTGGACTT", filepath + @"\" + Molecule2);
            //DNAMachine.DNAData = DNAMachine.StringToDNA("ATCGTCCCCCTT", filepath + @"\" + Molecule3);
            //DNAMachine.DNAData = DNAMachine.StringToDNA("TTTGTTGGACTT", filepath + @"\" + Molecule4);
            //DNAMachine.DNAData = DNAMachine.StringToDNA("ATCAAAGGACTT", filepath + @"\" + Molecule5);
            //DNAMachine.DNAData = DNAMachine.StringToDNA("ATCGTTGATCGT", filepath + @"\" + Molecule6);
            //DNAMachine.DNAData = DNAMachine.StringToDNA("ATCGGGGGACTT", filepath + @"\" + Molecule7);
            //DNAMachine.DNAData = DNAMachine.StringToDNA("ATCGTTTTATTT", filepath + @"\" + Molecule8);
            //DNAMachine.DNAData = DNAMachine.StringToDNA("ACGCGTGGACTT", filepath + @"\" + Molecule9);
            //DNAMachine.DNAData = DNAMachine.StringToDNA("ATCATCTCGATT", filepath + @"\" + Molecule10);


            //Console.WriteLine("DNA Data:");
            //Console.WriteLine(DNAMachine.Read(0, 20, Language.DNA) + "...");

            //Console.WriteLine("MRNA Data:");
            //Console.WriteLine(DNAMachine.Read(0, 20, Language.MRNA) + "...");

            //Console.WriteLine("TRNA Data:");
            //Console.WriteLine(DNAMachine.Read(0, 20, Language.TRNA) + "...");

            //Console.WriteLine("Amino Acids:");
            //Console.WriteLine(string.Join(" - ", DNAMachine.ReadAminoAcids(0, 20)));

            //Console.WriteLine("Peptine Bonds:");
            //foreach (List<string> pb in DNAMachine.ReadPeptineBonds(0, 20))
            //{
            //    Console.WriteLine(string.Join(" - ", pb));
            //}

            var app = new App();
            app.Run(new DNAVisual(DNAMachine));
        }
    }
}
