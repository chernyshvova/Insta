from enum import Enum

class STATUS_CODE(Enum):
    UNKNOWN = 998
    DEFAULT = 999
    S_OK = 0
    E_DATA_NOT_FOUND = 1
    E_EMAIL_PROVIDER_NOT_FOUND = 2
    E_INVALID_EMAIL = 3
    E_DB_CONNECTION = 4 
    E_DB_DATA_NOT_FOUND = 5
    E_DB_READING = 6
    E_DB_WRITING = 7 
    E_DB_CREATING = 8
    E_DB_EXECUTING = 9
    E_ACC_MEDIA_URL = 29
    E_ACC_SIGNIN = 30
    E_ACC_AVATAR = 31
    E_ACC_SETPOST = 32
    E_ACC_COMMENT = 33
    E_ACC_LIKE = 34
    E_ACC_FOLLOW = 35
    E_ACC_DIRECT = 36
    E_CHALLENGE = 40

def resolve_error_name(error_name):   
    if error_name == "UNKNOWN":
        return STATUS_CODE.UNKNOWN  
    if error_name == "DEFAULT":
        return STATUS_CODE.DEFAULT
    if error_name == "S_OK":
        return STATUS_CODE.S_OK
    if error_name == "E_DATA_NOT_FOUND":
        return STATUS_CODE.E_DATA_NOT_FOUND
    if error_name == "E_EMAIL_PROVIDER_NOT_FOUND":
        return STATUS_CODE.E_EMAIL_PROVIDER_NOT_FOUND
    if error_name == "E_INVALID_EMAIL":
        return STATUS_CODE.E_INVALID_EMAIL
    if error_name == "E_DB_DATA_NOT_FOUND":
        return STATUS_CODE.E_DB_DATA_NOT_FOUND    
    
    print("error {} is not handled".format(error_name))
    return error_name

def resolve_error_code(code):  
    if code == STATUS_CODE.UNKNOWN.value:
        return STATUS_CODE.UNKNOWN  
    if code == STATUS_CODE.DEFAULT.value:
        return STATUS_CODE.DEFAULT
    if code == STATUS_CODE.S_OK.value:
        return STATUS_CODE.S_OK
    if code == STATUS_CODE.E_DATA_NOT_FOUND.value:
        return STATUS_CODE.E_DATA_NOT_FOUND
    if code == STATUS_CODE.E_EMAIL_PROVIDER_NOT_FOUND.value:
        return STATUS_CODE.E_EMAIL_PROVIDER_NOT_FOUND
    if code == STATUS_CODE.E_INVALID_EMAIL.value:
        return STATUS_CODE.E_INVALID_EMAIL
    if code == STATUS_CODE.E_DB_DATA_NOT_FOUND.value:
        return STATUS_CODE.E_DB_DATA_NOT_FOUND
    
    print("error {} is not handled".format(code))
    return code

    