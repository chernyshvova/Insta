import sys
import os
from os import path
import clr

common_tools_path = path.join(os.path.dirname(os.path.abspath(__file__)),"../artifacts", "CommonTools.dll")
insta_tools_path = path.join(os.path.dirname(os.path.abspath(__file__)),"../artifacts", "InstarmCore.dll")
artifacts_path = path.join(os.path.dirname(os.path.abspath(__file__)),"../artifacts")

def get_wrapper(dll_name):
    dll_path = path.join(artifacts_path, dll_name)
    if not path.exists(dll_path):
        print("{} is not found".format(dll_path))
    
    sys.path.append(common_tools_path)
    clr.AddReference(common_tools_path)
    from CommonTools import PyWrapper
    return PyWrapper()
    
def get_insta_wrapper(dll_name):
    dll_path = path.join(artifacts_path, dll_name)
    if not path.exists(dll_path):
        print("{} is not found".format(dll_path))
    
    sys.path.append(insta_tools_path)
    clr.AddReference(insta_tools_path)
    from InstarmCore.Utils import PythonWrapper
    return PythonWrapper()
    