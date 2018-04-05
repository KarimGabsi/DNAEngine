import os
def ReadByteToBits(data, num):
    base = int(num/8)
    shift = num % 8
    return (data[base] & (1<<shift)) >> shift 

class Stream():  
    X = []
    Y = []

def GetFeed():
    import pandas as pd
    df = pd.DataFrame(Stream.X)
    return len(df.columns)

def GetOutputNodes():
    return 1 #Either Black or White    

def ReadFolder(filepath):
    Stream.X = []
    Stream.Y = []
    Specimens = []
    for specimen in os.listdir(filepath):
        Specimens.append(specimen)

    for specimen in Specimens:
        ChromosomeFiles = []
        for file in os.listdir("{0}/{1}".format(filepath, specimen)):
            if ".dna" in file:
                ChromosomeFiles.append(file)
                
        for file in os.listdir("{0}/{1}".format(filepath, specimen)):
            if "Result" in file:
                for i in range(0, len(ChromosomeFiles)):
                    if(file.split('.')[1] == "White"):
                        Stream.Y.append(1)
                    else:
                        Stream.Y.append(0)
        Chromosomes = []
        ChromosomeBytes = []
        
        for i in range(1, (len(ChromosomeFiles) + 1)):
            with open("{0}/{1}/Chromosome{2}.dna".format(filepath, specimen, i), "rb") as f:
                byte = f.read(1)
                while byte != b"":
                    # Do stuff with byte.
                    for i in range(len(byte)*8):
                        ChromosomeBytes.append(ReadByteToBits(byte, i))
                    byte = f.read(1)
                Chromosomes.append(ChromosomeBytes)
                Stream.X.append(ChromosomeBytes)
                ChromosomeBytes = []