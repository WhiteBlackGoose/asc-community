import socket
import sympy
from sympy import *
from ascmath import *

def console_log(*text):
    print(*text)

encoding_ = "utf8"

sock_ = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

server_address = ('localhost', 7000)
sock_.bind(server_address)
sock_.listen(1)
Error_ = "E"
OK_ = "O"

syms_ = []

e = 2.718281828459045235360287

while True:
    conn_, addr_ = sock_.accept()
    try:
        while True:
            received_data = conn_.recv(8192)
            console_log("Received data of size", len(received_data))
            if not received_data:
                console_log("Connection closed")
                break
            req_ = received_data.decode(encoding_)    # convert to string (Python 3 only)
            resp_ = ""
            for line_code in req_.split("\n"):
                if len(line_code) == 0:
                    continue
                cmd_, line_ = line_code[0].rstrip('\x00'), line_code[1:].rstrip('\x00')
                try:
                    if cmd_ == "S":
                        vars()[line_] = Symbol(line_)
                        resp_ = OK_
                        syms_.append(line_)
                    elif cmd_ == "E":
                        evres_ = eval(line_)
                        resp_ = OK_ + str(evres_)
                    elif cmd_ == "Q":
                        exit()
                    else:
                        raise Exception("Symbol " + cmd_ + " unresolved");
                    console_log("OK on " + cmd_)
                except Exception as e_:
                    resp_ = Error_
                    console_log("Error:", e_)
            very_long_name_for_variable = bytes(resp_, encoding=encoding_)
            conn_.sendall(very_long_name_for_variable)
            console_log("Responded with data of size", len(very_long_name_for_variable))
    except Exception as e_:
        console_log("Error:", e_)
    for vr_ in syms_:
        if vr_ in vars():
            del vars()[vr_]
    syms_ = []

    conn_.close()
