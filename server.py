import threading
import socket
import time
import struct
import json
# the first 2 devices to subscribe are players 1 and 2
# everyone after that is an observer
# can set it up to have the next observer vs the winner of previous match

# command Update locations:
#   - character, ball, other objects

DEBUG = True
SUBSCRIBE_MULTICAST_ADDR = '224.0.0.1'
SUBSCRIBE_MULTICAST_PORT = 5010  # sending port

GAME_SERVER_PORT = 5009  # Listening Port
ANY = '0.0.0.0'

player1 = "PLAYER1"
player2 = "PLAYER2"
ball = "BALL"
otherObjects = "OTHER_OBJECTS"
ballRotation = "BALL_ROTATION"

def debug(outString):
    outString = [str(i) for i in outString]
    outString = ' '.join(outString)
    if DEBUG:
        print(outString)


class gameServer(threading.Thread):
    def __init__(self):
        threading.Thread.__init__(self)
        self.player1IP = None
        self.player2IP = None
        self.spectatorList = []
        self.multicastSock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM, socket.IPPROTO_UDP)
        self.multicastSock.setsockopt(socket.IPPROTO_IP, socket.IP_MULTICAST_TTL, 255)
        self.loopsPerSec = 1.0  # 30.0
        self.loop = True
        # players (x, y, z, theta)
        # ball(x, y, z, (x, y, z)vector3)
        self.gameModel = {
            player1: (0.0, 0.0, 0.0),
            player2: (0.0, 0.0, 0.0),
            ball: (0.0, 0.0, 0.0,),
            ballRotation: (0.0, 0.0, 0.0),  
            otherObjects: {},
            }

    def run(self):
        while self.loop:
            time.sleep(1.0/self.loopsPerSec)
            self.sendMulticast()

    def addPlayer(self, addr):
        if self.player1IP and self.player2IP:
            self.spectatorList.append(addr)
        else:
            if not self.player1IP:
                self.player1IP = addr
            elif not self.player2IP:
                self.player2IP = addr
        # this logic blows

    def sendMulticast(self):
        self.multicastSock.sendto(self.packGameModel(), (SUBSCRIBE_MULTICAST_ADDR, SUBSCRIBE_MULTICAST_PORT))

    def packGameModel(self):
        jsonOtherObjects = json.dumps(self.gameModel[otherObjects])
        result = struct.pack('I', 1337)
        result += ''.join([struct.pack('f', i) for i in self.gameModel[player1]])
        result += ''.join([struct.pack('f', i) for i in self.gameModel[player2]])
        result += ''.join([struct.pack('f', i) for i in self.gameModel[ball]])
        result += ''.join([struct.pack('f', i) for i in self.gameModel[ballRotation]])
        result += struct.pack('I', len(jsonOtherObjects))
        result += jsonOtherObjects
        return result

    def updateModel(self, modelMessage):
        pass


def main():
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)  # listening socket
    sock.bind((ANY, GAME_SERVER_PORT))

    game = gameServer()
    game.start()
    try:
        while True:
            data, addr = sock.recvfrom(1024)  # buffer size is 1024 bytes

            if data == "suh dude":
                game.addPlayer(addr)
            else:
                game.updateModel(data)

            debug(["received message:", data, addr])

    except KeyboardInterrupt:
        game.loop = False
        exit()

if __name__ == "__main__":
    main()
