import socket
import json
import traceback
from collections import deque
import threading
import thread
import sys
import signal


class Vector3():
	def __init__(self, x, y, z):
		self.x = x
		self.y = y
		self.z = z		

	def toString(self):
		return str(self.x) + " " + str(self.y) + " " + str(self.z)

class PointStream(threading.Thread):
	def __init__(self):
		threading.Thread.__init__(self)
		self.__lock = threading.Lock()
		self.__pointsListQueue = deque()

	def __enqueue (self, points):
		self.__lock.acquire()

		try:
			pointsList = []
			for point in points:
				pointsList.append(Vector3(point['x'], point['y'], point['z']))
			self.__pointsListQueue.append(pointsList)
		finally:
			self.__lock.release()

	def dequeue(self):
		self.__lock.acquire()

		try:
			if self.__pointsListQueue:
				return self.__pointsListQueue.popleft()
			else:
				return
		finally:
			self.__lock.release()

	def empty(self):
		self.__lock.acquire()

		try:
			if self.__pointsListQueue:
				return False
			else:
				return True
		finally:
			self.__lock.release()

	def start(self):
		thread.start_new_thread(self.run, ())

	def run(self):
		while True:
			try:
				TCPSock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
				TCPSock.connect(("localhost", 8888))
				break
			except:
				TCPSock.close()

		print("Connection made")

		try:
			while True:
				data = TCPSock.recv(1024)
				points = json.loads(data);
				self.__enqueue(points['points'])

		except Exception:
			traceback.print_exc()
			TCPSock.close()


