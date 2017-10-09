import socket
import json
import traceback
from collections import deque
import threading
import thread
import sys
import signal

class Point():
	def __init__(self, x, y, z, roll, pitch, yaw):
		self.x = x
		self.y = y
		self.z = z	
		self.roll = roll
		self.pitch = pitch
		self.yaw = yaw	

	def toString(self):
		return str(self.x) + " " + str(self.y) + " " + str(self.z) + " " + str(self.roll) + " " + str(self.pitch) + " " + str(self.yaw)

class PointStream(threading.Thread):
	def __init__(self, ipAddress="localhost", port=8888):
		threading.Thread.__init__(self)
		self.__lock = threading.Lock()
		self.__pointsListQueue = deque()
		self.__ipAddress = ipAddress
		self.__port = port

	def __enqueue (self, points, normals):
		self.__lock.acquire()

		try:
			pointsList = []
			for i in range(0, len(points)):
				point = points[i]
				normal = normals[i]
				pointsList.append(Point(point['x'], point['y'], point['z'], normal['x'], normal['y'], normal['z']))
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
				TCPSock.connect((self.__ipAddress, self.__port))
				break
			except:
				TCPSock.close()

		try:
			while True:
				data = TCPSock.recv(8192)
				splits = data.split('\n')
				for split in splits:
					if not split: 
						continue
					points = json.loads(split);
					self.__enqueue(points['points'], points['normals'])

		except Exception:
			traceback.print_exc()
			TCPSock.close()