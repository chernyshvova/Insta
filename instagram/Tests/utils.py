from json import load


def read_json(path):
    with open(path, "r") as f:
        return load(f)