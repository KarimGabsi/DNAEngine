import keras
from keras.models import Sequential
from keras.layers import Dense

def ReadByteToBits(data, num):
    base = int(num/8)
    shift = num % 8
    return (data[base] & (1<<shift)) >> shift   

def main():
    import random
    import numpy as np
    import os
    
    filePath = "./DNAs"
    Specimens = []
    for specimen in os.listdir(filePath):
        Specimens.append(specimen)
        
    X = []
    Y = []
    for specimen in Specimens:
        ChromosomeFiles = []
        for file in os.listdir("{0}/{1}".format(filePath, specimen)):
            if ".dna" in file:
                ChromosomeFiles.append(file)
                
        for file in os.listdir("{0}/{1}".format(filePath, specimen)):
            if "Result" in file:
                for i in range(0, len(ChromosomeFiles)):
                    if(file.split('.')[1] == "White"):
                        Y.append(1)
                    else:
                        Y.append(0)
        Chromosomes = []
        ChromosomeBytes = []
        
        for i in range(1, (len(ChromosomeFiles) + 1)):
            with open("{0}/{1}/Chromosome{2}.dna".format(filePath, specimen, i), "rb") as f:
                byte = f.read(1)
                while byte != b"":
                    # Do stuff with byte.
                    for i in range(len(byte)*8):
                        ChromosomeBytes.append(ReadByteToBits(byte, i))
                    #ChromosomeBytes.append(byte)
                    byte = f.read(1)
                Chromosomes.append(ChromosomeBytes)
                X.append(ChromosomeBytes)
                ChromosomeBytes = []
    
    X = np.array(X)
    Y = np.array(Y)
    import pandas as pd
    df = pd.DataFrame(X)
    totaldata = len(df.columns)
    
    import DNA_ANN as dna_ann
    dna_ann.BuildANN(X,Y,totaldata)

if __name__ == "__main__":
    main()       
