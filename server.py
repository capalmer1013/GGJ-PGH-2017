import threading

# the first 2 devices to subscribe are players 1 and 2
# everyone after that is an observer
# can set it up to have the next observer vs the winner of previous match

class subscribeServer(threading.Thread):
    def __init__(self):
        pass

    def run(self):
        pass

    def getSubscriber(self):
        # add subscriber to game

        pass


class gameServer(threading.Thread):
    def __init__(self):
        pass

    def run(self):
        # receive player input
        # update model
        # send model to subscribers & players
        pass

