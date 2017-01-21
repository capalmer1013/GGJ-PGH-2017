import socket

UDP_IP = "127.0.0.1"
UDP_PORT = 5010
ANY = '0.0.0.0'
MULTICAST_IP = '224.0.0.1'

sock = socket.socket(socket.AF_INET,  # Internet
                     socket.SOCK_DGRAM)  # UDP
sock.bind((ANY, UDP_PORT))

while True:
    data, addr = sock.recvfrom(1024)  # buffer size is 1024 bytes
    print("received message:", data)
    print("addr:", addr)