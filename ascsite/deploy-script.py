import socket

IPADDRESS = '127.0.0.1'
PORT = 5000

conn = socket.socket()
conn.connect( (IPADDRESS, PORT) )
conn.send('build')
conn.close()
