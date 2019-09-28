from json import load

class Account:
    def __init__(self, tag, path):
        with open(path, "r") as f:
            data = load(f)

        for acc in data["accounts"]:
            if acc["tag"] == tag:
                self.login = acc["login"]
                self.password = acc["password"]