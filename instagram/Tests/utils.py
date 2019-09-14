from json import load

def get_config():
    pass

def read_json(path):
    with open(path, "r") as f:
        return load(f)