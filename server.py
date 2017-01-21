import threading
import socket
import time

# the first 2 devices to subscribe are players 1 and 2
# everyone after that is an observer
# can set it up to have the next observer vs the winner of previous match

# command Update locations:
#   - character, ball, other objects

SUBSCRIBE_MULTICAST_ADDR = '224.0.0.1'
SUBSCRIBE_MULTICAST_PORT = 5010  # sending port

GAME_SERVER_PORT = 5009  # Listening Port
ANY = '0.0.0.0'

player1 = "PLAYER1"
player2 = "PLAYER2"
ball = "BALL"

class subscribeServer(threading.Thread):
    def __init__(self):
        self.player1IP = None
        self.player2IP = None
        self.spectatorList = []
        self.loop = True

    def run(self):
        while self.loop:
            # receive any subscriptions
            pass

    def getSubscriber(self):
        # add subscriber to game

        pass


class gameServer(threading.Thread):
    def __init__(self):
        self.loopsPerSec = 30.0
        self.loop = True

    def run(self):
        while self.loop:
            time.sleep(1.0/self.loopsPerSec)
            pass

    def sendMulticast(self):
        pass

    def updateModel(self, newModel):
        pass


def main():
    model = {
        player1: None,
        player2: None,
        ball: None,
    }
    game = gameServer()
    subscribe = subscribeServer()
    try:
        while True:
            pass

    except KeyboardInterrupt:
        game.loop = False
        subscribe.loop = False
        exit()


if __name__ == "__main__":
    main()
