import keras
from keras.models import Sequential
from keras.layers import Dense
#Libraries
def BuildANN(X,Y, totaldata):     
    #Splitting the dataset into Training set and test set
    from sklearn.model_selection import train_test_split
    X_train, X_test, Y_train, Y_test = train_test_split(X, Y, test_size = 0.2, random_state = 0)
     
    #Initialize ANN
    classifier = Sequential()  
    
    #Adding the input layer and first hidden layer
    classifier.add(Dense(units = (totaldata * 4), kernel_initializer = 'uniform', activation = 'relu', input_dim = totaldata))  
    #Add second hidden layer
    classifier.add(Dense(units = (totaldata * 3), kernel_initializer = 'uniform', activation = 'relu')) 
    #Add third hidden layer
    classifier.add(Dense(units = (totaldata * 2), kernel_initializer = 'uniform', activation = 'relu')) 
    #Adding the output layer
    classifier.add(Dense(units = 1, kernel_initializer = 'uniform', activation = 'sigmoid')) 
    
    #Compile ANN
    #classifier.compile(optimizer = 'adam', loss = 'binary_crossentropy', metrics = ['accuracy']) 
    classifier.compile(optimizer = 'adam', loss = 'binary_crossentropy', metrics = ['accuracy']) 
    
    #Fitting the ANN to the training set
    classifier.fit(X_train, Y_train, batch_size = 10, epochs = 100)
    
    #Predicting the Test set results
    y_pred = classifier.predict(X_test)
    y_pred = (y_pred > 0.5)
    
    #Making the confusion matrix
    from sklearn.metrics import confusion_matrix
    con = confusion_matrix(Y_test, y_pred)
    print("Con: {0}".format(con))
    
    #Evaluating the ANN
    from keras.wrappers.scikit_learn import KerasClassifier
    from sklearn.model_selection import cross_val_score
    
    #classifier = KerasClassifier(build_fn = BuildClassifier, batch_size = 10, epochs = 100)
    #accuracies = cross_val_score(estimator = classifier, X = X_train, y = Y_train, cv=10, n_jobs=-1)
    
    #Improving the ANN
    #Tuning 
    # -*- coding: utf-8 -*-
def BuildClassifier():
    classifier = Sequential() 
    classifier.add(Dense(units = 10, kernel_initializer = 'uniform', activation = 'relu', input_dim = 40))  
    classifier.add(Dense(units = 10, kernel_initializer = 'uniform', activation = 'relu')) 
    classifier.add(Dense(units = 10, kernel_initializer = 'uniform', activation = 'relu')) 
    classifier.add(Dense(units = 1, kernel_initializer = 'uniform', activation = 'sigmoid'))
    classifier.compile(optimizer = 'adam', loss = 'sparse_categorical_crossentropy', metrics = ['accuracy']) 
    return classifier
