# -*- coding: utf-8 -*-
def main():
    import sys
    maxsize = "{0:b}".format(sys.maxsize)
    maxsize = len(maxsize) +1
    print ("Current system: {}-bit".format(maxsize))
    
    #If on windows script will run very slowly, if on linux script will utilze all cpus
    os = sys.platform
    print(os)
    
    import DNA_Analyzer
    filepath = "./DNAs"
    #precision offset must be between 2 and 64-bit, 32 if on 32-bitsystem
    #example using 25 bytes as dna string, lets use precisionoffset of 8
    precisionoffset = 8    
    dna_analyzer = DNA_Analyzer.DNA_Analyzer(precisionoffset,os)
    dna_analyzer.Load(filepath)
    print(dna_analyzer.displayresult)
    
    #Read one specimen and test if is white or black
    specimen = "Mouse10"
    dna_analyzer.PredictOneSpecimen(filepath, specimen)
    print(dna_analyzer.displayresult)

if __name__ == "__main__": main() 