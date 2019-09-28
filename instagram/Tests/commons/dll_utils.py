import sys
import os
from os import path
import clr

common_tools_path = path.join(os.path.dirname(os.path.abspath(__file__)),"../artifacts", "CommonTools.dll")

def get_wrapper(login, password):
    sys.path.append(common_tools_path)
    clr.AddReference(common_tools_path)
    from CommonTools import PyWrapper
    return PyWrapper(login, password)