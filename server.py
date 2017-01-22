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
player1Theta = "PLAYER1_THETA"
player2Theta = "PLAYER2_THETA"
ball = "BALL"
otherObjects = "OTHER_OBJECTS"
ballRotation = "BALL_ROTATION"


def debug(outString):
    outString = [str(i) for i in outString]  # '%.3f' % i if type(i) == float else
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
        self.loopsPerSec = 20.0
        self.loop = True
        # players (x, y, z, theta)
        # ball(x, y, z, (x, y, z)vector3)
        self.gameModel = {
            player1:      (0.0, 0.75, 10.0,),
            player1Theta: 0.0,
            player2:      (0.0, 0.75, -10.0,),
            player2Theta: 0.0,
            ball:         (0.0, 0.5, 0.0,),
            ballRotation: (10.0, 10.0, 10.0,),
            otherObjects: {},
            }

        self.tempModel = {
            player1: (0.0, 0.0, 0.0),
            player1Theta: 0.0,
            player2: (0.0, 0.0, 0.0),
            player2Theta: 0.0,
            ball: (0.0, 0.0, 0.0,),
            ballRotation: (0.0, 0.0, 0.0),
            otherObjects: {},
            }
        self.lastBallPos = None

    def run(self):
        while self.loop:
            time.sleep(1.0/self.loopsPerSec)
            self.sendMulticast()
            self.alertPlayers()
            z = self.gameModel[player2][2]
            if self.gameModel[player2][2] < 0:
                self.gameModel[player2] = (0.0, 0.75, z+0.1)
            if self.gameModel[player2][2] > 10:
                self.gameModel[player2] = (0.0, 0.75, z-0.1)

    def alertPlayers(self):
        if self.player1IP:
            self.multicastSock.sendto(struct.pack('I', 1),
                                      (self.player1IP, SUBSCRIBE_MULTICAST_PORT))

        if self.player2IP:
            self.multicastSock.sendto(struct.pack('I', 2), (self.player2IP, SUBSCRIBE_MULTICAST_PORT))

        for spectatorIP in self.spectatorList:
            self.multicastSock.sendto(struct.pack('I', 0), (spectatorIP, SUBSCRIBE_MULTICAST_PORT))

    def addPlayer(self, addr):
        if self.player1IP == addr[0]:
            return

        if self.player1IP and self.player2IP:
            self.spectatorList.append(addr[0])

        else:
            if not self.player1IP and addr[0] != self.player1IP:
                self.player1IP = addr[0]

            elif not self.player2IP and addr[0] != self.player2IP:
                self.player2IP = addr[0]

        # this logic blows
        # I was right

    def sendMulticast(self):
        self.multicastSock.sendto(self.packGameModel(), (SUBSCRIBE_MULTICAST_ADDR, SUBSCRIBE_MULTICAST_PORT))
        #debug(["player1:", self.gameModel[player1]])
        # debug(["ball:", self.gameModel[ball]])

    def packGameModel(self):
        jsonOtherObjects = json.dumps(self.gameModel[otherObjects])
        result = struct.pack('I', 1337)
        result += ''.join([struct.pack('f', i) for i in self.gameModel[player1]])
        result += struct.pack('f', self.gameModel[player1Theta])
        result += ''.join([struct.pack('f', i) for i in self.gameModel[player2]])
        result += struct.pack('f', self.gameModel[player2Theta])
        result += ''.join([struct.pack('f', i) for i in self.gameModel[ball]])
        result += ''.join([struct.pack('f', i) for i in self.gameModel[ballRotation]])
        result += struct.pack('I', len(jsonOtherObjects))
        result += jsonOtherObjects
        return result

    def unpackGameModel(self, messageBytes, addr):
        offset = 0
        # print "data:", messageBytes
        self.tempModel[player1] = struct.unpack_from('fff', messageBytes, offset)
        offset += struct.calcsize('fff')
        self.tempModel[player1Theta] = struct.unpack_from('f', messageBytes, offset)
        offset += struct.calcsize('f')

        self.tempModel[player2] = struct.unpack_from('fff', messageBytes, offset)
        offset += struct.calcsize('fff')
        self.tempModel[player2Theta] = struct.unpack_from('f', messageBytes, offset)
        offset += struct.calcsize('f')

        self.tempModel[ball] = struct.unpack_from('fff', messageBytes, offset)
        offset += struct.calcsize('fff')
        self.tempModel[ballRotation] = struct.unpack_from('fff', messageBytes, offset)
        offset += struct.calcsize('fff')

        if addr == self.player1IP:
            self.gameModel[player1] = self.tempModel[player1]
            # add player theta

        if addr == self.player2IP:
            self.gameModel[player2] = self.tempModel[player2]

        self.gameModel[ball] = self.tempModel[ball]
        self.gameModel[ballRotation] = self.tempModel[ballRotation]
        # debug(["ball:", self.tempModel[ball]])

    def updateModel(self):
        # print "updating game model"
        # self.gameModel = self.tempModel
        pass


def main():
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)  # listening socket
    sock.bind((ANY, GAME_SERVER_PORT))

    game = gameServer()
    game.start()

    try:
        while True:
            data, addr = sock.recvfrom(1024)  # buffer size is 1024 bytes
            # print("data:", data)
            # print (addr)

            if data == "suh dude":
                game.addPlayer(addr)
                print("player1:", game.player1IP)
                print("player2:", game.player2IP)
                print("spectatorList:", game.spectatorList)

            else:

                game.unpackGameModel(data, addr[0])
                print("data:", data)
                game.updateModel()

    except:  # Exception as e:
        game.loop = False
        # print("exception", e)
        exit()

if __name__ == "__main__":
    main()
