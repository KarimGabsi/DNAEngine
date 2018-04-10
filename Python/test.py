# -*- coding: utf-8 -*-
import sys
intSize = sys.getsizeof(int())
print (intSize)
bitstoint  = int('1111111111111111', 2)
print (bitstoint)
print (sys.maxsize)
maxsizeBits = "{0:b}".format(sys.maxsize)
print (len(maxsizeBits) +1)