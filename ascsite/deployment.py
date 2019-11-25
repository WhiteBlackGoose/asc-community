import socket
import os

# config
IPADDRESS = '127.0.0.1'
PORT = 5000
PROJECT_DIRECTORY = r'C:\Users\Momo\source\repos\Angourisoft\asc-community\ascsite'
EXECUTABLE_PATH = r'C:\Users\Momo\source\repos\Angourisoft\asc-community\ascsite\bin\Release\netcoreapp3.0\AscSite.exe'

# win commands
# os.system('cmd dotnet build ' + PROJECT_DIRECTORY + r'\ascsite.sln -c Release')
# os.system(EXECUTABLE_PATH)

def console_log(*text):
    print(*text)

encoding_ = "utf8"

sock_ = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

server_address = (IPADDRESS, PORT)
sock_.bind(server_address)
sock_.listen(1)

while True:
    conn_, addr_ = sock_.accept()
    try:
        while True:
            received_data = conn_.recv(1024)
            if not received_data:
                console_log("Connection closed")
                break
            req_ = received_data.decode(encoding_)    # convert to string (Python 3 only)
            if req == 'build':
                console_log('build started...')
                os.system('cmd dotnet build ' + PROJECT_DIRECTORY + r'\ascsite.sln -c Release')
                os.system(EXECUTABLE_PATH)
                console_log('build succeeded')
    except Exception as e_:
        console_log("Error:", e_)
    conn_.close()
