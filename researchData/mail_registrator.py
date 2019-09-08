import requests
from json import load

import urllib3
urllib3.disable_warnings()




s_user_url = "https://registrace.seznam.cz/api/v1/user"
s_init_url = "https://registrace.seznam.cz/?return_url=https%3A%2F%2Femail.seznam.cz%2F&service=email"
s_check_user_url = "https://registrace.seznam.cz/api/v1/username-check/test12345621321%40seznam.cz"
s_check_password_url = "https://registrace.seznam.cz/api/v1/password-strength"

headers = {"Host" : "registrace.seznam.cz",
            "Accept": "application/json;q=0.9,*/*;q=0.8",
            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36",
            "Sec-Fetch-Mode": "cors",
            "Content-type": "application/json",
            "Origin" : "https://registrace.seznam.cz",
            "Sec-Fetch-Site" : "same-origin",
            "Referer" : "https://registrace.seznam.cz/?service=email&return_url=https%3A%2F%2Femail.seznam.cz%2F",
            "Accept-Encoding" : "gzip, deflate, br",
            "Accept-Language" : "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7",   
            }

init_headers = {
            "Host" : "registrace.seznam.cz",
            "Connection" :"keep-alive",
            "Upgrade-Insecure-Requests" : "1",
            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36",
            "Sec-Fetch-Mode" : "navigate",
            "Accept": "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3",
            "Sec-Fetch-Site" : "none",
            "Accept-Encoding" : "gzip, deflate, br",
            "Accept-Language" : "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7"
}

def get_body():
    with open("registerData.json", "r") as f:
        data = load(f)
    return data


new_login = "test123456213219@seznam.cz"

data = get_body()
data["username"] = new_login
data["password"] = "BamBam753951"


with requests.session() as s:
    response = s.get(verify = False, headers = init_headers, url = s_init_url)
    
    print("filrst request = {}".format(response.content))
    response = s.get(verify = False, headers = init_headers, url = s_check_user_url)

    print("check user = {}".format(response))

    response = s.post(verify = False, headers = init_headers, url = s_check_password_url)
    print("check password = {}".format(response))
    respons = s.post(verify = False, url = s_user_url, headers = headers, json= data)

print(respons.content)