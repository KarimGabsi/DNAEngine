import keras
from keras.models import Sequential
from keras.layers import Dense

class DNA_Analyzer():
    def __init__(self, precisionoffset, os):
        self.precisionoffset = precisionoffset
        self.displayresult = ""
        self.classifier = None
        self.X = None
        self.Y = None
        self.feed = None
        self.output = None
        if(os.lower() == "linux"): 
            self.njobs = -1 
        else: 
            self.njobs = 1
        
    def dash(self):
        dash = "\n"
        for i in range(0, 100):
           dash += "="
        return dash
    
    def find_between(self, s, first, last):
        try:
            start = s.index( first ) + len( first )
            end = s.index( last, start )
            return s[start:end]
        except ValueError:
            return ""
        
    def Load(self, filepath):
        
        self.displayresult = ""
        self.displayresult += self.dash()
        
        import DNA_Reader
        dna_reader = DNA_Reader.DNA_Reader()
        
        #Read DNA Folder
        dna_reader.ReadAllSPecimenInFolder(filepath, self.precisionoffset)
        
        # X (DNA String in integer per precisionoffset-sector)
        self.X = dna_reader.GetX()
        # Y (results)
        self.Y = dna_reader.GetY()
        
        #Transform to numpy array
        import numpy as np
        self.X = np.array(self.X)
        self.Y = np.array(self.Y)
        
        #Feed(input): number of bits / precisionoffset
        self.feed = dna_reader.GetFeed()
        #Output: Either Black(0) or White(1), 1 output
        self.output = dna_reader.GetOutputNodes()
        
        self.displayresult += "\nDNA READER with precision offset: {}".format(self.precisionoffset)
        self.displayresult += "\nX (DNA) Shape: {}".format(self.X.shape)
        self.displayresult += "\nY (Result) Shape: {}".format(self.X.shape)
        self.displayresult += "\nFeed: {}".format(self.feed)
        self.displayresult += "\nOutput: {}".format(self.output)
        self.displayresult += self.dash()
                  
    def GetBestParameters(self, parameters):    
        
        self.displayresult += self.dash()
        self.displayresult += "\nBEST PARAMETER GENERATION"
        self.displayresult += "\nParameters to test: {}".format(parameters)
        #Generate best parameters (WARNING: takes a while...)
        bestparameters = self.GenerateBestParameterSet(self.X, self.Y, self.feed, self.output, parameters, self.njobs)
        self.displayresult += "\nParameters Result: {}".format(bestparameters)
        self.displayresult += self.dash()
        return bestparameters
    
    def GenerateANNWithBestParameters(self, bestparameters):
        
        self.displayresult += self.dash()
        self.displayresult += "\nGENERATE ANN USING BEST PARAMETERS"
        
        #Use best ANN
        bestOptimizer = self.find_between(bestparameters[0], "optimizer", "}")
        bestOptimizer = self.find_between(bestOptimizer, "': '", "'")
        optimizer = bestOptimizer
        self.displayresult  += "\nBest Optimizer: {}".format(optimizer)
        #Set batchsize
        bestbatchsize = self.find_between(bestparameters[0], "batch_size\':", ",")
        batchsize = int(bestbatchsize)
        self.displayresult += "\nBest Batchsize: {}".format(batchsize)
        #Set epochs
        bestepochs = self.find_between(bestparameters[0], "epochs\':", ",")
        epochs = int(bestepochs)
        self.displayresult  += "\nBest Epochs: {}".format(epochs)
        
        self.classifier = self.CreateANN(self.X,self.Y,self.feed, self.output, optimizer, batchsize, epochs) 
        #displayresult.append("Classifier: {}".format(classifier.get_config()))
        
        evaluation = self.EvaluateANN(self.X, self.Y, self.feed, self.output, optimizer, batchsize, epochs)
        self.displayresult += "\nEvaluation: {}".format(evaluation)
        self.displayresult += self.dash()
        
    def PredictOneSpecimen(self, filepath, specimen):
        
        self.displayresult += self.dash()
        self.displayresult += "\nPREDICTING ONE SPECIMEN"
        
        import DNA_Reader
        dna_reader = DNA_Reader.DNA_Reader()
        
        dna_reader.ReadOneSpecimen(filepath, specimen, self.precisionoffset)
        # X (DNA String in integer per precisionoffset-sector)
        X = dna_reader.GetX()
        # Y (results)
        Y = dna_reader.GetY()
        
        #Transform to numpy array
        import numpy as np
        X = np.array(X)
        Y = np.array(Y)
        
        #Feed(input): number of bits / precisionoffset
        feed = dna_reader.GetFeed()
        #Output: Either Black(0) or White(1), 1 output
        output = dna_reader.GetOutputNodes()
        
        self.displayresult += "\nDNA READER with precision offset: {}".format(self.precisionoffset)
        self.displayresult += "\nONE SPECIMEN"
        self.displayresult += "\nFilepath : {}".format(filepath)
        self.displayresult += "\nSpecimen: {}".format(specimen)
        self.displayresult += "\nX (DNA) Shape: {}".format(X.shape)
        self.displayresult += "\nY (Result) Shape: {}".format(X.shape)
        self.displayresult += "\nFeed: {}".format(feed)
        self.displayresult += "\nOutput: {}".format(output)
        
        new_prediction = self.classifier.predict(X)
        outcome = (new_prediction >= 0.5)
        if(outcome):
            self.displayresult += "\nMouse prediction is White ({})".format(new_prediction)
        else:
            self.displayresult += "\nMouse prediction is Black ({})".format(new_prediction)
        
        self.displayresult += self.dash()
        
        self.displayresult += self.dash()
        self.displayresult += "\nShow Weights"

        for layer in self.classifier.layers:
            self.displayresult += str(layer.get_weights())
        self.displayresult += "\nLayers: {}".format(len(self.classifier.layers))
    
    def GenerateBestParameterSet(self, X, Y, feed, output, parameters, njobs):
        import DNA_ANN
        dna_ann = DNA_ANN.DNA_ANN(feed, output, None)
        bestparameters = dna_ann.GenerateBestParameterSet(X, Y, parameters, njobs)
        #Generate best ANN using the parameters
        return bestparameters
    
    def CreateANN(self, X, Y, feed, output, optimizer, batchsize, epochs):  
        import DNA_ANN
        dna_ann = DNA_ANN.DNA_ANN(feed, output, optimizer)
        return dna_ann.CreateANN(X, Y, batchsize, epochs)
    
    def EvaluateANN(self,X,Y,feed, output, optimizer, batchsize, epochs):
        import DNA_ANN
        dna_ann = DNA_ANN.DNA_ANN(feed, output, optimizer)
        return dna_ann.EvaluateANN(X, Y, batchsize, epochs)
