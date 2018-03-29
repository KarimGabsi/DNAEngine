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
            string ManualDNASequenceFile = "ManualDNASequence.dna";


            Console.WriteLine("DNA Engine Starting up...");
            DNAMachine DNAMachine = new DNAMachine();
            Console.WriteLine("DNA Engine started.");

            //Console.WriteLine("Generating  and loading DNA Data...");
            //DNAMachine.GenerateAndLoadDNASequence(filepath + @"\" + GeneratedDNASequenceFile);

            Console.WriteLine("Loading DNA Data...");
            DNAMachine.LoadDNASequence(filepath + @"\" + DNASequenceFile);

            //DNAMachine.DNAData = DNAMachine.StringToDNA("TTTCAGTATCTT", filepath + @"\" + ManualDNASequenceFile);

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
