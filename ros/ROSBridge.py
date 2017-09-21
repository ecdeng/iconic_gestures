import socket
import json
import traceback
from collections import deque
import threading
import thread
import sys
import signal

def siginit_handler(signum, frame):
	ps.active = False
	ps.join()
	sys.exit()
signal.signal(signal.SIGINT, siginit_handler)

class Vector3():
	def __init__(self, x, y, z):
		self.x = x
		self.y = y
		self.z = z		

class PointStream(threading.Thread):
	def __init__(self):
		threading.Thread.__init__(self)
		self.lock = threading.Lock()
		self.pointsListQueue = deque()
		self.active = False

	def enqueue (self, points):
		self.lock.acquire()
		pointsList = []
		for point in points:
			#pointsList.append(Vector3(point['x'], point['y'], point['z']))
			pointsList.append(point)
		self.pointsListQueue.append(pointsList)
		self.lock.release()



	def dequeue(self):
		self.lock.acquire()
		if self.pointsListQueue:
			rval = self.pointsListQueue.popleft()
		else:
			rval = None
		self.lock.release()
		if rval != None:
			return rval

def run(pointStream):

	while True:
		try:
			TCPSock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
			TCPSock.connect(("localhost", 8888))
			break
		except:
			TCPSock.close()

	print("Connection made")
	pointStream.active = True

	try:
		while True:
			data = TCPSock.recv(1024)
			points = json.loads(data);
			pointStream.enqueue(points['points'])

	except Exception:
		traceback.print_exc()
		TCPSock.close()

def main():
	global ps
	ps = PointStream()
	thread.start_new_thread(run, (ps,))
	while True:
		value = ps.dequeue()
		if value == None:
			continue
		else:
			print(value)


if __name__ == '__main__':
	main()
