import sys
import os
from os import path
import clr


common_tools_path = path.join(os.path.dirname(os.path.abspath(__file__)),"artifacts", "CommonTools.dll")

def get_common_tools():
    print("Initializing CommonTools dll")
    sys.path.append(common_tools_path)
    clr.AddReference(common_tools_path)
    from CommonTools import PyWrapper
    return PyWrapper()




tools = get_common_tools()

print(tools.ParseMessage())