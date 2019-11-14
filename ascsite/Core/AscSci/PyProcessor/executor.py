import socket
import sympy
from sympy import *
from ascmath import *

def console_log(*text):
    print(*text)

encoding = "utf8"

sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

server_address = ('localhost', 7000)
sock.bind(server_address)
sock.listen(1)
Error = "E"
OK = "O"

syms = []

while True:
    conn, addr = sock.accept()
    try:
        while True:
            data = conn.recv(8192)
            console_log("Received data of size", len(data))
            if not data:
                console_log("Connection closed")
                break
            req = data.decode(encoding)    # convert to string (Python 3 only)
            resp = ""
            for linecode in req.split("\n"):
                if len(linecode) == 0:
                    continue
                cmd, line = linecode[0].rstrip('\x00'), linecode[1:].rstrip('\x00')
                try:
                    if cmd == "S":
                        vars()[line] = Symbol(line)
                        resp = OK
                        syms.append(line)
                    elif cmd == "E":
                        evres = eval(line)
                        resp = OK + str(evres)
                    else:
                        raise Exception("Symbol " + cmd + " unresolved");
                    console_log("OK on " + cmd)
                except Exception as e:
                    resp = Error
                    console_log("Error:", e)
            b = bytes(resp, encoding=encoding)
            conn.sendall(b)
            console_log("Responded with data of size", len(b))
    except Exception as e:
        console_log("Error:", e)
    for vr in syms:
        if vr in vars():
            del vars()[vr]

    conn.close()
